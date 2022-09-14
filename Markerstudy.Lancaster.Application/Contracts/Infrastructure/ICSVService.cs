using System.Collections.Generic;
using System.Threading.Tasks;
using Markerstudy.Lancaster.Domain.Models;

namespace Markerstudy.Lancaster.Application.Contracts.Infrastructure
{
    public interface ICSVService
    {
        Task<IEnumerable<Valuation>> GetRecordsAsync(string filename);
    }
}