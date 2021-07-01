using System.Collections.Generic;

namespace AVATI.Data
{
    public interface IBasicDataService
    {
        public bool CreateSoftSkill(string description);

        public bool UpdateSoftSkill(string newDescription, string oldDescription);

        public bool DeleteSoftSkill(string description);

        public List<string> GetAllSoftSkills();

        public bool CreateRole(string description);

        public bool UpdateRole(string newDescription, string oldDescription);

        public bool DeleteRole(string description);
        
        public List<string> GetAllRoles();
        
        public bool CreateField(string description);

        public bool UpdateField(string newDescription, string oldDescription);

        public bool DeleteField(string description);
        
        public List<string> GetAllFields();

        public List<string> GetAllLanguages();

        public bool CheckDescriptionSoftskill(string description);

        public bool CheckDescriptionField(string description);

        public bool CheckDescriptionRole(string description);
    }
}