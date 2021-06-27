using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVATI.Data.EmployeeDetailFiles
{
    public interface IEmployeeDetailService
    {

        public Task<EmployeeDetail> GetEmployeeDetail(int employeeId, int proposalId);

        public bool CopyDetail(int proposalId, int newId, int emp);
        public bool DeleteEmployeeDetail(int employeeId, int proposalId);

        public List<EmployeeDetail> GetAllEmployeeDetail();

        public Task<bool> UpdateEmployeeDetail(int propId, int empId, EmployeeDetail employeeDetail);
    }
}