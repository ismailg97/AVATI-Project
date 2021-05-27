namespace Team12.Data {
    public class Skill {
        public enum Category {
            Hardskill,
            Softskill
        }
        public int Id {get; set;}
        public string Name {get; set;}
        public Category SkillCategory { get; set;}
    }
}
