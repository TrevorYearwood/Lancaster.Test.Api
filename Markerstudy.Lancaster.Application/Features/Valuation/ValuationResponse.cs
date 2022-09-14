using System;

namespace Markerstudy.Lancaster.Application.Features.Valuation
{
    public class ValuationResponse
    {
        public string Brand { get; set; }
        public string BrokerReference { get; set; }
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address1 { get; set; }
        public string Postcode { get; set; }
        public int OriginalValue { get; set; }
        public int EstimatedValue { get; set; }
        public int AgreedValue { get; set; }
        public DateTime Expirydate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string OverallCondition { get; set; }
        public int Mileage { get; set; }
        public string AdditionalComments { get; set; }
        public string Modification { get; set; }
    }
}