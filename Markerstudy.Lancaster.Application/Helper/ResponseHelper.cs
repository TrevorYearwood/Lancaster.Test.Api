using System;
using Markerstudy.Lancaster.Application.Features.Valuation;

namespace Markerstudy.Lancaster.Application.Helper
{
    public static class ResponseHelper
    {
        public static ValuationResponse GetValuationResponse()
        {
            return new ValuationResponse
            {
                BrokerReference = "TERX07PC01",
                RegistrationNumber = "ABC123L",
                Make = "MG",
                Model = "B",
                Title = "Miss",
                FirstName = "Rebecca",
                Surname = "Test",
                Address1 = "Lancaster House",
                Postcode = "PE27 4ZB",
                OriginalValue = 5000,
                Expirydate = new DateTime(2023, 07, 27),
                EstimatedValue = 6500,
                ReceivedDate = new DateTime(2022, 07, 27),
                AgreedValue = 6500,
                OverallCondition = "Very Good",
                Mileage = 131689,
                AdditionalComments = "No"
            };
        }
    }
}