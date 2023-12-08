using FMS.Services.ADFPipelines.Interfaces;
using FMS.Services.ADFPipelines.Models;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Rest.Azure.Authentication;
using FMS.Services.ADFPipelines.Helpers;
using System.Collections.Concurrent;

namespace FMS.Services.ADFPipelines.Service
{
    public class ADFPipeineService : IADFPipeineService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenantId;
        private readonly string _subscriptionId;
        private readonly string _resourceGroupName;
        private readonly string _dataFactoryName;
        public ADFPipeineService(IConfiguration configuration)
        {
            _clientId = configuration.GetValue<string>("ADFPipeline:clientId");
            _clientSecret = configuration.GetValue<string>("ADFPipeline:clientSecret");
            _tenantId = configuration.GetValue<string>("ADFPipeline:tenantId");
            _subscriptionId = configuration.GetValue<string>("ADFPipeline:subscriptionId");
            _resourceGroupName = configuration.GetValue<string>("ADFPipeline:resourceGroupName");
            _dataFactoryName = configuration.GetValue<string>("ADFPipeline:dataFactoryName");
        }

        public async Task<ResponseDTO> RerunPipelineAsync(string pipelineName)
        {
            try
            {
                ResponseDTO res = new ResponseDTO();
                var token = await GetAccessTokenAsync(_clientId, _clientSecret, _tenantId);


                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    string baseUrl = $"https://management.azure.com/subscriptions/{_subscriptionId}/resourceGroups/{_resourceGroupName}/providers/Microsoft.DataFactory/factories/{_dataFactoryName}/pipelines/{pipelineName}/createRun?api-version=2018-06-01";
                    // Perform the rerun operation
                    var response = await client.PostAsync(baseUrl, new StringContent(""));

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Pipeline run rerun initiated successfully!");
                        /*res.StatusCode = response.StatusCode.ToString();
                        res.StatusMessage = response.IsSuccessStatusCode.ToString();*/
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                        //res.StatusCode = response.StatusCode.ToString();
                        
                    }
                    res.StatusCode = response.StatusCode.ToString();
                    res.StatusMessage = response.IsSuccessStatusCode.ToString();
                }
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return new ResponseDTO(); 
        }

        async Task<List<PipelineDTO>> IADFPipeineService.GetPipelinesDataAsync()
        {
            

            var serviceClientCredentials = ApplicationTokenProvider.LoginSilentAsync(_tenantId, _clientId, _clientSecret).Result;

            var dataFactoryManagementClient = new DataFactoryManagementClient(serviceClientCredentials)
            {
                SubscriptionId = _subscriptionId
            };
            int i = 1;

            // Get a list of pipelines in the specified Data Factory
            var pipelines = await dataFactoryManagementClient.Pipelines.ListByFactoryAsync(_resourceGroupName, _dataFactoryName);

            List<string> pipelineStatuses = new List<string>();
            List<PipelineDTO> pipelineData = new List<PipelineDTO>();

           

            string continuationToken = null;
            PipelineRunsQueryResponse pipelineRuns;

            // Get the latest run status of the pipeline
            do
            {
                // Get the latest run status of the pipeline with pagination
                pipelineRuns = await dataFactoryManagementClient.PipelineRuns.QueryByFactoryAsync(
                    _resourceGroupName,
                    _dataFactoryName,
                    new RunFilterParameters
                    {
                        LastUpdatedAfter = DateTime.UtcNow.AddMonths(-1), // Adjust the time range as needed
                        LastUpdatedBefore = DateTime.UtcNow,
                        ContinuationToken= continuationToken, // Pass the continuation token
                        
                    }
                    
                );

                var result = new ConcurrentBag<PipelineDTO>();

                Parallel.ForEach(pipelineRuns.Value, item =>
                {
                    PipelineDTO pipelineDTO = new PipelineDTO
                    {
                        PipelineName = item.PipelineName,
                        RunStart = (DateTime)item.RunStart,
                        RunEnd = (DateTime)item.RunEnd,
                        DurationInMS = (int)item.DurationInMs,
                        RunID = item.RunId,
                        ErrorMessage = item.Message,
                        Status = item.Status,
                        Parameters = (Dictionary<string, string>)item.Parameters
                    };
                    result.Add(pipelineDTO);
                });
                pipelineData.AddRange(result);
                //foreach (var item in pipelineRuns.Value)
                //{
                //    // Process the pipeline run data
                //    PipelineDTO pipelineDTO = new PipelineDTO
                //    {
                //        PipelineName = item.PipelineName,
                //        RunStart = (DateTime)item.RunStart,
                //        RunEnd = (DateTime)item.RunEnd,
                //        DurationInMS = (int)item.DurationInMs,
                //        RunID = item.RunId,
                //        ErrorMessage = item.Message,
                //        Status = item.Status,
                //        Parameters = (Dictionary<string, string>)item.Parameters
                //    };
                //    pipelineData.Add(pipelineDTO);
                //    //Console.WriteLine($"Pipeline Run ID: {pipelineRun.RunId}" + " i -> "+i++);
                //}

                // Update the continuation token for the next iteration
                continuationToken = pipelineRuns.ContinuationToken;

            } while (!string.IsNullOrEmpty(continuationToken));

            pipelineData = pipelineData.OrderByDescending(p => p.RunStart).ToList();
            return pipelineData;
        }

        private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string tenantId)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestBody = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "resource", "https://management.azure.com/" },
                };

                HttpResponseMessage response = await client.PostAsync($"https://login.microsoftonline.com/{tenantId}/oauth2/token", new FormUrlEncodedContent(requestBody));

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                    return tokenResponse.access_token;
                }
                else
                {
                    throw new Exception($"Error getting access token: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
        }
    }
}
