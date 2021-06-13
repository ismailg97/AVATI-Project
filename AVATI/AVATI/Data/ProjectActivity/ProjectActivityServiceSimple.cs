using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;


namespace AVATI.Data
{
    public class ProjectActivityServiceSimple : IProjectActivityService
    {
        private List<ProjectActivity> EmpList;
        private List<ProjectActivity> ProjList;
        
        public List<ProjectActivity> pALIst;

        public ProjectActivityServiceSimple()
        {
            pALIst = new List<ProjectActivity>()
            {
                new ProjectActivity() {Description = "test2", EmployeeID = 1, ProjectID = 1},
                new ProjectActivity() {Description = "test1", EmployeeID = 1, ProjectID = 2}
            };
        }


        public bool SetProjectActivity(int EmployeeId, int ProjectId, string Description)
        {
            ProjectActivity temp;
            ProjectActivity pA = new ProjectActivity()
            {
                Description = Description,
                EmployeeID = EmployeeId,
                ProjectID = ProjectId
            };
            if ((temp = pALIst.Find(x => x.ProjectID == ProjectId && EmployeeId == x.EmployeeID)) == null)
            {
                pALIst.Add(pA);
            }
            else
            {
                temp = pA;
            }
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

        List<ProjectActivity> IProjectActivityService.GetProjectActivities(int EmployeeId, int ProjectId)
        {
            return pALIst.FindAll(x => x.ProjectID == ProjectId && x.EmployeeID == EmployeeId);
        }

        public List<ProjectActivity> ReturnListProjectActivities(int ProjectID)
        {
            return pALIst.FindAll(x => x.ProjectID == ProjectID);
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

        public List<ProjectActivity> GetProjectActivitiesList()
        {
            return pALIst;
        }

        public List<ProjectActivity> GetProjectActivityListEmployee(int EmpId)
        {
            EmpList = new List<ProjectActivity>();

            foreach (var activity in pALIst)
            {
                if (activity.EmployeeID == EmpId)
                {
                    EmpList.Add(activity);
                }
            }

            return EmpList;
        }

        public List<ProjectActivity> GetProjectActivityListProject(int ProjectId)
        {
            ProjList = new List<ProjectActivity>();
            foreach (var activity in pALIst)
            {
                if (activity.ProjectID == ProjectId)
                {
                    ProjList.Add(activity);
                }
            }

            return ProjList;
        }
    }
}