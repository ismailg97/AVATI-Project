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
            Employee employee = new Employee();
            employee.Field = emp.Field;
            employee.FirstName = employee.LastName;
            employee.LastName = emp.LastName;
            employee.Language = emp.Language;
            employee.Role = emp.Role;
            employee.EmploymentTime = emp.EmploymentTime;
            employee.EmpType = emp.EmpType;
            employee.HardSkills = emp.HardSkills;
            employee.SoftSkills = emp.SoftSkills;
            employee.RcLevel = emp.RcLevel;
            employee.RelevantWorkExperience = emp.RelevantWorkExperience;
            EmpList.Add(employee);
            return true;
        }

        public bool EditEmployeeProfile(Employee emp)
        {
            foreach (Employee employee in EmpList)
            {
                if (emp.EmployeeId == employee.EmployeeId)
                {
                    employee.Field = emp.Field;
                    employee.FirstName = employee.LastName;
                    employee.LastName = emp.LastName;
                    employee.Language = emp.Language;
                    employee.Role = emp.Role;
                    employee.EmploymentTime = emp.EmploymentTime;
                    employee.EmpType = emp.EmpType;
                    employee.HardSkills = emp.HardSkills;
                    employee.SoftSkills = emp.SoftSkills;
                    employee.RcLevel = emp.RcLevel;
                    employee.RelevantWorkExperience = emp.RelevantWorkExperience;
                    return true;
                }
            }

            return false;
        }

        public Employee GetEmployeeProfile(int employeeId)
        {
            foreach (Employee emp in EmpList)
            {
                if (emp.EmployeeId == employeeId)
                {
                    return emp;
                }
            }

            return null;
        }

        public bool EditStatus(int employeeId,bool status)
        {
            foreach (Employee emp in EmpList)
            {
                if (emp.EmployeeId == employeeId)
                {
                    emp.IsActive = status;
                    return true;
                }
            }
            return false;
        }

        public bool? GetSatus(int employeeId)
        {
            foreach (Employee emp in EmpList)
            {
                if (emp.EmployeeId == employeeId)
                {
                    return emp.IsActive;
                }
            }
            return null;
        }
    }
}