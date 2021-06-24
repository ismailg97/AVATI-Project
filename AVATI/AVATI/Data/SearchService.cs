using System;
using System.Collections.Generic;
using System.Linq;

namespace AVATI.Data
{
    //This class serves as a temporary storage of all Search-Related attributes
    public class SearchService
    {
        //This list will recieve all Employes from the Database
        public List<Employee> EmployeeList { get; set; }
        public IEmployeeService EmployeeService { get; set; }

        public Employee Employee { get; set; }
        public int Priority { get; set; }

        public bool PerfectMatch { get; set; }

        public bool TableIsVisible = false;

        //These are the attributes the user can choose from in the Search-Mask
        public List<string> SoftskillsToDisplay { get; set; } = new List<string>();
        public List<string> RolesToDisplay { get; set; } = new List<string>();
        public List<Hardskill> HardskillsToDisplay { get; set; } = new List<Hardskill>();

        //These are the for the search selected attributes
        public string EmployeeName { get; set; }
        public List<string> SoftskillsToSearch { get; set; } = new List<string>();
        public List<string> RolesToSearch { get; set; } = new List<string>();
        public List<Hardskill> HardskillsToSearch { get; set; } = new List<Hardskill>();

        //These get initialized in the EmployeeSearch Page and wont be changed

        public List<string> Softskills { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<Hardskill> Hardskills { get; set; } = new List<Hardskill>();

        //In order to only display the relevant Attributes

        public string EmployeeNameTemp { get; set; }
        public List<string> SoftskillsToSearchTemp { get; set; } = new List<string>();
        public List<string> RolesToSearchTemp { get; set; } = new List<string>();
        public List<Hardskill> HardskillsToSearchTemp { get; set; } = new List<Hardskill>();

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

        public void EmptyQuery()
        {
            SoftskillsToSearch?.Clear();
            HardskillsToSearch?.Clear();
            RolesToSearch?.Clear();
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

        public void InitAttributes(List<string> softskills, List<string> roles, List<Hardskill> hardskills)
        {
            //EmployeeService = new EmployeeService();
            EmployeeList = EmployeeService.GetAllEmployees();
            Hardskills = new List<Hardskill>(hardskills);
            Softskills = new List<string>(softskills);
            Roles = new List<string>(roles);
            SoftskillsToDisplay = new List<string>(Softskills);
            HardskillsToDisplay = new List<Hardskill>(Hardskills);
            RolesToDisplay = new List<string>(Roles);
            TableIsVisible = false;
        }


        public List<Employee> SearchEmployee(string name, List<string> Softskill, List<Hardskill> Hardskill,
            List<string> Rolle
        )
        {
            PerfectMatch = false;
            SoftskillsToSearchTemp = new List<string>(Softskill);
            HardskillsToSearchTemp = new List<Hardskill>(Hardskill);
            RolesToSearchTemp = new List<string>(Rolle);
            EmployeeNameTemp = name;
            List<SearchService> TempEmployee = new List<SearchService>();
            List<Employee> EmployeeListToReturn = new List<Employee>();
            foreach (var employee in EmployeeList)
            {
                int numberOfMatches = 0;
                if (name != null && name == String.Concat(employee.FirstName, " " + employee.LastName))
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

                foreach (var role in Rolle)
                {
                    if (employee.Roles.Contains(role))
                    {
                        ++numberOfMatches;
                    }
                }

                if (numberOfMatches != 0)
                {
                    TempEmployee.Add(new SearchService() {Employee = employee, Priority = numberOfMatches});
                }

                if (numberOfMatches == Hardskill.Count + Softskill.Count + Rolle.Count + ((name == null) ? 0 : 1))
                {
                    PerfectMatch = true;
                }
            }

            TempEmployee = TempEmployee.OrderBy(e => e.Priority).ToList();
            TempEmployee.Reverse();

            foreach (var service in TempEmployee)
            {
                EmployeeListToReturn.Add(service.Employee);
            }


            return EmployeeListToReturn;
        }
    }
}