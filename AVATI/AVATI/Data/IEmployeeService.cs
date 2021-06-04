

namespace AVATI.Data
{
    public interface IEmployeeService
    {
        public bool CreateEmployeeProfile(Employee emp);
        public bool EditEmployeeProfile(Employee emp);
        public Employee GetEmployeeProfile(int employeeId);
       
        public bool EditStatus(int employeeId, bool status);
        public bool? GetSatus(int employeeId);
    }
}