using Markerstudy.Lancaster.Domain.Common;

namespace Markerstudy.Lancaster.Domain.Models
{
    public class File : AuditableEntity
    {
        public string Name { get; set; }

        public string Message { get; set; }
    }
}
