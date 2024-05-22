using ApigeeToMulesoftMigrationUtil.Models;
using Newtonsoft.Json;
using System.IO.Compression;

namespace ApigeeToMulesoftMigrationUtil.Services
{
    public class ApigeeManagementApiService : IApigeeManagementApiService
    {
        private readonly HttpClient _client;
        private readonly string _authenticationBaseUrl;
        //private readonly IProxyMetaDataDataAccess _proxyMetaDataDataAccess;
        private string _authenticationCode;

        public ApigeeManagementApiService(IApigeeConfiguration apigeeConfiguration)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{apigeeConfiguration.ManagementBaseUrl}/v1/organizations/{apigeeConfiguration.OrganizationName}/");
            _authenticationBaseUrl = apigeeConfiguration.AuthenticationBaseUrl;
            //_proxyMetaDataDataAccess = proxyMetaDataDataAccess;

            AuthenticationToken = string.Empty;
            Username = string.Empty;
            Password = string.Empty;

            ProxyName = apigeeConfiguration.ProxyOrProductName;
            Environment = apigeeConfiguration.EnvironmentName;
        }

        public string AuthenticationToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string? Environment { get; private set; }
        public string ProxyName { get; private set; }

        public async Task<ApiProductMetaData> GetApiProducts()
        {
            await ResetHttpClient();
            HttpResponseMessage apiProxyResponse = await _client.GetAsync($"apiproducts");
            apiProxyResponse.EnsureSuccessStatusCode();
            var apiMetaData = JsonConvert.DeserializeObject<ApiProductMetaData>(await apiProxyResponse.Content.ReadAsStringAsync());
            return apiMetaData;
        }

        public async Task<ApigeeEntityModel> GetApiProxyByName(string proxyName)
        {
            await ResetHttpClient();
            HttpResponseMessage apiProxyResponse = await _client.GetAsync($"apis/{proxyName}");
            apiProxyResponse.EnsureSuccessStatusCode();
            var apiMetaData = JsonConvert.DeserializeObject<ApigeeEntityModel>(await apiProxyResponse.Content.ReadAsStringAsync());
            return apiMetaData;
        }

        public async Task<ApigeeTargetServerModel> GetTargetServerByName(string targetServerName, string environment)
        {
            await ResetHttpClient();
            HttpResponseMessage apiProxyResponse = await _client.GetAsync($"environments/{environment}/targetservers/{targetServerName}");
            apiProxyResponse.EnsureSuccessStatusCode();
            var apiMetaData = JsonConvert.DeserializeObject<ApigeeTargetServerModel>(await apiProxyResponse.Content.ReadAsStringAsync());
            return apiMetaData;
        }

        public async Task<ApiProductMetaData> GetApiProductByName(string productName)
        {
            await ResetHttpClient();
            HttpResponseMessage apiProxyResponse = await _client.GetAsync($"apiproducts/{productName}");
            apiProxyResponse.EnsureSuccessStatusCode();
            var apiMetaData = JsonConvert.DeserializeObject<ApiProductMetaData>(await apiProxyResponse.Content.ReadAsStringAsync());
            return apiMetaData;
        }

        public async Task<KeyValueMapModel> GetKeyValueMapByName(string proxyName, string environment, string mapIdentifier)
        {
            await ResetHttpClient();

            KeyValueMapModel result = null;

            HttpResponseMessage proxyKvmResponse = await _client.GetAsync($"apis/{proxyName}/keyvaluemaps/{mapIdentifier}");
            if (proxyKvmResponse.StatusCode == System.Net.HttpStatusCode.OK)
                result = JsonConvert.DeserializeObject<KeyValueMapModel>(await proxyKvmResponse.Content.ReadAsStringAsync());

            HttpResponseMessage environmentKvmResponse = await _client.GetAsync($"environments/{environment}/keyvaluemaps/{mapIdentifier}");
            if (environmentKvmResponse.StatusCode == System.Net.HttpStatusCode.OK)
                result = JsonConvert.DeserializeObject<KeyValueMapModel>(await environmentKvmResponse.Content.ReadAsStringAsync());

            HttpResponseMessage organizationKvmResponse = await _client.GetAsync($"keyvaluemaps/{mapIdentifier}");
            if (organizationKvmResponse.StatusCode == System.Net.HttpStatusCode.OK)
                result = JsonConvert.DeserializeObject<KeyValueMapModel>(await organizationKvmResponse.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<string> DownloadApiProxyBundle(string basePath, string proxyName, int revision)
        {
            await ResetHttpClient();
            HttpResponseMessage apiRevisionResponse = await _client.GetAsync($"apis/{proxyName}/revisions/{revision}?format=bundle");

            Stream inputStream = await apiRevisionResponse.Content.ReadAsStreamAsync();
            string proxyPath = Directory.CreateDirectory(Path.Combine(basePath, proxyName)).FullName;
            string zipPath = Path.Combine(proxyPath, $"{proxyName}.zip");
            using (FileStream outputFileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write))
            {
                inputStream.CopyTo(outputFileStream);
            }
            ZipFile.ExtractToDirectory(zipPath, proxyPath, true);
            return proxyPath;
        }


        //TODO: replace with a new model
        public async Task<ApigeeEntityModel> GetSharedFlowByName(string sharedFlowName)
        {
            await ResetHttpClient();
            HttpResponseMessage apiProxyResponse = await _client.GetAsync($"sharedflows/{sharedFlowName}");
            apiProxyResponse.EnsureSuccessStatusCode();
            var apiMetaData = JsonConvert.DeserializeObject<ApigeeEntityModel>(await apiProxyResponse.Content.ReadAsStringAsync());
            return apiMetaData;
        }

        public async Task<string> DownloadSharedFlowBundle(string basePath, string sharedFlowName, int revision)
        {
            await ResetHttpClient();
            HttpResponseMessage apiRevisionResponse = await _client.GetAsync($"sharedflows/{sharedFlowName}/revisions/{revision}?format=bundle");

            Stream inputStream = await apiRevisionResponse.Content.ReadAsStreamAsync();
            string sharedFlowPath = Directory.CreateDirectory(Path.Combine(basePath, sharedFlowName)).FullName;
            string zipPath = Path.Combine(sharedFlowPath, $"{sharedFlowName}.zip");
            using (FileStream outputFileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write))
            {
                inputStream.CopyTo(outputFileStream);
            }
            ZipFile.ExtractToDirectory(zipPath, sharedFlowPath, true);
            return sharedFlowPath;
        }

        private async Task ResetHttpClient()
        {
            string bearerToken = await GetBearerToken();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

        }

        private async Task<string> GetBearerToken()
        {
            if (!string.IsNullOrEmpty(AuthenticationToken))
                return await GetBearerTokenFromOneTimeAuthToken();
            else
                return await GetBearerTokenFromUsernameAndPassword();
        }
        private async Task<string> GetBearerTokenFromOneTimeAuthToken()
        {
            if (string.IsNullOrEmpty(_authenticationCode))
            {
                HttpClient client = new HttpClient();
                // NOT A REAL TOKEN.  THIS IS A DEFAULT TOKEN THAT IS REQUIRED FOR ALL CLIENTS BY THE API
                // See: https://docs.apigee.com/api-platform/system-administration/management-api-tokens#request-headers
                client.DefaultRequestHeaders.Add("Authorization", "Basic ZWRnZWNsaTplZGdlY2xpc2VjcmV0");

                HttpResponseMessage authTokenResponse = await client.PostAsync($"{_authenticationBaseUrl}/oauth/token?grant_type=password&passcode={AuthenticationToken}", null);
                authTokenResponse.EnsureSuccessStatusCode();
                var authresponse = JsonConvert.DeserializeObject<AuthToken>(await authTokenResponse.Content.ReadAsStringAsync());
                _authenticationCode = authresponse.access_token;
            }

            return _authenticationCode;
        }

        private async Task<string> GetBearerTokenFromUsernameAndPassword()
        {
            if (string.IsNullOrEmpty(_authenticationCode))
            {
                HttpClient client = new HttpClient();

                // NOT A REAL TOKEN.  THIS IS A DEFAULT TOKEN THAT IS REQUIRED FOR ALL CLIENTS BY THE API
                // See: https://docs.apigee.com/api-platform/system-administration/management-api-tokens#request-headers
                client.DefaultRequestHeaders.Add("Authorization", "Basic ZWRnZWNsaTplZGdlY2xpc2VjcmV0");

                HttpResponseMessage authTokenResponse = await client.PostAsync($"{_authenticationBaseUrl}/oauth/token?grant_type=password&username={Username}&password={Password}", null);
                authTokenResponse.EnsureSuccessStatusCode();
                var authresponse = JsonConvert.DeserializeObject<AuthToken>(await authTokenResponse.Content.ReadAsStringAsync());
                _authenticationCode = authresponse.access_token;
            }

            return _authenticationCode;
        }
    }
}
