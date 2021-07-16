using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVATI.Data.EmployeeDetailFiles
{
    public interface IEmployeeDetailService
    {

        public Task<EmployeeDetail> GetEmployeeDetail(int employeeId, int proposalId);

        
        /// <summary>
        /// Copies all selected data from Employee in specific Proposal
        /// </summary>
        /// <param name="proposalId">Identification Proposal</param>
        /// <param name="newId">Id of the Proposal that is beeing copied</param>
        /// <param name="emp">Id of employee</param>
        /// <returns>True of proposal exists and copy was successful</returns>
        public bool CopyDetail(int proposalId, int newId, int emp);
        
        /// <summary>
        /// Deletes all Related data from EmployeeDetail for specific employee & proposal and deletes table afterwards
        /// </summary>
        /// <param name="employeeId">Identification Employee</param>
        /// <param name="proposalId">Identification Proposal</param>
        /// <returns></returns>
        public bool DeleteEmployeeDetail(int employeeId, int proposalId);
        
        /// <summary>
        /// Returns a list of all EmployeeDetail-Objects of specific Proposal
        /// </summary>
        /// <param name="proposalId">Identification Proposal</param>
        /// <returns></returns>

        public List<EmployeeDetail> GetAllEmployeeDetail(int proposalId);

        /// <summary>
        /// Compares value of EmpDetail found in DB with Parameter EmpDetail and deletes/adds missing attributes
        /// </summary>
        /// <param name="propId">Identification Proposal</param>
        /// <param name="empId">Identification Employee</param>
        /// <param name="employeeDetail">EmployeeDetail-Object with all related data</param>
        /// <returns>true if Update was successful</returns>
        public Task<bool> UpdateEmployeeDetail(int propId, int empId, EmployeeDetail employeeDetail);
        
        
        
    }
}