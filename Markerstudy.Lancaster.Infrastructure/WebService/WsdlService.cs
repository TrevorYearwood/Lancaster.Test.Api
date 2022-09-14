using System;
using System.Threading.Tasks;
using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Logging;
using OpenGI;

namespace Markerstudy.Lancaster.Infrastructure.WebService
{
    public class WsdlService : IWsdlService
    {
        private readonly ILogger<WsdlService> _logger;

        public WsdlService(ILogger<WsdlService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<string> PostToOpenGiAsync(string requestXml)
        {
            if (string.IsNullOrEmpty(requestXml))
            {
                _logger.LogWarning("Requested XML is empty");
                throw new ArgumentNullException(nameof(requestXml));
            }

            try
            {
                using (var client = new OpenInterchangeClient())
                {
                    // These are the UAT versions of these variables - so probably should use config to select
                    // between UAT and LIVE versions depending on enviroment
                    string oicMarsReference = "THAM021";
                    string oicBNumber = "Z00200";
                    string oicLicenceKey = "JVT8FXSOE";

                    _logger.LogInformation("Request - {requestXml}", requestXml);

                    var openGIResponse = await client.xStreamMessageAsync(oicMarsReference, oicBNumber, oicLicenceKey, requestXml, 200);

                    var response = openGIResponse?.@return;

                    _logger.LogInformation("OpenGI Response - {response}", response);

                    return response ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error calling WSDL Service", ex.Message);
                throw;
            }
        }
    }
}