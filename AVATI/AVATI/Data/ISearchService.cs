using System;
using System.Collections.Generic;
using System.Data.Common;

namespace AVATI.Data
{
    public interface ISearchService
    {
       
        /// <summary>
        /// True if employee exists that has all attributes in query
        /// </summary>
        bool PerfectMatch { get; set; }
        List<string> SoftskillsToDisplay { get; set; }
        List<string> RolesToDisplay { get; set; }
        List<Hardskill> HardskillsToDisplay { get; set; }
        List<Hardskill> CategoriesToDisplay { get; set; }
        
        /// <summary>
        /// Used for searching specific employee, can be NULL
        /// </summary>
        string EmployeeName { get; set; }
        List<string> SoftskillsToSearch { get; set; }
        List<string> RolesToSearch { get; set; }
        List<Hardskill> HardskillsToSearch { get; set; }
        List<Hardskill> CategoriesToSearch { get; set; }
        List<string> SoftskillsToSearchTemp { get; set; }
        List<string> RolesToSearchTemp { get; set; }
        List<Hardskill> HardskillsToSearchTemp { get; set; }
        
        /// <summary>
        /// Swaps softskill from display-list to search-list (insertion at index = 0)
        /// </summary>
        /// <param name="softskill"></param>
        void AddSoftSearch(string softskill);
        /// <summary>
        /// Swaps hardskill from display-list to hardskill-list (insertion at index = 0)
        /// </summary>
        /// <param name="hardskill"></param>
        void AddHardSearch(Hardskill hardskill);
        
        /// <summary>
        /// Swaps role from display-list to role-list (insertion at index = 0)
        /// </summary>
        /// <param name="role"></param>
        void AddRoleSearch(string role);
        
        /// <summary>
        /// Swaps hardskillcat from display-list to role-list (insertion at index = 0)
        /// </summary>
        /// <param name="hardskill"></param>
        void AddCategorySearch(Hardskill hardskill);

        bool TableIsVisible { get; set; }
        
        /// <summary>
        /// After calling a search-query all of the search-values get emptied and the values to display get renewed
        /// </summary>
        void EmptyQuery();

        /// <summary>
        /// Is called upon opening employee search, passes all attributes (hard-/softskills, roles and hardskillcats) from .razor page to this class. All of the values get initialized
        /// </summary>
        /// <param name="softskills"></param>
        /// <param name="roles"></param>
        /// <param name="hardskills"></param>
        /// <param name="hardskillCat"></param>
        void InitAttributes(List<string> softskills, List<string> roles, List<Hardskill> hardskills,
            List<Hardskill> hardskillCat);

        /// <summary>
        /// Simple Search function that compares search-attributes with attributes of employee.
        /// Employees get sorted according to number of matching attributes
        /// </summary>
        /// <param name="name">Name of Employee (can be NULL)</param>
        /// <param name="Softskill">List of required Softskills</param>
        /// <param name="Hardskill">List of required Hardskills</param>
        /// <param name="Rolle">List of required Roles</param>
        /// <param name="Categories">List of required Categories (Won't be visible in Proposal!)</param>
        /// <param name="foundCats">List of found Categories in Employee List (Result of</param>
        /// <param name="forProposal"></param>
        /// <returns>List of all Employees that fulfill at least one search criteria</returns>
        List<Employee> SearchEmployee(string name, List<string> Softskill, List<Hardskill> Hardskill,
            List<string> Rolle, List<Hardskill> Categories, List<Tuple<string, int>> foundCats, bool forProposal
        );
    }
}