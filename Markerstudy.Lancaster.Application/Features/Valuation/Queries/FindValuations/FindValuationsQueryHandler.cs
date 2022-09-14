using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Markerstudy.Lancaster.Application.Features.Valuation.Queries.FindValuations
{
    public class FindValuationsQueryHandler : IRequestHandler<FindValuationsQuery, ValuationQueryResponse>
    {
        private readonly ILogger<FindValuationsQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ICSVService _fileService;
        private readonly IXMLService _xmlService;
        private readonly IWsdlService _wsdlService;

        public FindValuationsQueryHandler(
            ILogger<FindValuationsQueryHandler> logger,
            IMapper mapper,
            ICSVService fileService,
            IXMLService xmlService,
            IWsdlService wsdlService)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            _fileService = fileService ?? throw new System.ArgumentNullException(nameof(fileService));
            _xmlService = xmlService ?? throw new System.ArgumentNullException(nameof(xmlService));
            _wsdlService = wsdlService ?? throw new System.ArgumentNullException(nameof(wsdlService));
        }

        public async Task<ValuationQueryResponse> Handle(FindValuationsQuery query, CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            timer.Start();

            var valuationQueryResponse = new ValuationQueryResponse();

            await ValidateQueryRequest(query, valuationQueryResponse, cancellationToken);

            if (valuationQueryResponse.Success)
            {
                var results = new List<ValuationResultResponse>();

                var valuations = await _fileService.GetRecordsAsync(query.Filename);

                var valuationResponses = _mapper.Map<List<ValuationResponse>>(valuations);

                foreach (var item in valuationResponses)
                {
                    var xml = await _xmlService.CreateValidXMLAsync(item);
                    var result = await _wsdlService.PostToOpenGiAsync(xml);
                    UpdateValuationResult(results, result, item);
                }

                timer.Stop();

                var formattedResult = DisplayResults(results, timer);

                valuationQueryResponse.Message = formattedResult;
            }

            return valuationQueryResponse;
        }

        private async Task ValidateQueryRequest(FindValuationsQuery query, ValuationQueryResponse valuationQueryResponse, CancellationToken cancellationToken)
        {
            var validator = new FindValuationQueryValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                valuationQueryResponse.Success = false;
                valuationQueryResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    valuationQueryResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }
        }

        private void UpdateValuationResult(List<ValuationResultResponse> valuationResults, string xml, ValuationResponse item)
        {
            var resultResponse = new ValuationResultResponse();

            var errorResult = _xmlService.CheckResponseSuccessful(xml, item.BrokerReference);

            if (errorResult == null)
                resultResponse.SuccessfulValuations.Add(item);
            else
            {
                resultResponse.ErrorDetails.Add(errorResult);
                resultResponse.FailedValuations.Add(item);
            }

            valuationResults.Add(resultResponse);
        }

        private string DisplayResults(List<ValuationResultResponse> result, Stopwatch timer)
        {
            var successful = result.SelectMany(r => r.SuccessfulValuations).Distinct().ToList();
            var failed = result.SelectMany(r => r.FailedValuations).Distinct().ToList();
            var errorDetails = result.SelectMany(r => r.ErrorDetails).Distinct().ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Total records processed - {result.Count} - this took {timer.Elapsed.TotalSeconds} seconds");
            stringBuilder.AppendLine($"Total records successfully processes - {successful.Count}");
            stringBuilder.AppendLine($"Total records failed - {failed.Count}");

            foreach (var item in errorDetails)
            {
                stringBuilder.AppendLine($"Broker Reference: {item?.BrokerReference} - Error: {item?.ErrorMessage} ");
            }

            var fullString = stringBuilder.ToString();
            _logger.LogInformation(fullString);

            return fullString;
        }
    }
}