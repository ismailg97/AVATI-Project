using System;
using System.Collections.Generic;

namespace AVATI.Data.EmployeeDetailFiles
{
    public interface IEmployeeDetailService
    {
        public bool UpdateEmployeeDetail(int employeeId, int proposalId, EmployeeDetail employeeDetail);

        public EmployeeDetail GetEmployeeDetail(int employeeId, int proposalId);

        public bool DeleteEmployeeDetail(int employeeId, int proposalId);

        public List<EmployeeDetail> GetAllEmployeeDetail();

        public void DeleteRc(EmployeeDetail input);

        public void DeleteField(EmployeeDetail input, string field);

        public void DeleteHard(EmployeeDetail input, Hardskill hardskill);

        public void DeleteSoft(EmployeeDetail input, string softskill);

        public void DeleteLang(EmployeeDetail input, Tuple<string, LanguageLevel> lang);

        public void DeleteRole(EmployeeDetail input, string role);

        public void AddRc(EmployeeDetail input, int rc);

        public void AddField(EmployeeDetail input, string field);

        public void AddHard(EmployeeDetail input, Hardskill hardskill);

        public void AddSoft(EmployeeDetail input, string softskill);

        public void AddLang(EmployeeDetail input, Tuple<string, LanguageLevel> lang);

        public void AddRole(EmployeeDetail input, string role);
    }
}