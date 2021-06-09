using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AVATI.Data
{
    public class ProjectServiceSimple : IProjektService
    {
        public List<Project> _projects;
        public List<Project> Searching;
        private static int identification = 1;
        
        public bool CreateProject(Project project)
        {
            _projects.Add(project);
            return true;
        }

        public void AddProject(Project project)
        {
            project.ProjectID = identification++;
            _projects.Add(project);
        }

        public bool UpdateProject(Project project)
        {
           if (_projects.Any() == false)
            {
                project.ProjectID = 0;
                AddProject(project);
            }
            else if (project.ProjectID != 0)
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
        }                           //probleme mit updating 
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
                fields = new List<string>(), Projectdescription = "iwasyallah", Employees = new List<Employee>(), Projectpurpose = new List<string>(), Projecttitel = "goodbye",
                Runtime = DateTime.Today
            };
            Project zwei = new Project
            {
                fields = new List<string>(), Projectdescription = "zelda", Employees = new List<Employee>(), Projectpurpose = new List<string>(), Projecttitel = "link",
                Runtime = DateTime.Today
            };
            Project drei = new Project
            {
                fields = new List<string>(), Projectdescription = "bladerunner", Employees = new List<Employee>(), Projectpurpose = new List<string>(), Projecttitel = "better than star wars",
                Runtime = DateTime.Today
            };
            _projects = new List<Project>();
            AddProject(eins);
            AddProject(zwei);
            AddProject(drei);
        }
        
        public List<Project> SearchProject(List<Project> projects, string input)
        {
            Searching = new List<Project>();
            if (input == null)
            {
                return null;
            }
            foreach (var project in projects)
            {
                
                if (project.Projecttitel.Contains(input))
                {
                    Searching.Add(project);
                }

                Searching = Searching.OrderBy(x => x.Projecttitel).ToList();
            }
            Console.WriteLine("did it");
            return Searching;
        }
    }
}