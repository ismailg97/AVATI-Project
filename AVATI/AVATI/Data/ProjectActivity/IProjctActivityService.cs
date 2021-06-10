using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IProjctActivityService
    {
        public bool SetProjectActivity(int EmployeeId, int ProjectId, ProjectActivity pA);
        public bool DeleteProjectActivity(int EmployeeId, int ProjectId, string Description);
        public List<ProjectActivity> GetProjectActivities(int EmployeeId, int ProjectId);
    }
}