using System.Collections.Generic;

namespace AVATI.Data
{
    public class HardskillServiceSimple: IHardskillService
    {
        private readonly List<Hardskill> _allHardskills;
        
        private readonly List<Hardskill> _allHardskillCat;

        public HardskillServiceSimple()
        {
            Hardskill javascript = 
                new Hardskill {Description = "JavaScript", Subcat = null, Uppercat = null, Height = 0};
            Hardskill css = 
                new Hardskill {Description = "CSS", Subcat = null, Uppercat = null, Height = 0};
            Hardskill html = 
                new Hardskill {Description = "HTML", Subcat = null, Uppercat = null, Height = 0};

            Hardskill aussp = 
                new Hardskill {Description = "Auszeichnungssprachen", Subcat = new List<Hardskill>(){ html, css },
                    Uppercat = null, Height = 1, IsHardskill = false};
            Hardskill objWeb = 
                new Hardskill {Description = "objektorientierte Web-Programmierung", Subcat = new List<Hardskill>(){ javascript }, 
                    Uppercat = null, Height = 1, IsHardskill = false};
            Hardskill fronEnd = 
                new Hardskill {Description = "Front-End", Subcat = new List<Hardskill>(){ aussp, objWeb }, 
                    Uppercat = null, Height = 2, IsHardskill = false};
            Hardskill webent = 
                new Hardskill {Description = "Webentwicklung", Subcat = new List<Hardskill>(){ fronEnd },
                    Uppercat = null, Height = 3, IsHardskill = false};
            javascript.Uppercat = objWeb;
            css.Uppercat = aussp;
            html.Uppercat = aussp;
            aussp.Uppercat = fronEnd;
            objWeb.Uppercat = fronEnd;
            fronEnd.Uppercat = webent;

            _allHardskills = new List<Hardskill>() { javascript, css, html };
            _allHardskillCat = new List<Hardskill>() { aussp, objWeb, fronEnd, webent};
        }

        public bool CreateHardskill(Hardskill hardskill)
        {
            _allHardskills.Add(hardskill);
            hardskill.Uppercat?.Subcat.Add(hardskill);
            return true;
        }

        public bool UpdateHardskill(Hardskill newHardskill, Hardskill oldHardskill)
        {
            var index = _allHardskills.IndexOf(oldHardskill);
            if (index == -1) return false;
            oldHardskill.Uppercat?.Subcat.Remove(oldHardskill);
            newHardskill.Uppercat?.Subcat.Add(newHardskill);

            _allHardskills[index] = newHardskill;
            return true;
        }

        public bool DeleteHardskill(string description)
        {
            var hardskill = _allHardskills.Find(x => x.Description == description);

            if (hardskill == null) return false;
            hardskill.Uppercat?.Subcat.Remove(hardskill);
            
            return _allHardskills.Remove(hardskill);
        }

        public Hardskill GetHardskill(string description)
        {
            return _allHardskills.Find(x => x.Description == description);
        }

        public List<Hardskill> GetAllHardskills()
        {
            return _allHardskills;
        }

        public bool CreateHardskillCategory(Hardskill hardskillcat)
        {
            hardskillcat.Uppercat?.Subcat.Add(hardskillcat);
            
            foreach (var skill in hardskillcat.Subcat)
            {
                skill.Uppercat = hardskillcat;
                hardskillcat.Uppercat?.Subcat.Remove(skill);
            }

            _allHardskillCat.Add(hardskillcat);
            return true;
        }

        public bool UpdateHardskillCategory(string oldDescription, string newDescription)
        {
            var hardskillcat = _allHardskillCat.Find(x => x.Description == oldDescription);
            if (hardskillcat == null) return false;
            hardskillcat.Description = newDescription;
            return true;
        }

        public bool DeleteHardskillCategory(string description)
        {
            var hardskillcat = _allHardskillCat.Find(x => x.Description == description);

            if (hardskillcat == null) return false;
            
            foreach (var cat in hardskillcat.Subcat)
            {
                hardskillcat.Uppercat?.Subcat.Add(cat);
                cat.Uppercat = hardskillcat.Uppercat;
            }
                
            hardskillcat.Uppercat?.Subcat.Remove(hardskillcat);

            return _allHardskillCat.Remove(hardskillcat);
        }

        public Hardskill GetHardskillCategory(string description)
        {
            return _allHardskillCat.Find(x => x.Description == description);
        }

        public List<Hardskill> GetAllHardskillCategorys()
        {
            return _allHardskillCat;
        }
    }
}