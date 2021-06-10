using System.Collections.Generic;



namespace AVATI.Data
{
    public class ProjectActivityServiceSimple : IProjctActivityService
    {
        private IEmployeeService _employeeService;
        private IProjektService _projektService;
        private IHardskillService _hardskillService;
        
        public Employee Emp;
        public Project Proj;
        
        

        public List<ProjectActivity> pALIst = new List<ProjectActivity>()
        {
            new ProjectActivity()
            {
                Description = "UI-Tester", EmployeeID = 3, ProjectID = 0,
            }
            
        };
        

        public bool SetProjectActivity(int EmployeeId, int ProjectId, ProjectActivity pA)
        {
            Emp = _employeeService.GetEmployeeProfile(EmployeeId);
            Proj = _projektService.GetProject(ProjectId);
            foreach (var pActivityEmp in pALIst)
            {
                if (pActivityEmp.EmployeeID == Emp.EmployeeId)
                {
                    foreach (var pActivityProj in pALIst)
                    {
                        if (pActivityProj.ProjectID == Proj.ProjectID)
                        {
                            pActivityProj.Description = pA.Description;
                        }
                    }  
                }
            }
            


            throw new System.NotImplementedException();
        }

        

        public bool DeleteProjectActivity(int EmployeeId, int ProjectId, string Description)
        {
            throw new System.NotImplementedException();
        }

        public List<ProjectActivity> GetProjectActivities(int EmployeeId, int ProjectId)
        {
            throw new System.NotImplementedException();
        }
    }
}