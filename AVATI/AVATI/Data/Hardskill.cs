using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace AVATI.Data
{
    public class Hardskill
    {
        [Required]
        [NotNull]
        public string Description { get; set; }
        
        public Hardskill Uppercat { get; set; }

        public List<Hardskill> Subcat { get; set; }
        
        //if height != 0, the so called "hardskill" is a hardskill category
        [Required]
        [NotNull]
        public int Height { get; set; }

        public bool IsHardskill { get; set; } = true;

        //returns the root of the tree in which the object is contained
        public Hardskill GetRoot()
        {
            return Uppercat == null ? this : Uppercat.GetRoot();
        }

        public bool ContainsHardskills()
        {
            return Subcat != null && Subcat.Any() && Subcat[0].IsHardskill;
        }

        public bool IsRoot()
        {
            return (!IsHardskill && Uppercat == null);
        }
    }
}