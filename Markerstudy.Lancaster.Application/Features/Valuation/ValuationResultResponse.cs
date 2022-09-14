using System.Collections.Generic;

namespace Markerstudy.Lancaster.Application.Features.Valuation
{
    public class ValuationResultResponse
    {
        public List<ValuationResponse> SuccessfulValuations { get; set; } = new List<ValuationResponse>();
        public List<ValuationResponse> FailedValuations { get; set; } = new List<ValuationResponse>();
        public List<ValuationErrorDetail> ErrorDetails { get; set; } = new List<ValuationErrorDetail>(); 
    }
}
