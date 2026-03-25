using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface IBoardRepository : IGenericRepository<Board>
    {
        Task<IEnumerable<Board>> GetAllByProjectAsync(Guid projectId);
    }
}
