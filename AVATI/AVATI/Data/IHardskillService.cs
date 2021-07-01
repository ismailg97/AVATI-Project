using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVATI.Data
{
    public interface IHardskillService
    {
        public Task<Hardskill> GetHardskillOrCategory(string description);
        public Task<bool> CreateHardskill( Hardskill hardskill );
        public Task<bool> UpdateHardskill( Hardskill newHardskill, Hardskill oldHardskill );
        public Task<bool> DeleteHardskill( string description );

        public Task<Hardskill> GetHardskill( string description );
        public Task<List<Hardskill>> GetAllHardskills();
        
        public Task<bool> CreateHardskillCategory( Hardskill hardskillcat );
        public Task<bool> UpdateHardskillCategory(string oldDescription, string newDescription);

        public Task<bool> EditHardskillsCategory(string hardskillcat, List<string> hardskills);
        public Task<bool> DeleteHardskillCategory( string description );

        public Task<bool> ContainsAnyHardskills( string description );
        
        public Task<Hardskill> GetHardskillCategory( string description );
        public Task<List<Hardskill>> GetAllHardskillCategorys();
        
        public Task<List<string>> GetAllDesCategorys();
        
        public Task<List<string>> GetAllDesHardskills();

        public Task<List<string>> GetAllRoots();

        public Task<List<string>> GetHardskillsOfCategory(string description);
        public Task<bool> CheckDescriptionHardskill(string description);

    }
}