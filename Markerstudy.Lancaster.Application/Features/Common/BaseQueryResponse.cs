using System.Collections.Generic;

namespace Markerstudy.Lancaster.Application.Features.Common
{
    public abstract class BaseQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> ValidationErrors { get; set; }

        public BaseQueryResponse()
        {
            Success = true;
        }

        public BaseQueryResponse(bool success = true, string message = null)
        {
            Success = success;
            Message = message;
        }
    }
}
