using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IProjektService
    {
        public bool CreateProject(Project project);
        public bool UpdateProject(Project project);
        public bool DeleteProject(int projectId);
        public Project GetProject(int projectId);
        public List<Project> GetAllProjects();
        
        public List<string> GetAllFieldsFromOneProject(int projectId);

        public bool DeleteEmployeeFromProject(int projectId, int employeeId);

        public bool UpdateFieldsFromProject(int projectId, List<string> fields);
    }
}