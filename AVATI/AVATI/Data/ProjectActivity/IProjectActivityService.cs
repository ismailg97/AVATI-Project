using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace AVATI.Data
{
    public interface IProjectActivityService
    {
        public bool SetProjectActivityToEmployee(ProjectActivity activity);
        public bool UpdateProjectActivityToEmployee(ProjectActivity activity);
        public bool UpdateSkillsToActivity(int projectActivityId, List<string> hardSkills, List<string> softSkills);
        public bool DeleteProjectActivityToEmployee(int projectActivityId);
        public Dictionary<int, List<string>> GetActivitiesWithProjectsGrouped(int employeeId);
        public List<ProjectActivity> GetProjectActivitiesOfEmployee(int employeeId);
        public List<string> GetActivitiesDesOfProject(int projectId);
        public Dictionary<string, List<int>> GetActivitiesWithEmployeesGrouped(int projectId);
        public List<ProjectActivity> GetEmployeeProjectActivities(int employeeId, int projectId);
        public string GetProjectActivityForPurpose(string purpose);
        public bool SetProjectActivitiesToProject(int projectId, List<string> activities);
        public bool AddGlobalProjectActivity(string description);
        public bool UpdateGlobalProjectActivity(string oldDescription, string newDescription);
        public bool DeleteGlobalProjectActivity(string description);
        public List<string> GetAllGlobalProjectActivities();
        public bool IsGlobal(string description);
    }
}