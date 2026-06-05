using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly AppDbContext db;

        public ProjectMemberRepository(AppDbContext _db)
        {
            db = _db;
        }

        public async Task AddAsync(ProjectMember entity)
        {
            await db.ProjectMembers.AddAsync(entity);
        }

        public async Task DeleteAsync(ProjectMember entity)
        {
            db.ProjectMembers.Remove(entity);
        }

        public async Task<IEnumerable<ProjectMember>> GetAllAsync()
        {
            return await db.ProjectMembers.ToListAsync();
        }

        public async Task<ProjectMember> GetByIdAsync(Guid Id)
        {
            return await db.ProjectMembers.FindAsync(Id);
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectMember entity)
        {
            db.ProjectMembers.Update(entity);
        }
    }
}
