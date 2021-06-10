using System.Collections.Generic;

namespace AVATI.Data.EmployeeDetailFiles
{
    public interface IEmployeeDetailService
    {
        public bool UpdateEmployeeDetail(int employeeId, int proposalId, EmployeeDetail employeeDetail);

        public EmployeeDetail GetEmployeeDetail(int employeeId, int proposalId);

        public bool DeleteEmployeeDetail(int employeeId, int proposalId);

        public List<EmployeeDetail> GetAllEmployeeDetail();

        public void DeleteRc(int employeeId, int proposalId);

        public void DeleteField(int employeeId, int proposalId, string field);

        public void DeleteHard(int employeeId, int proposalId, Hardskill hardskill);

        public void DeleteSoft(int employeeId, int proposalId, string softskill);

        public void DeleteLang(int employeeId, int proposalId, string lang);

        public void DeleteRole(int employeeId, int proposalId, string role);
    }
}