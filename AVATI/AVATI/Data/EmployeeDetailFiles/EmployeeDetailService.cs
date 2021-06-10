using System;
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

        public void DeleteRc(EmployeeDetail input)
        {
            input.Rc = 0;
        }

        public void DeleteField(EmployeeDetail input, string field)
        {
            input.Fields.Remove(field);
        }

        public void DeleteHard(EmployeeDetail input, Hardskill hardskill)
        {
            input.Hardskills.Remove(hardskill);
        }

        public void DeleteSoft(EmployeeDetail input, string softskill)
        {
            input.Softskills.Remove(softskill);
        }

        public void DeleteLang(EmployeeDetail input, Tuple<string, LanguageLevel> lang)
        {
            input.Languages.Remove(lang);
        }

        public void DeleteRole(EmployeeDetail input, string role)
        {
            input.Roles.Remove(role);
        }

        public void AddRc(EmployeeDetail input, int rc)
        {
            input.Rc = rc;
        }

        public void AddField(EmployeeDetail input, string field)
        {
            input.Fields.Add(field);
        }

        public void AddHard(EmployeeDetail input, Hardskill hardskill)
        {
            input.Hardskills.Add(hardskill);
        }

        public void AddSoft(EmployeeDetail input, string softskill)
        {
            input.Softskills.Add(softskill);
        }

        public void AddLang(EmployeeDetail input, Tuple<string, LanguageLevel> lang)
        {
            input.Languages.Add(lang);
        }

        public void AddRole(EmployeeDetail input, string role)
        {
            input.Roles.Add(role);
        }
    }
}