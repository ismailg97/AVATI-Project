using System.Collections.Generic;
using System.Linq;

namespace AVATI.Data
{
    public class ProjectServiceSimple : IProjektService
    {
        private readonly List<Project> _projects = new List<Project>();
        
        public bool CreateProject(Project project)
        {
            _projects.Add(project);
            return true;
        }

        public bool UpdateProject(Project project)
        {
            if (!_projects.Any())
            {
                project.ProjectID = 1;
                _projects.Add(project);
            }else if (project.ProjectID != 0)
            {
                foreach (var pro in _projects)
                {
                    if (pro.ProjectID == project.ProjectID)
                    {
                        pro.Projecttitel = project.Projecttitel;
                        pro.Projectdescription = project.Projectdescription;
                        pro.Projectpurpose = project.Projectpurpose;
                    }
                }
            }

            return true;
        }

        public bool DeleteProject(int projectID)
        {
            Project temp = _projects.Find(x => x.ProjectID.Equals(projectID));
            return true;
        }

        public Project GetProject(int projectID)
        {
            return _projects.Find(x => x.ProjectID.Equals(projectID));
        }

        public List<Project> GetAllProjects()
        {
            return _projects;
        }

        public void AddFieldstoProject(int projectID, string field)
        {
            foreach (var project in _projects)
            {
                if (projectID == project.ProjectID)
                {
                    project.fields.Add(field);
                }
            }
        }
    }
}