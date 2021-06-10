using System.Collections.Generic;

namespace AVATI.Data
{
    public class EmployeeDetailService : IEmployeeDetailService
    {
        public List<EmployeeDetail> EmployeeDetails { get; set; } = new List<EmployeeDetail>();
        public bool UpdateEmployeeDetail(EmployeeDetail employeeDetail)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateEmployeeDetail(int employeeId, int proposalId, EmployeeDetail employeeDetail)
        {
            throw new System.NotImplementedException();
        }

        public EmployeeDetail GetEmployeeDetail(int employeeId, int proposalId)
        {
            
            if (EmployeeDetails != null && EmployeeDetails.Find(e => e.EmployeeId == employeeId) != null &&
                EmployeeDetails.Find(e => e.ProposalId == proposalId) != null)
            {
                return EmployeeDetails.Find(e => e.EmployeeId == employeeId && e.ProposalId == proposalId);
            }
            else
            {
                return null;
            }
        }

        public bool DeleteEmployeeDetail(int employeeId, int proposalId)
        {
            throw new System.NotImplementedException();
        }

        public List<EmployeeDetail> GetAllEmployeeDetail()
        {
            return EmployeeDetails;
        }
    }
}