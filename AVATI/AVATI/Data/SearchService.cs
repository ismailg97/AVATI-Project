using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    //This class serves as a temporary storage of all Search-Related attributes
    public class SearchService
    {
        private readonly IConfiguration _configuration;
        public DbConnection GetConnection()
        {
            return new SqlConnection
                (_configuration.GetConnectionString("AVATI-Database"));
        }

        public SearchService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Employee> EmployeeList { get; set; }
        public IEmployeeService EmployeeService { get; set; }

        public Employee Employee { get; set; }
        public int Priority { get; set; }

        private int EmployeeId;
        public bool PerfectMatch { get; set; }

        public bool TableIsVisible = false;

        //These are the attributes the user can choose from in the Search-Mask
        public List<string> SoftskillsToDisplay { get; set; } = new List<string>();
        public List<string> RolesToDisplay { get; set; } = new List<string>();
        public List<Hardskill> HardskillsToDisplay { get; set; } = new List<Hardskill>();

        public List<Hardskill> CategoriesToDisplay { get; set; } = new List<Hardskill>();

        //These are the for the search selected attributes
        public string EmployeeName { get; set; }
        public List<string> SoftskillsToSearch { get; set; } = new List<string>();
        public List<string> RolesToSearch { get; set; } = new List<string>();
        public List<Hardskill> HardskillsToSearch { get; set; } = new List<Hardskill>();

        public List<Hardskill> CategoriesToSearch { get; set; } = new List<Hardskill>();
        //These get initialized in the EmployeeSearch Page and wont be changed

        public List<string> Softskills { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();

        public List<Hardskill> Categories { get; set; } = new List<Hardskill>();

        //In order to only display the relevant Attributes

        public string EmployeeNameTemp { get; set; }
        public List<string> SoftskillsToSearchTemp { get; set; } = new List<string>();
        public List<string> RolesToSearchTemp { get; set; } = new List<string>();
        public List<Hardskill> HardskillsToSearchTemp { get; set; } = new List<Hardskill>();

        public List<Hardskill> CategoriesToSearchTemp { get; set; } = new List<Hardskill>();

        // List to Compare with current Proposal

        public List<Employee> AlreadyAddedEmployees { get; set; } = new List<Employee>();

        //All Functions


        public void AddSoftSearch(string softskill)
        {
            if (!SoftskillsToSearch.Contains(softskill))
            {
                SoftskillsToSearch.Insert(0, softskill);
                SoftskillsToDisplay.Remove(softskill);
            }
        }

        public void AddHardSearch(Hardskill hardskill)
        {
            if (!HardskillsToSearch.Contains(hardskill))
            {
                HardskillsToSearch.Insert(0, hardskill);
                HardskillsToDisplay.Remove(hardskill);
            }
        }

        public void AddRoleSearch(string role)
        {
            if (!RolesToSearch.Contains(role))
            {
                RolesToSearch.Insert(0, role);
                RolesToDisplay.Remove(role);
            }
        }

        public void AddCategorySearch(Hardskill hardskill)
        {
            if (!CategoriesToSearch.Contains(hardskill))
            {
                CategoriesToSearch.Insert(0, hardskill);
                CategoriesToDisplay.Remove(hardskill);
            }
        }

        public void EmptyQuery()
        {
            SoftskillsToSearch?.Clear();
            HardskillsToSearch?.Clear();
            RolesToSearch?.Clear();
            CategoriesToSearch?.Clear();
            EmployeeName = null;
            if (SoftskillsToDisplay != null)
            {
                SoftskillsToDisplay = new List<string>(Softskills);
            }

            if (HardskillsToDisplay != null)
            {
                HardskillsToDisplay = new List<Hardskill>(Hardskills);
                HardskillsToDisplay = HardskillsToDisplay.OrderBy(e => e.Description).ToList();
            }

            if (RolesToDisplay != null)
            {
                RolesToDisplay = new List<string>(Roles);
            }

            if (CategoriesToDisplay != null)
            {
                CategoriesToDisplay = new List<Hardskill>(Categories);
            }
        }

        public void EmptySoftSearch()
        {
            SoftskillsToSearch.Clear();
            SoftskillsToDisplay = new List<string>(Softskills);
        }

        public void EmptyHardSearch()
        {
            HardskillsToSearch.Clear();
            HardskillsToDisplay = new List<Hardskill>(Hardskills);
        }

        public void EmptyRoleSearch()
        {
            RolesToSearch.Clear();
            RolesToDisplay = new List<string>(Roles);
        }

        public void InitAttributes(List<string> softskills, List<string> roles, List<Hardskill> hardskills,
            List<Hardskill> hardskillCat)
        {
            EmployeeService = new EmployeeService(_configuration);
            EmployeeList = EmployeeService.GetAllEmployees();
            Hardskills = new List<Hardskill>(hardskills);
            Softskills = new List<string>(softskills);
            Categories = new List<Hardskill>(hardskillCat);
            Roles = new List<string>(roles);
            SoftskillsToDisplay = new List<string>(Softskills);
            HardskillsToDisplay = new List<Hardskill>(Hardskills);
            CategoriesToDisplay = new List<Hardskill>(Categories);
            RolesToDisplay = new List<string>(Roles);
            TableIsVisible = false;
        }


        public List<Employee> SearchEmployee(string name, List<string> Softskill, List<Hardskill> Hardskill,
            List<string> Rolle, List<Hardskill> Categories, List<Tuple<string, int>> foundCats, bool forProposal
        )
        {
            PerfectMatch = false;
            HardskillsToSearchTemp = new List<Hardskill>(Hardskill);
            CategoriesToSearchTemp = new List<Hardskill>(Categories);
            RolesToSearchTemp = new List<string>(Rolle);
            SoftskillsToSearchTemp = new List<string>(Softskill);
            EmployeeNameTemp = name;
            List<SearchService> TempEmployee = new List<SearchService>();
            List<Employee> EmployeeListToReturn = new List<Employee>();
            foreach (var employee in EmployeeList)
            {
                if (employee.IsActive || !forProposal)
                {
                    int numberOfMatches = 0;
                    if (name != null && String.Concat(employee.FirstName, " " + employee.LastName)
                        .Contains(name, StringComparison.OrdinalIgnoreCase))
                    {
                        ++numberOfMatches;
                    }

                    foreach (var soft in Softskill)
                    {
                        if (employee.Softskills.Contains(soft))
                        {
                            ++numberOfMatches;
                        }
                    }

                    foreach (var hard in Hardskill)
                    {
                        if (employee.Hardskills.Exists(e => e.Description.Equals(hard.Description)))
                        {
                            ++numberOfMatches;
                        }
                    }

                    foreach (var category in foundCats.FindAll(e => e.Item2 == employee.EmployeeID))
                    {
                        ++numberOfMatches;
                    }

                    foreach (var role in Rolle)
                    {
                        if (employee.Roles.Contains(role))
                        {
                            ++numberOfMatches;
                        }
                    }

                    if (numberOfMatches != 0)
                    {
                        TempEmployee.Add(new SearchService(_configuration)
                            {Employee = employee, EmployeeId = employee.EmployeeID, Priority = numberOfMatches});
                    }

                    if (numberOfMatches == Hardskill.Count + Softskill.Count + Rolle.Count + Categories.Count +
                        ((name == null) ? 0 : 1))
                    {
                        PerfectMatch = true;
                    }
                }
            }

            TempEmployee = TempEmployee.OrderBy(e => e.Priority).ToList();
            TempEmployee.Reverse();

            foreach (var service in TempEmployee)
            {
                EmployeeListToReturn.Add(EmployeeList.Find(e => e.EmployeeID == service.EmployeeId));
            }

            foreach (var emp in EmployeeListToReturn)
            {
                Console.WriteLine(emp.Roles.First());
            }

            return EmployeeListToReturn;
        }
    }
}