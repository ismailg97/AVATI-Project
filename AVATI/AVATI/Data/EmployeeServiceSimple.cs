using System.Collections.Generic;




namespace AVATI.Data
{
    public class EmployeeServiceSimple : IEmployeeService
    {
        
        
         public List<Employee> EmpList = new List<Employee>() {
        
                        new Employee()
                            {FirstName = "Ismail", LastName = "Gürsöz", Role = new List<string>(){"Programming"} , HardSkills = new List<Hardskill>()
                            {
                                new Hardskill {Description = "JavaScript", Subcat = null, Uppercat = null, Height = 0},
                                new Hardskill {Description = "CSS", Subcat = null, Uppercat = null, Height = 0}
                            },
                                SoftSkills = new List<string>(){"Teamleading"}
                            },
                        
                        new Employee()
                        {FirstName = "Anton", LastName = "Huber", Role = new List<string>(){"Programming"} , HardSkills = new List<Hardskill>()
                        {
                            new Hardskill {Description = "Python", Subcat = null, Uppercat = null, Height = 0},
                        },
                        SoftSkills = new List<string>(){"Teamleading"}
                        },
                        
                        new Employee()
                        {FirstName = "Victoria", LastName = "Kuch", Role = new List<string>(){"Programming"} , HardSkills = new List<Hardskill>()
                        {
                            new Hardskill {Description = "Java", Subcat = null, Uppercat = null, Height = 0},
                        },
                        SoftSkills = new List<string>(){"none"}
                        }
        
                };
        
        
        

       
        
        
        
        
        public bool CreateEmployeeProfile(Employee emp)
        {
            throw new System.NotImplementedException();
        }

        public bool EditEmployeeProfile(Employee emp)
        {
            throw new System.NotImplementedException();
        }

        public Employee GetEmployeeProfile(int employeeId)
        {
            throw new System.NotImplementedException();
        }

        public bool EditStatus(int employeeId,int status)
        {
            throw new System.NotImplementedException();

        }

        public int GetSatus(int employeeId)
        {
            throw new System.NotImplementedException();
        }
    }
}