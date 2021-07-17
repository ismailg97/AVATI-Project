using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IProjectService
    {
        /// <summary>
        /// Takes all the information from passed Project and creates a new Entry in DB (Including Employees and Fields)
        /// </summary>
        /// <param name="project">Information to be loaded onto database</param>
        /// <returns>true if creation was successful</returns>
        public bool CreateProject(Project project);
        
        /// <summary>
        /// Compares Data from Project in DB with Data from Project that was passed and updates DB accordingly
        /// </summary>
        /// <param name="project">The updated version of the Project to be loaded into DB</param>
        /// <returns>true if Update was successful</returns>
        public bool UpdateProject(Project project);
        
        /// <summary>
        /// Deletes Project with specified projectID from DB including all ProjectActivities and Employees associated with Project 
        /// </summary>
        /// <param name="projectId">Identifier Project</param>
        /// <returns>true if deletion was successful</returns>
        public bool DeleteProject(int projectId);
        
        /// <summary>
        /// Returns Project with specified projectID
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Project GetProject(int projectId);
        
        /// <summary>
        /// Returns all available Projects from Database including all Attributes 
        /// </summary>
        /// <returns>List of Projects</returns>
        public List<Project> GetAllProjects();

    }
}