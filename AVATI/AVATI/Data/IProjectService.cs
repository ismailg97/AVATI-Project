using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IProjektService
    {
        public bool CreateProject(Project project);
        public bool UpdateProject(Project project);
        public bool DeleteProject(int projectID);
        public Project GetProject(int projectID);
        public List<Project> GetAllProjects();
        public List<string> GetAllFieldsFromOneProject(int ProjectID);

        public bool DeleteEmployeeFromProject(int ProjectID, int EmployeeID);

        public bool UpdateFieldsFromProject(int ProjectID, List<string> fields);
    }
}