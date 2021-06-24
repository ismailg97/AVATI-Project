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

        private static int identification = 0;
        public bool CreateProject(Project project)
        {
            project.ProjectID = identification++;
            project.Projectbeginning = DateTime.Today;
            _projects.Add(project);
            return true;
        }

        public bool UpdateProject(Project project)
        {
            if (_projects.Any() == false)
            {
                return false;
            }
            else if (project.ProjectID != 0)
            {
                foreach (var proj in _projects)
                {
                    if (project.ProjectID.Equals(proj.ProjectID))
                    {
                        proj.Projecttitel = project.Projecttitel;
                        proj.Projectdescription = project.Projectdescription;
                        proj.Fields = project.Fields;
                        proj.Projectpurpose = project.Projectpurpose;
                        proj.Projectbeginning = project.Projectbeginning;
                        proj.Employees = project.Employees;
                        proj.Projectend = project.Projectend;
                        proj.ProjectActivities = project.ProjectActivities;
                     
                    }
                }
            }

            return true;
        } //probleme mit updating 

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
                    temp = field.Fields;
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
                    project.Fields.Add(field);
                }
            }
        }

        public ProjectServiceSimple()
        {
            _projects = new List<Project>();
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
            return Searching;
        }
    }
}