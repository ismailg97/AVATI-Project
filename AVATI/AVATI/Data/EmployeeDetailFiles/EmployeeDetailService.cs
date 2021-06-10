using System.Collections.Generic;

namespace AVATI.Data.EmployeeDetailFiles
{
    public class EmployeeDetailService : IEmployeeDetailService
    {
        public EmployeeDetailService()
        {
            EmployeeDetails = new List<EmployeeDetail>();
        }
        public List<EmployeeDetail> EmployeeDetails;
        public bool UpdateEmployeeDetail(EmployeeDetail employeeDetail)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateEmployeeDetail(int employeeId, int proposalId, EmployeeDetail employeeDetail)
        {
            EmployeeDetail temp;
            if ((temp = EmployeeDetails.Find(e => e.ProposalId == proposalId && e.EmployeeId == employeeId)) == null)
            {
                EmployeeDetails.Add(new EmployeeDetail()
                {
                    EmployeeId = employeeId, ProposalId = proposalId, Rc = employeeDetail.Rc, Softskills = employeeDetail.Softskills,
                    Hardskills = employeeDetail.Hardskills, Fields = employeeDetail.Fields, Languages = employeeDetail.Languages,
                    Roles = employeeDetail.Roles
                });
                return true;
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