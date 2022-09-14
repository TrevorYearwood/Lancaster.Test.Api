using System.Collections.Generic;
using Markerstudy.Lancaster.Application.Features.Common;

namespace Markerstudy.Lancaster.Application.Features.Valuation
{
    public class ValuationQueryResponse : BaseQueryResponse
    {
        public List<ValuationResultResponse> ValuationResultResponses { get; set; }
    }
}
