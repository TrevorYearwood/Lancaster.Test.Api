using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using Markerstudy.Lancaster.Application.Features.Valuation;
using Microsoft.Extensions.Configuration;

namespace Markerstudy.Lancaster.Infrastructure.Conversion
{
    public class XMLService : IXMLService
    {
        private readonly IConfiguration _configuration;

        public XMLService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateValidXMLAsync(ValuationResponse valuationResponse)
        {
            XmlWriter? writer = null;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();

                var stream = new StringWriter();

                await using var memoryStream = new MemoryStream();
                writer = XmlWriter.Create(memoryStream, settings);
                writer.WriteStartElement("xmlexecute");
                // job element
                writer.WriteStartElement("job");
                writer.WriteElementString("script", _configuration["ScriptTag"] ?? string.Empty);
                writer.WriteElementString("branch", _configuration["BranchTag"] ?? string.Empty);
                writer.WriteElementString("printtype", null);
                writer.WriteEndElement();
                // parameters element
                writer.WriteStartElement("parameters");
                writer.WriteStartElement("yzt");
                writer.WriteElementString("char20.1", _configuration["Char20.1Tag"] ?? string.Empty);
                writer.WriteEndElement();
                writer.WriteEndElement();
                //broomsdata element
                writer.WriteStartElement("broomsdata");
                writer.WriteStartElement("broomsclient");
                writer.WriteStartElement("bcm");
                writer.WriteElementString("refno", valuationResponse.BrokerReference.AsSpan(0, 6).ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("broomspolicy");
                writer.WriteStartElement("bpy");
                writer.WriteElementString("refno", valuationResponse.BrokerReference);
                writer.WriteEndElement();
                writer.WriteStartElement("AVIF");
                writer.WriteElementString("Reg", valuationResponse.RegistrationNumber);
                writer.WriteElementString("Make", valuationResponse.Make);
                writer.WriteElementString("Model", valuationResponse.Model);
                writer.WriteElementString("Title", valuationResponse.Title);
                writer.WriteElementString("Firstname", valuationResponse.FirstName);
                writer.WriteElementString("Surname", valuationResponse.Surname);
                writer.WriteElementString("Addr1", valuationResponse.Address1);
                writer.WriteElementString("Pcode", valuationResponse.Postcode);
                writer.WriteElementString("Origval", valuationResponse.OriginalValue.ToString());
                writer.WriteElementString("Expiry", valuationResponse.Expirydate.ToShortDateString());
                writer.WriteElementString("Estval", valuationResponse.EstimatedValue.ToString());
                writer.WriteElementString("Received", valuationResponse.ReceivedDate.ToShortDateString());
                writer.WriteElementString("Agreedval", valuationResponse.AgreedValue.ToString());
                writer.WriteElementString("Condition", valuationResponse.OverallCondition);
                writer.WriteElementString("Mileage", valuationResponse.Mileage.ToString());
                writer.WriteElementString("Comments", GetYesNoFromAdditionalComments(valuationResponse.AdditionalComments));
                writer.WriteElementString("Mods", GetYesNoFromModification(valuationResponse.Modification));
                writer.WriteElementString("Modsdesc", GetModification(valuationResponse.Modification));
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement(); //broomsdata
                writer.WriteEndElement(); //xmlexecute
                writer.Flush();

                //TODO: Is this the best encoding
                var output = Encoding.ASCII.GetString(memoryStream.ToArray());

                return output.Replace("???", string.Empty).Trim();
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public ValuationErrorDetail? CheckResponseSuccessful(string xml, string brokerReference)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList nodes = doc.GetElementsByTagName("messages");

            if (nodes.Count == 0)
                return new ValuationErrorDetail { BrokerReference = brokerReference, ErrorMessage = "No message return" };

            foreach (XmlNode node in nodes)
            {
                XmlNodeList childNodes = ((XmlElement)node.ParentNode).GetElementsByTagName("error");
                if (childNodes.Count > 0)
                    return new ValuationErrorDetail { BrokerReference = brokerReference, ErrorMessage = childNodes[0].InnerText };
            }

            return null;
        }

        private string GetYesNoFromAdditionalComments(string comments)
        {
            if (string.IsNullOrEmpty(comments?.Trim()))
                return "NO";

            return "YES";
        }

        private string GetYesNoFromModification(string modification)
        {
            if (string.IsNullOrEmpty(modification?.Trim()))
                return "NO";

            var split = modification.Split(" ");
            if (split[0] != null)
                return (split[0].Equals("Yes", StringComparison.InvariantCultureIgnoreCase) 
                        || split[0].Equals("No", StringComparison.InvariantCultureIgnoreCase))
                        ? split[0]
                        : "YES";

            return "NO";
        }

        private string GetModification(string modification)
        {
            if (string.IsNullOrEmpty(modification?.Trim()))
                return "NO";

            var split = modification.Split(" ");
            if (split[0] != null)
                return modification.Replace(split[0], string.Empty)?.TrimStart()?.TrimEnd() ?? string.Empty;

            return string.Empty;
        }
    }
}