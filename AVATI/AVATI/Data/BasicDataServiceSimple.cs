using System.Collections.Generic;

namespace AVATI.Data
{
    public class BasicDataServiceSimple: IBasicDataService
    {
        private readonly List<string> _allSoftSkills;
        private readonly List<string> _allRoles;
        private readonly List<string> _allFields;
        private readonly List<string> _allLanguages;

        public BasicDataServiceSimple()
        {
            _allSoftSkills = new List<string>()
            {
                "Interdisziplinärer Sachverstand",
                "Kommunikationsfähigkeit",
                "Problemlösungsfähigkeit",
                "Soziale Kompetenz",
                "Überzeugungskraft",
                "Methodische und strukturierte Vorgehensweise",
                "Interkulturelle Kompetenz",
                "Mitarbeiterförderung",
                "Ganzheitliches Denken"
            };

            _allRoles = new List<string>()
            {
                "Software Developer",
                "Agile Coach",
                "UI/UX-Designer",
                "Product Owner"
            };

            _allFields = new List<string>()
            {
                "Chemie",
                "Dienstleistung",
                "Druck/Verpackung",
                "IT",
                "Elektro/Elektronik",
                "Energie",
                "Forschung/Entwicklung",
                "Gesundheit/Soziales/Pflege",
                "Kunst/Kultur",
                "Landwirtschaft/Nahrungsmittel"
            };

            _allLanguages = new List<string>()
            {
                "Deutsch",
                "Englisch",
                "Türkisch",
                "Französisch",
                "Spanisch",
                "Italienisch",
                "Russisch",
                "Arabisch",
                "Mandarin",
                "Japanisch"
            };
        }


        public bool CreateSoftSkill(string description)
        {
            _allSoftSkills.Add(description);
            return true;
        }

        public bool UpdateSoftSkill(string newDescription, string oldDescription)
        {
            int index = _allSoftSkills.IndexOf(oldDescription);
            if (index == -1) return false;
            _allSoftSkills[index] = newDescription;
            return true;
        }

        public bool DeleteSoftSkill(string description)
        {
            return _allSoftSkills.Remove(description);
        }

        public List<string> GetAllSoftSkills()
        {
            return _allSoftSkills;
        }

        public bool CreateRole(string description)
        {
            _allRoles.Add(description);
            return true;
        }

        public bool UpdateRole(string newDescription, string oldDescription)
        {
            int index = _allRoles.IndexOf(oldDescription);
            if (index == -1) return false;
            _allRoles[index] = newDescription;
            return true;
        }

        public bool DeleteRole(string description)
        {
            return _allRoles.Remove(description);
        }

        public List<string> GetAllRoles()
        {
            return _allRoles;
        }

        public bool CreateField(string description)
        {
            _allFields.Add(description);
            return true;
        }

        public bool UpdateField(string newDescription, string oldDescription)
        {
            int index = _allFields.IndexOf(oldDescription);
            if (index == -1) return false;
            _allFields[index] = newDescription;
            return true;
        }

        public bool DeleteField(string description)
        {
            return _allFields.Remove(description);
        }

        public List<string> GetAllFields()
        {
            return _allFields;
        }

        public List<string> GetAllLanguages()
        {
            return _allLanguages;
        }
    }
}