﻿using TP_CS.Business.IRepositories;
using TP_CS.Business.IServices;
using TP_CS.Business.Models;

namespace TP_CS.Business.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        
        public ProjectService(IProjectRepository taskRepository)
        {
            _projectRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public BusinessResult<Project> GetProjectById(long id)
        {
            var project = _projectRepository.GetProjectById(id);
            return BusinessResult<Project>.FromSuccess(project);
        }

        public BusinessResult<IEnumerable<Project>> GetProjects()
        {
            var proj = _projectRepository.GetProjects();
            return BusinessResult<IEnumerable<Project>>.FromSuccess(proj);
        }
        public BusinessResult<Project> CreateProject(Project project)
        {
            _projectRepository.CreateProject(project);
            return BusinessResult<Project>.FromSuccess(project);
        }

        public BusinessResult Update(Project project, long id)
        {
            var existingProject = _projectRepository.UpdateProject(project, id);
            return BusinessResult.FromSuccess(existingProject);
        }

        public BusinessResult DeleteProject(long id)
        {
            _projectRepository.DeleteProject(id);
            return BusinessResult.FromSuccess();
        }
        
        public BusinessResult<List<Project>> SearchProject(string keyword)
        {
            var matchingTasks = _projectRepository.SearchProjects(keyword);
            return BusinessResult<List<Project>>.FromSuccess(matchingTasks?.ToList());
        }
    }
}