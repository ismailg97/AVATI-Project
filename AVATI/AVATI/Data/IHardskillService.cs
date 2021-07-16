using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVATI.Data
{
    public interface IHardskillService
    {
        /// <summary>
        /// Returns Task of Hardskill-Object which is either Category or Hardskill based on given Description
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Task<Hardskill> GetHardskillOrCategory(string description);
        
        /// <summary>
        /// Creates new Hardskill including Description and if given also adds Relation to Parent-Category
        /// </summary>
        /// <param name="hardskill">Object containing all Information about Hardskill to be added</param>
        /// <returns>true if Hardskill was successfully created</returns>
        public Task<bool> CreateHardskill( Hardskill hardskill );
        
        /// <summary>
        /// Replaces information about oldHardskill with newHardskill (Including Upper Categories) 
        /// </summary>
        /// <param name="newHardskill">Hardskill to replace old Hardskill</param>
        /// <param name="oldHardskill">Hardskill to be replaced</param>
        /// <returns>true if changes were applied successfully</returns>
        public Task<bool> UpdateHardskill( Hardskill newHardskill, Hardskill oldHardskill );
        
        /// <summary>
        /// Removes Hardskill with given Description from DB and removes Reference from Upper-Categories
        /// </summary>
        /// <param name="description">Hardskill to be deleted</param>
        /// <returns>true if deletion successful</returns>
        public Task<bool> DeleteHardskill( string description );

        /// <summary>
        /// Returns Hardskill based of description
        /// </summary>
        /// <param name="description">String to identify Hardskill</param>
        /// <returns>Hardskill if description is in DB and value in DB is not a Category, null else</returns>
        public Task<Hardskill> GetHardskill( string description );
        
        /// <summary>
        /// Returns List of all Hardskills currently in DB
        /// </summary>
        /// <returns>List of Hardskills</returns>
        public Task<List<Hardskill>> GetAllHardskills();
        
        /// <summary>
        /// Inserts new Hardskill-Category into DB, appending to existing Uppercat if specified in Object.
        /// If Hardskill-Object contains Subcategories, said Subcategories will be appended to new Category
        /// and if needed removed from current Uppercategory. 
        /// </summary>
        /// <param name="hardskillcat"></param>
        /// <returns>true if Category was successfully inserted into hierarchy</returns>
        public Task<bool> CreateHardskillCategory( Hardskill hardskillcat );
        
        /// <summary>
        /// Changes name of Category with oldDescription to new Description
        /// </summary>
        /// <param name="oldDescription"></param>
        /// <param name="newDescription"></param>
        /// <returns>true if update successful</returns>
        public Task<bool> RenameHardskillCategory(string oldDescription, string newDescription);
        /// <summary>
        /// Removes all Subcategories/Hardskills of specified Category that are not found in List of specified Hardskills
        /// </summary>
        /// <param name="hardskillcat">Specific Category</param>
        /// <param name="hardskills">List of hardskills/Subcats to keep in db</param>
        /// <returns>true if edit was successful</returns>
        public Task<bool> EditHardskillsCategory(string hardskillcat, List<string> hardskills);
        
        /// <summary>
        /// Deletes Category from DB and if existing appends all subcategories to all parents of Category
        /// </summary>
        /// <param name="description"></param>
        /// <returns>true if deletion successfull</returns>
        public Task<bool> DeleteHardskillCategory( string description );

        /// <summary>
        /// Returns wether a given Hardskill-Category has any children that are Hardskills
        /// </summary>
        /// <param name="description"></param>
        /// <returns>true if Category has children that are hardskills</returns>
        public Task<bool> ContainsAnyHardskills( string description );

        /// <summary>
        /// Returns wether a given Category only contains Hardskills as Children
        /// </summary>
        /// <param name="description"></param>
        /// <returns>true if Hardskill only has Hardskills as children</returns>
        public Task<bool> ContainsJustHardskills( string description);
        
        /// <summary>
        /// Returns direct Parent-Category of given hardskill
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Category if exists, null else</returns>
        public Task<Hardskill> GetHardskillCategory( string description );
        
        /// <summary>
        /// Returns all Categories including their relations
        /// </summary>
        /// <returns></returns>
        public Task<List<Hardskill>> GetAllHardskillCategorys();
        
        /// <summary>
        /// Returns only the description of all Hardskills specified as Category in DB
        /// </summary>
        /// <returns>List of strings</returns>
        public Task<List<string>> GetAllDesCategorys();
        /// <summary>
        /// Returns only the description of all Hardskills specified as Hardskill in DB
        /// </summary>
        /// <returns>List of strings</returns>
        
        public Task<List<string>> GetAllDesHardskills();

        /// <summary>
        /// Returns all Categories that have no parents 
        /// </summary>
        /// <returns>Description of all Categories with no parents</returns>
        public Task<List<string>> GetAllRoots();

        /// <summary>
        /// Returns all Hardskills that are related to specific Category
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Task<List<string>> GetHardskillsOfCategory(string description);
     
        public Task<bool> CheckExistHardskill(string description);
        
        /// <summary>
        /// Checks if Hardskill has a Description
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool CheckEmptyHardskill(string description); 

        /// <summary>
        /// Checks whether specific hardskill has valid Description-Length
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool CheckLengthHardskill(string description);

        /// <summary>
        /// Removes relation between Hardskill and a specified Category
        /// </summary>
        /// <param name="category">Identifier Category</param>
        /// <param name="hardskill">Identifier Hardskill</param>
        /// <returns></returns>
        public Task<bool> DeleteHardskillOutOfCategory(string category, string hardskill);

    }
}