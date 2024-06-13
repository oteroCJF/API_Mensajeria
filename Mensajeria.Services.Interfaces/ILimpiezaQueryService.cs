using Limpieza.Service.Queries.DTOs;
using Service.Common.Collection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Limpieza.Services.Interfaces
{
    public interface ILimpiezaQueryService
    {
        Task<DataCollection<CedulasEvaluacionDto>> GetAllAsync(int page, int take, IEnumerable<int> products = null);
        Task<CedulasEvaluacionDto> GetAsync(int id);
    }
}
