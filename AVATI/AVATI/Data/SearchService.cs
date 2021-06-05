using System.Collections.Generic;

namespace AVATI.Data
{
    //This class serves as a temporary storage of all Search-Related attributes
    public class SearchService
    {
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
        
        public List<string> Softskills { get; set; }
        public List<string> Roles { get; set; }
        public List<Hardskill> Hardskills { get; set; } 

        
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
            SoftskillsToSearch.Clear();
            HardskillsToSearch.Clear();
            RolesToSearch.Clear();
            EmployeeName = null;
            SoftskillsToDisplay = new List<string>(Softskills);
            HardskillsToDisplay = new List<Hardskill>(Hardskills);
            RolesToDisplay = new List<string>(Roles);

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
            Hardskills = new List<Hardskill>(hardskills);
            Softskills = new List<string>(softskills);
            Roles = new List<string>(roles);
            SoftskillsToDisplay = new List<string>(Softskills);
            HardskillsToDisplay = new List<Hardskill>(Hardskills);
            RolesToDisplay = new List<string>(Roles);
        }
        

        public void SearchEmployee()
        {
            //TODO
        }
    }
}