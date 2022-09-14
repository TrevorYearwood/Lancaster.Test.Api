using System;

namespace Markerstudy.Lancaster.Application.Features.Common
{
    public abstract class AuditableResponse : BaseResponse
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
