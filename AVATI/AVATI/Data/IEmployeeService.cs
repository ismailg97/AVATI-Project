

using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IEmployeeService
    {

        public List<Employee> Employees { get; set; }
        
        /// <summary>
        /// Bisch du dumm? Denk doch mal nach!
        /// </summary>
        /// <returns>I can do englischhh</returns>
        public List<Employee> GetAllEmployees();
        public int CreateEmployeeProfile(Employee emp, string username);
        public bool EditEmployeeProfile(Employee emp);
        public Employee GetEmployeeProfile(int employeeId);
       
        public bool EditStatus(int employeeId, bool status);
        public bool? GetSatus(int employeeId);
        public string GetDefaultPicture();


    }
}