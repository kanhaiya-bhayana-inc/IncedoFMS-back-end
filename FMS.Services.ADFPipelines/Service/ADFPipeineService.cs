using FMS.Services.ADFPipelines.Interfaces;
using FMS.Services.ADFPipelines.Models;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Rest.Azure.Authentication;
using FMS.Services.ADFPipelines.Helpers;

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


        async Task<List<PipelineDTO>> IADFPipeineService.GetPipelinesDataAsync()
        {
            

            var serviceClientCredentials = ApplicationTokenProvider.LoginSilentAsync(_tenantId, _clientId, _clientSecret).Result;

            var dataFactoryManagementClient = new DataFactoryManagementClient(serviceClientCredentials)
            {
                SubscriptionId = _subscriptionId
            };

            // Get a list of pipelines in the specified Data Factory
            var pipelines = await dataFactoryManagementClient.Pipelines.ListByFactoryAsync(_resourceGroupName, _dataFactoryName);

            List<string> pipelineStatuses = new List<string>();
            List<PipelineDTO> pipelineData = new List<PipelineDTO>();

            foreach (var pipeline in pipelines)
            {
                // Get the latest run status of the pipeline
                var pipelineRuns = await dataFactoryManagementClient.PipelineRuns.QueryByFactoryAsync(
                    _resourceGroupName,
                    _dataFactoryName,
                    new RunFilterParameters
                    {
                        LastUpdatedAfter = DateTime.UtcNow.AddMonths(-1), // Adjust the time range as needed
                        LastUpdatedBefore = DateTime.UtcNow
                    }
                );

                var latestRun = pipelineRuns.Value
                    .Where(run => run.PipelineName.Equals(pipeline.Name, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(run => run.RunEnd)
                    .FirstOrDefault();

                if (latestRun != null)
                {
                    pipelineStatuses.Add($"Pipeline: {latestRun.PipelineName} | Run start: {latestRun.RunStart} | Run end: {latestRun.RunEnd} | Duration: {latestRun.DurationInMs} | Status: {latestRun.Status} | RunID: {latestRun.RunId}");
                    if (latestRun.Parameters != null)
                    {
                        foreach (var parameter in latestRun.Parameters)
                        {
                            pipelineStatuses.Add($"{parameter.Key}: {parameter.Value}");
                        }
                    }
                    if (latestRun.Status == "Failed")
                    {
                        pipelineStatuses.Add($"Error message: {latestRun.Message}");
                    }
                    PipelineDTO pipelineDTO = new PipelineDTO
                    {
                        PipelineName = latestRun.PipelineName,
                        RunStart = (DateTime)latestRun.RunStart,
                        RunEnd = (DateTime)latestRun.RunEnd,
                        DurationInMS = (int)latestRun.DurationInMs,
                        RunID = latestRun.RunId,
                        ErrorMessage = latestRun.Message,
                        Status = latestRun.Status,
                        Parameters = (Dictionary<string, string>)latestRun.Parameters
                    };
                    pipelineData.Add( pipelineDTO );

                    //Console.WriteLine($"Pipeline: {pipeline.Name}, Status: {latestRun.Status} + RunID: {pipeline.Etag}");
                }
                else
                {
                    //pipelineStatuses.Add($"Pipeline: {pipeline.Name}, Status: No runs found + RunID: { pipeline.Etag}");
                    //Console.WriteLine($"Pipeline: {pipeline.Name}, Status: No runs found + RunID: {pipeline.Etag}");
                }
            }

            // Print or use the pipeline statuses as needed
            foreach (var status in pipelineStatuses)
            {
                Console.WriteLine(status);
            }
            var result = pipelineStatuses;

            return pipelineData;
        }
    }
}
