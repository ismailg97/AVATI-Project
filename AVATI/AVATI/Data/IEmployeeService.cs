

namespace AVATI.Data
{
    public interface IEmployeeService
    {
        public bool CreateEmployeeProfile(Employee emp);
        public bool EditEmployeeProfile(Employee emp);
        public Employee GetEmployeeProfile(int employeeId);
       
        public bool EditStatus(int employeeId, int status);
        public int GetSatus(int employeeId);
    }
}