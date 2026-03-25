using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories
{
    public class ProjectRespository : IProjectRespository
    {
        private readonly AppDbContext db;

        public ProjectRespository(AppDbContext _db)
        {
            db = _db;
        }

        public async Task<IEnumerable<Project>> GetAllAsync(string UserId)
        {
            var projects = await db.Projects.Where(d => d.OwnerId == UserId).ToListAsync();
            return projects;
        }

        public async Task<Project> GetByIdAsync(Guid Id)
        {
            return await db.Projects.FirstOrDefaultAsync(d => d.Id == Id);
        }

        public async Task AddAsync(Project project)
        {
            await db.Projects.AddAsync(project);
        }

        public async Task DeleteAsync(Project project)
        {
            db.Projects.Remove(project);
        }

        public async Task SaveChangesAsync() => await db.SaveChangesAsync();

    }
}
