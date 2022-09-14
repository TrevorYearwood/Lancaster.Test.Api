using MediatR;

namespace Markerstudy.Lancaster.Application.Features.Valuation.Queries.FindValuations
{
    public class FindValuationsQuery : IRequest<ValuationQueryResponse>
    {
        public string Filename { get; set; }
    }
}