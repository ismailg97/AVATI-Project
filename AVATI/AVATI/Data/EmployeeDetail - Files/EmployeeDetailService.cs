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
            EmployeeDetail temp;
            if ((temp = EmployeeDetails.Find(e => e.ProposalId == proposalId && e.EmployeeId == employeeId)) == null)
            {
                return false;
            }
            else
            {
                temp.Fields = employeeDetail.Fields;
                temp.Roles = employeeDetail.Roles;
                temp.Hardskills = employeeDetail.Hardskills;
                temp.Languages = employeeDetail.Languages;
                temp.Rc = employeeDetail.Rc;
                temp.Softskills = employeeDetail.Softskills;
                return true;
            }
        }

        public EmployeeDetail GetEmployeeDetail(int employeeId, int proposalId)
        {
            
            if (EmployeeDetails?.Find(e => e.EmployeeId == employeeId) != null && EmployeeDetails.Find(e => e.ProposalId == proposalId) != null)
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