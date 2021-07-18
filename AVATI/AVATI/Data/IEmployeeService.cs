

using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IEmployeeService
    {

        public List<Employee> Employees { get; set; }
        
        /// <summary>
        /// Returns a List of all Employees including all related attributes
        /// </summary>
        /// <returns>List of Employee-Objects</returns>
        public List<Employee> GetAllEmployees();
        
        /// <summary>
        /// Creates a new Profile with the information in the passed Employee-Object and Loads it into DB
        /// </summary>
        /// <param name="emp">Object containing all relevant information</param>
        /// <returns>Id of employee</returns>
        public int CreateEmployeeProfile(Employee emp, string username);
        
        /// <summary>
        /// Compares current EmployeeProfile in DB with passed Employee-Object and Saves all changes
        /// </summary>
        /// <param name="emp">Object containing Updated Employee-Profile</param>
        /// <returns>true if update was successful</returns>
        public bool EditEmployeeProfile(Employee emp);
        
        /// <summary>
        /// Returns Employee with specific EmployeeId (Including all Attributes associated with Employee)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public Employee GetEmployeeProfile(int employeeId);
       
        /// <summary>
        /// Changes Employees current Employee Status depending on passed boolean - Value
        /// </summary>
        /// <param name="employeeId">Identifier Employee</param>
        /// <param name="status">boolean value</param>
        /// <returns></returns>
        public bool EditStatus(int employeeId, bool status);

        
        /// <summary>
        /// Gets Employees current Employee Status
        /// </summary>
        /// <param name="employeeId">Identifier Employee</param>
        /// <returns>status</returns>
        public bool GetStatus(int employeeId);



    }
}