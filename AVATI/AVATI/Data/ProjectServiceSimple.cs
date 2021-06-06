using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AVATI.Data
{
    public class ProjectServiceSimple : IProjektService
    {
        public readonly List<Project> _projects;
        private static int identification = 0;
        
        public bool CreateProject(Project project)
        {
            _projects.Add(project);
            return true;
        }
        

        public bool UpdateProject(Project project)
        {
            if (_projects.Any() == false)
            {
                project.ProjectID = 0;
                _projects.Add(project);
            }
            
            else if (project.ProjectID == 0)
            {
                foreach (var proj in _projects)
                {
                    if (project.ProjectID.Equals(proj.ProjectID))
                    {
                        proj.Projecttitel = project.Projecttitel;
                        proj.Projectdescription = project.Projectdescription;
                        proj.fields = project.fields;
                        proj.Projectpurpose = project.Projectpurpose;
                        proj.Runtime = project.Runtime;
                    }
                }
            }else if (project.ProjectID != 0)
            {
                foreach (var proj in _projects)
                {
                    if (project.ProjectID.Equals(proj.ProjectID))
                    {
                        proj.Projecttitel = project.Projecttitel;
                        proj.Projectdescription = project.Projectdescription;
                        proj.fields = project.fields;
                        proj.Projectpurpose = project.Projectpurpose;
                        proj.Runtime = project.Runtime;
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

        public List<string> GetAllFieldsFromOneProject(int ProjectID)
        {
            List<string> temp = new List<string>();
            foreach (var field in _projects)
            {
                if (field.ProjectID == ProjectID)
                {
                    temp = field.fields;
                }
            }

            return temp;
        }

        public void AddFieldstoProject(int projectid, string field)
        {
            foreach (var project in _projects)
            {
                if (projectid == project.ProjectID)
                {
                    project.fields.Add(field);
                }
            }
        }

        public ProjectServiceSimple()
        {
            Project eins = new Project
            {
                fields = new List<string>(), Projectdescription = "iwasyallah", Projectpurpose = new List<string>(), Projecttitel = "goodbye",
                Runtime = DateTime.Today, ProjectID = identification++
            };
            Project zwei = new Project
            {
                fields = new List<string>(), Projectdescription = "zelda", Projectpurpose = new List<string>(), Projecttitel = "link",
                Runtime = DateTime.Today, ProjectID = identification++
            };
            Project drei = new Project
            {
                fields = new List<string>(), Projectdescription = "bladerunner", Projectpurpose = new List<string>(), Projecttitel = "better than star wars",
                Runtime = DateTime.Today, ProjectID = identification++
            };
            _projects = new List<Project>() {eins, zwei, drei};
        }
    }
}