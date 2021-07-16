using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IBasicDataService
    {
        /// <summary>
        /// Creates new Softskill with given Description
        /// </summary>
        /// <param name="description"></param>
        /// <returns>true if Softskill with Description doesnt already exist</returns>
        public bool CreateSoftSkill(string description);

        public bool UpdateSoftSkill(string newDescription, string oldDescription);

        public bool DeleteSoftSkill(string description);

        public List<string> GetAllSoftSkills();
        
        /// <summary>
        /// Creates new Role with given Description
        /// </summary>
        /// <param name="description"></param>
        /// <returns>true if Role with Description doesnt already exist</returns>

        public bool CreateRole(string description);

        public bool UpdateRole(string newDescription, string oldDescription);

        public bool DeleteRole(string description);
        
        public List<string> GetAllRoles();
        
        /// <summary>
        /// Creates new Field with given Description
        /// </summary>
        /// <param name="description"></param>
        /// <returns>true if Field with Description doesnt already exist</returns>
        
        public bool CreateField(string description);

        public bool UpdateField(string newDescription, string oldDescription);

        public bool DeleteField(string description);
        
        public List<string> GetAllFields();

        public List<string> GetAllLanguages();

        public bool CheckDescriptionSoftskill(string description);

        public bool CheckDescriptionField(string description);

        public bool CheckDescriptionRole(string description);
        
        public bool CheckEmptyBasicData(string description);

        public bool CheckLengthBasicData(string description);
    }
}