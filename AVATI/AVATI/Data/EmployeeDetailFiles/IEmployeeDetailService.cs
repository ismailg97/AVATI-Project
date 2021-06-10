using System.Collections.Generic;

namespace AVATI.Data.EmployeeDetailFiles
{
    public interface IEmployeeDetailService
    {
        public bool UpdateEmployeeDetail(int employeeId, int proposalId, EmployeeDetail employeeDetail);

        public EmployeeDetail GetEmployeeDetail(int employeeId, int proposalId);

        public bool DeleteEmployeeDetail(int employeeId, int proposalId);

        public List<EmployeeDetail> GetAllEmployeeDetail();

        

    }
}