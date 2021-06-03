using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IHardskillService
    {
        public bool CreateHardskill( Hardskill hardskill );
        public bool UpdateHardskill( Hardskill hardskill );
        public bool DeleteHardskill( string description );

        public Hardskill GetHardskill( string description );
        public List<Hardskill> GetAllHardskills();
        
        public bool CreateHardskillCategory( Hardskill hardskillcat );
        public bool UpdateHardskillCategory( Hardskill hardskillcat );
        public bool DeleteHardskillCategory( string description );
        
        public Hardskill GetHardskillCategory( string description );
        public List<Hardskill> GetAllHardskillCategorys();
    }
}