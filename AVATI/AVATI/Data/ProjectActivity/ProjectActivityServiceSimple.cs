using System.Collections.Generic;



namespace AVATI.Data
{
    public class ProjectActivityServiceSimple : IProjectActivityService
    {

       
        
        

        public List<ProjectActivity> pALIst = new List<ProjectActivity>()
        {
        };
        

        public bool SetProjectActivity(int EmployeeId, int ProjectId, string Description)
        {
            ProjectActivity pA = new ProjectActivity();
            pA.Description = Description;
            pA.EmployeeID = EmployeeId;
            pA.ProjectID = ProjectId;
            foreach (var activity in pALIst)
            {
                if (activity.EmployeeID == EmployeeId)
                {
                    if (activity.ProjectID == ProjectId)
                    {
                        pALIst.Remove(activity);
                        
                    }
                }
            }
            pALIst.Add(pA);
            return true;

        }

       


        public bool DeleteProjectActivity(int EmployeeId, int ProjectId)
        {
            foreach (var activity in pALIst)
            {
                if (activity.EmployeeID == EmployeeId)
                {
                    if (activity.ProjectID == ProjectId)
                    {
                        pALIst.Remove(activity);
                        return true;

                    }
                }
            }

            return false;
        }

        public ProjectActivity GetProjectActivities(int EmployeeId, int ProjectId)
        {
            foreach (var activity in pALIst)
            {
                if (activity.EmployeeID == EmployeeId)
                {
                    if (activity.ProjectID == ProjectId)
                    {
                        return activity;


                    }
                }
            }

            return null;
        }
    }
}