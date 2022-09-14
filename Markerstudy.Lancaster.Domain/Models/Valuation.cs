using System;
using CsvHelper.Configuration.Attributes;

namespace Markerstudy.Lancaster.Domain.Models
{
    public class Valuation
    {
        public string Brand { get; set; }

        [Name("Broker_Reference")]
        public string BrokerReference { get; set; }

        [Name("Registration_Number")]
        public string RegistrationNumber { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Title { get; set; }

        [Name("First_Name")]
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Address1 { get; set; }

        public string Postcode { get; set; }

        [Name("Original Value")]
        public int OriginalValue { get; set; }

        [Name("Estimated Value")]
        public int EstimatedValue { get; set; }

        [Name("Agreed Value")]
        public int AgreedValue { get; set; }

        public DateTime Expirydate { get; set; }

        [Name("Received Date")]
        public DateTime ReceivedDate { get; set; }

        [Name("Overall Condition")]
        public string OverallCondition { get; set; }

        public int Mileage { get; set; }

        [Name("Additional Comments")]
        public string AdditionalComments { get; set; }

        //TODO: Index used a column name blank, very fragile and needs reviewing
        [Index(18)]
        public string Modification { get; set; }
    }
}
