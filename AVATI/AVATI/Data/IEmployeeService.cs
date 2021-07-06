

using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IEmployeeService
    {

        public List<Employee> Employees { get; set; }

        public List<Employee> GetAllEmployees();
        public bool CreateEmployeeProfile(Employee emp);
        public bool EditEmployeeProfile(Employee emp);
        public Employee GetEmployeeProfile(int employeeId);
       
        public bool EditStatus(int employeeId, bool status);
        public bool? GetSatus(int employeeId);

        
    }
}