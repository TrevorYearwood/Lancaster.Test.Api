using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper.TypeConversion;

namespace Markerstudy.Lancaster.Application.Exceptions
{
    public class RowValidationExeception : ApplicationException
    {
        public List<string> ValidationErrors { get; set; } = new List<string>();

        public RowValidationExeception(List<TypeConverterException> rowExceptions)
        {
            var exceptionMessage = new StringBuilder();

            foreach (var exception in rowExceptions)
            {
                var spiltException = exception.Message.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);

                var rowNumber = spiltException.FirstOrDefault(r => r.Contains("Row", StringComparison.InvariantCultureIgnoreCase));

                exceptionMessage.AppendLine($"{spiltException[0]} {rowNumber?.Trim()}"); 
            }                 
            
            ValidationErrors.Add($"Error reading file, please amend and resubmit. {exceptionMessage}");          
        }
    }
}
