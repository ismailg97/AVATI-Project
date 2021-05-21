namespace Team12.Data {
    public class Skill {
        public enum Category {
            Hardskill,
            Softskill
        }
        public int Id {get; set;}
        public string Name {get; set;}
        public Category SkillCategory { get; set;}
        public Skill(int id, Category cat, string name) {
            Id = id;
            SkillCategory = cat;
            Name = name;
        }
        public Skill()
        {
            Id = -1;
            Name = "name here";
            SkillCategory = Category.Hardskill;
        }
    }
}