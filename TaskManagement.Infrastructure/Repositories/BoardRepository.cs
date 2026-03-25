using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly AppDbContext db;

        public BoardRepository(AppDbContext _db)
        {
            db = _db;
        }

        public async Task<IEnumerable<Board>> GetAllAsync()
        {
            var Boards = await db.Boards.ToListAsync();
            return Boards;
        }

        public async Task<IEnumerable<Board>> GetAllByProjectAsync(Guid projectId)
        {
            var Boards = await db.Boards.Where(d => d.ProjectId == projectId).ToListAsync();
            return Boards;
        }

        public async Task<Board> GetByIdAsync(Guid Id)
        {
            return await db.Boards.FirstOrDefaultAsync(d => d.Id == Id);
        }

        public async Task AddAsync(Board entity)
        {
            //Board board = new()
            //{
            //    Name = entity.Name,
            //    ProjectId = entity.ProjectId
            //};
            await db.Boards.AddAsync(entity);
        }

        public async Task DeleteAsync(Board entity)
        {
            db.Boards.Remove(entity);
        }
        
        public async Task UpdateAsync(Board updated)
        {
            db.Boards.Update(updated);
        }

        public async Task SaveChangesAsync() => await db.SaveChangesAsync();
    }
}
