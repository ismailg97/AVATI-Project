using System.Collections.Generic;
using System.Linq;


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