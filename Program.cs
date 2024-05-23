using ApigeeToMulesoftMigrationUtil.Models;
using ApigeeToMulesoftMigrationUtil.Providers;
using ApigeeToMulesoftMigrationUtil.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApigeeToMulesoftMigrationUtil
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting up ...");

                ConfigService configService = new ConfigService();

                if (configService == null)
                    throw new ArgumentNullException("Invalid Configuration");

                string metaDataPath = Path.Combine(configService.ExportConfiguration.Folder, $"Extract_{DateTime.UtcNow.ToString("ddMMyyhhmmss")}");
                configService.ValiadteConfig();
                var container = RegisterDependencies(configService, metaDataPath);

                var extracter = container.GetRequiredService<IApigeeMetaDataExtracter>();
                extracter.OnStatusPulished += Extracter_OnStatusPulished;
                await extracter.Extract();

                if (configService.ApigeeConfiguration.ConfigDir == null)
                    Console.WriteLine($"Meta Data Extracted at location: {metaDataPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("press any key to continue...");
            Console.ReadKey();
        }

        private static void Extracter_OnStatusPulished(string msg)
        {
            Console.WriteLine(msg);
        }

        private static ServiceProvider RegisterDependencies(ConfigService configService, string metaDataPath)
        {
            var serviceCollection = new ServiceCollection()
                    .AddSingleton<IConfigService>(configService)
                    .AddSingleton<IApigeeConfiguration>(configService.ApigeeConfiguration)
                    .AddSingleton<IApigeeManagementApiService, ApigeeManagementApiService>()
                    .AddSingleton<IApigeeMetaDataExtracter, ApigeeMetaDataExtracter>();

            if (configService.ApigeeConfiguration.ConfigDir != null)
            {
                serviceCollection.AddSingleton<IBundleProvider, ApigeeFileBundleProvider>(
                    serviceProvider => new ApigeeFileBundleProvider(configService.ApigeeConfiguration.ConfigDir));
            }
            else
            {
                serviceCollection.AddSingleton<IBundleProvider, ApigeeOnlineBundleProvider>(
                    serviceProvider => new ApigeeOnlineBundleProvider(metaDataPath,
                    serviceProvider.GetRequiredService<IApigeeManagementApiService>()));
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}