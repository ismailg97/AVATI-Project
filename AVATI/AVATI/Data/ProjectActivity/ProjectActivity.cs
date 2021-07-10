using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AVATI.Data
{
    public class ProjectActivity
    {
        [Required][NotNull]
        public string Description { get; set; }
        public int ProjectID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectActivityID { get; set; }
        public List<string> SoftSkills { get; set; } = new List<string>();
        public List<string> HardSkills { get; set; } = new List<string>();

        private sealed class DescriptionEqualityComparer : IEqualityComparer<ProjectActivity>
        {
            public bool Equals(ProjectActivity x, ProjectActivity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Description == y.Description;
            }

            public int GetHashCode(ProjectActivity obj)
            {
                return (obj.Description != null ? obj.Description.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<ProjectActivity> DescriptionComparer { get; } = new DescriptionEqualityComparer();
    }
}