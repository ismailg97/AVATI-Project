using System.Collections.Generic;
using System.Linq;

namespace AVATI.Data
{
    public class ProjectPurposeService
    {
        public List<ProjectPurpose> _Purposes;
        public static int Identification = 0;

        public ProjectPurpose CreatePurpose(string description, int ProjectID)
        {
            ProjectPurpose temp = new ProjectPurpose();
            temp.Purpose = description;
            temp.ProjectID = ProjectID;
            return temp;
        }

        public bool UpdatePurpose(ProjectPurpose purpose)
        {
            if (_Purposes.Any() == false)
            {
                return false;
            }
            else if(purpose.ProjectID != 0)
            {
                foreach (var purp in _Purposes)
                {
                    if (purpose.ProjectID.Equals(purp.ProjectID))
                    {
                        purp.Purpose = purpose.Purpose;
                        
                    }
                }
            }

            return true;
        }
    }
}