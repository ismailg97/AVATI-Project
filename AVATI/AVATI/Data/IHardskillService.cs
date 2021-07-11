using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVATI.Data
{
    public interface IHardskillService
    {
        public Task<Hardskill> GetHardskillOrCategory(string description); //(implicit) tested
        public Task<bool> CreateHardskill( Hardskill hardskill ); //tested
        public Task<bool> UpdateHardskill( Hardskill newHardskill, Hardskill oldHardskill ); //tested
        public Task<bool> DeleteHardskill( string description ); //tested

        public Task<Hardskill> GetHardskill( string description ); //tested
        public Task<List<Hardskill>> GetAllHardskills(); //not to be tested
        
        public Task<bool> CreateHardskillCategory( Hardskill hardskillcat );
        public Task<bool> RenameHardskillCategory(string oldDescription, string newDescription);

        public Task<bool> EditHardskillsCategory(string hardskillcat, List<string> hardskills);
        public Task<bool> DeleteHardskillCategory( string description );

        public Task<bool> ContainsAnyHardskills( string description ); //tested

        public Task<bool> ContainsJustHardskills( string description); //tested
        
        public Task<Hardskill> GetHardskillCategory( string description ); //tested
        public Task<List<Hardskill>> GetAllHardskillCategorys(); //not to be tested
        
        public Task<List<string>> GetAllDesCategorys(); //not to be tested
        
        public Task<List<string>> GetAllDesHardskills(); //not to be tested

        public Task<List<string>> GetAllRoots(); //not to be tested

        public Task<List<string>> GetHardskillsOfCategory(string description);
        public Task<bool> CheckExistHardskill(string description); //tested

        public bool CheckEmptyHardskill(string description); //tested

        public bool CheckLengthHardskill(string description); //tested

    }
}