using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IProjctActivityService
    {
        public bool SetProjectActivity(int EmployeeId, int ProjectId, string Description);
        public bool DeleteProjectActivity(int EmployeeId, int ProjectId, string Description);
        public ProjectActivity GetProjectActivities(int EmployeeId, int ProjectId);
        //Fehler im Designklassendiagramm
    }
}