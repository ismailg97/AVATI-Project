using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

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

        //returns the root of the tree in which the object is contained
        public Hardskill GetRoot()
        {
            return Uppercat == null ? this : Uppercat.GetRoot();
        }

        //returns false if the so called "hardskill" is a hardskill category
        public bool IsHardskill()
        {
            return Height == 0;
        }

        public bool IsRoot()
        {
            return (Height != 0 && Uppercat == null);
        }
    }
}