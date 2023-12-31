﻿using Microsoft.EntityFrameworkCore;
using TP_CS.Business.IRepositories;
using TP_CS.Business.Models;
using TP_CS.Data.Context;
using UserTask = TP_CS.Business.Models.UserTask;

namespace TP_CS.Data.Repositories
{
    public class DatabaseTaskRepository : ITaskRepository
    {
        private readonly MyDbContext _dbContext;

        public DatabaseTaskRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public void CreateTask(UserTask newTask, Project project, User users)
        {
            //_dbContext.Users?.FirstOrDefault(u => u.Id == newTask.UserId);
            _dbContext.Users?.FirstOrDefault(u => u.Id == users.Id)?.UserTasks.Add(newTask);
            _dbContext.Projects?.FirstOrDefault(p => p.Id == project.Id)?.UserTasks.Add(newTask);
            _dbContext.Tasks?.Add(newTask);
            _dbContext.SaveChanges();
        }

        public IEnumerable<UserTask>? GetTasks()
        {
            return _dbContext.Tasks?
                .Include(b => b.Tags)
                .ToList();
        }

        public void DeleteTask(long taskId)
        {
            var task = _dbContext.Tasks?.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new InvalidOperationException("Tâche introuvable");
            }

            _dbContext.Tasks?.Remove(task);
            _dbContext.SaveChanges();
        }

        public UserTask GetTaskById(long? taskId)
        {
            return _dbContext.Tasks?.FirstOrDefault(t => t.Id == taskId)!;
        }


        public BusinessResult<UserTask> UpdateTaskStatus(long taskId, bool isDone)
        {
            var task = _dbContext.Tasks?.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.Completed = isDone;
            }
            else
            {
                throw new InvalidOperationException("Tâche introuvable");
            }
            _dbContext.SaveChanges();
            return BusinessResult<UserTask>.FromSuccess(task);
        }

        public IEnumerable<UserTask>? GetTasksByCompleted(bool completed)
        {
            return _dbContext.Tasks?.Where(t => t.Completed == completed).ToList();
        }

        public IEnumerable<UserTask>? GetTasksByUserId(long userId)
        {
            return _dbContext.Tasks?.Where(t => t.UserId == userId).ToList();
        }

        public IEnumerable<UserTask>? SearchTasks(string keyword)
        {
            return _dbContext.Tasks?.AsEnumerable()
                .Where(task => task.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}