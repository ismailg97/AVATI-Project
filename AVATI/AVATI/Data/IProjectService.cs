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
        public List<Project> SearchProject(List<Project> projects, string input);
        public List<string> GetAllFieldsFromOneProject(int ProjectID);
        
    }
}