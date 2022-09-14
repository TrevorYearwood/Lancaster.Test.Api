using System.Threading.Tasks;

namespace Markerstudy.Lancaster.Application.Contracts.Infrastructure
{
    public interface IWsdlService
    {
        Task<string> PostToOpenGiAsync(string requestXml);
    }
}