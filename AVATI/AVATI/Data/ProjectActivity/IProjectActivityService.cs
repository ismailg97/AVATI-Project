﻿using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IProjectActivityService
    {
        public bool SetProjectActivity(int EmpId, int ProjId, ProjectActivity activity);
        public bool DeleteProjectActivityEmployee(int ProjectActivityID);

        public bool DeleteProjectActivity(string Description);
        public List<ProjectActivity> GetEmployeeProjectActivities(int EmployeeId, int ProjectId);
        public List<ProjectActivity> GetProjectActivitiesProject(int ProjectID);

        public List<ProjectActivity> GetAllProjectActivities();
        public List<ProjectActivity> GetProjectActivitiesEmployee(int EmployeeId);
        public bool UpdateActivity(string oldDescription, string newDescription);
        public bool AddActivity(string description);
        public bool UpdateProjectActivity(int proposalId, List<ProjectActivity> activities);
        public bool UpdateProjectActivityEmployee(int EmpId, int ProjId,ProjectActivity activity);
        public string GetProjectActivity(string description);
    }
}