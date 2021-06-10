using System;
using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IEmployeeDetailService
    {
        public bool UpdateEmployeeDetail(EmployeeDetail employeeDetail);

        public EmployeeDetail GetEmployeeDetail(int employeeId, int proposalId);

        public bool DeleteEmployeeDetail(int employeeId, int proposalId);

        public List<EmployeeDetail> GetAllEmployeeDetail();
        
        

    }
}