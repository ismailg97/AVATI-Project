using System;
using System.Collections.Generic;

namespace AVATI.Data.EmployeeDetailFiles
{
    public interface IEmployeeDetailService
    {

        public EmployeeDetail GetEmployeeDetail(int employeeId, int proposalId);

        public bool CopyDetail(int proposalId, int newId, int emp);
        public bool DeleteEmployeeDetail(int employeeId, int proposalId);

        public List<EmployeeDetail> GetAllEmployeeDetail();

        public bool UpdateEmployeeDetail(int propId, int empId, EmployeeDetail employeeDetail);
    }
}