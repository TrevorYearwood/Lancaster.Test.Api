using System.Threading.Tasks;
using Markerstudy.Lancaster.Application.Features.Valuation;

namespace Markerstudy.Lancaster.Application.Contracts.Infrastructure
{
    public interface IXMLService
    {
        Task<string> CreateValidXMLAsync(ValuationResponse valuationResponse);

        ValuationErrorDetail CheckResponseSuccessful(string xml, string brokerReference);
    }
}