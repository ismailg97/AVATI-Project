using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using AVATI.Data.ValidationAttributes;

namespace AVATI.Data
{
    public class Hardskill
    {
        [Required]
        [NotNull]
        public string Description { get; set; }
        
        public List<string> Uppercat { get; set; }

        public List<string> Subcat { get; set; }

        public bool IsHardskill { get; set; } = true;

        public bool IsRoot()
        {
            return !IsHardskill && (Uppercat == null || !Uppercat.Any());
        }
    }
}