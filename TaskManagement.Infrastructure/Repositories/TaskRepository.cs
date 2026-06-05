using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext db;

        public TaskRepository(AppDbContext _db)
        {
            db = _db;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await db.TaskItems.ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByBoardAsync(Guid Id)
        {
            return await db.TaskItems.Where(d => d.BoardId == Id).ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(Guid Id)
        {
            return await db.TaskItems.Where(d => d.Id == Id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TaskItem entity)
        {
            await db.TaskItems.AddAsync(entity);
        }

        public async Task UpdateAsync(TaskItem entity)
        {
            db.TaskItems.Update(entity);
        }

        public async Task DeleteAsync(TaskItem entity)
        {
            db.TaskItems.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserAsync(string userId)
        {
            var myTasks = await db.TaskItems.Where(d => d.AssignedToUserId == userId || d.Board.Project.ProjectMembers.Any(m => m.UserId == userId)).ToListAsync();
            return myTasks;
        }

        public async Task<IEnumerable<TaskItem>> GetUserTasksAsync(string userId)
        {
            var myTasks = await db.TaskItems.Where(d => d.AssignedToUserId == userId).ToListAsync();
            return myTasks;
        }
    }
}
