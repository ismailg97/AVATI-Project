using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace AVATI.Data
{
    public class EmployeeServiceSimple : IEmployeeService
    {
        public List<Employee> Employees { get; set; }

        public EmployeeServiceSimple()
        {
            Employees = new List<Employee>()
            {
                new Employee()
                {
                    
                    EmployeeId = 1,FirstName = "Ismail", LastName = "Gürsöz", Roles = new List<string>()
                    {
                        "Software Developer",
                        "Agile Coach",
                        "UI/UX-Designer",
                        "Product Owner"
                    },
                    Hardskills = new List<Hardskill>()
                    {
                        new Hardskill {Description = "C++"}, new Hardskill() {Description = "C"},
                        new Hardskill() {Description = "JavaScript"}, new Hardskill() {Description = "C#"}
                    },
                    Softskills = new List<string>()
                    {
                        "Konzeptionsstärke",
                        "Organisationsfähigkeit",
                        "Lehrfähigkeit",
                    },
                    Field = new List<string>()
                    {
                        "Automobil",
                        "Bildung"
                    },
                    RelevantWorkExperience = new List<string>()
                    {
                        "Design",
                        "Test"
                    },
                    Projects = new List<Project>()
                    {
                        new Project() {Projecttitel = "Auto 3.0",Projectdescription = "Irgendwas mit Autos", ProjectID = 3, fields = new List<string>() {"Automobil","Smtn"}},
                        new Project() {Projecttitel = "BuildBsp",Projectdescription = "Was weiss ich", ProjectID = 4, fields = new List<string>(){"Bildung"}}
                    },
                    Language = new List<Tuple<string, LanguageLevel>>()
                    {
                       
                    }
                },

                new Employee()
                {
                    
                    EmployeeId = 0,FirstName = "Anton", LastName = "Huber", Roles = new List<string>()
                    {
                        "Agile Coach",
                        "UI/UX-Designer",
                        "Product Owner"
                    },
                    Hardskills =
                        new List<Hardskill>()
                        {
                            new Hardskill {Description = "Java"}, new Hardskill() {Description = "JavaScript"},
                            new Hardskill() {Description = "Python"}, new Hardskill() {Description = "C#"}
                        },
                    Softskills = new List<string>()
                    {
                        "Beratungsfähigkeit",
                        "Rhetorik",
                        "Analytische Fähigkeiten",
                    }
                },

                new Employee()
                {
                    
                    EmployeeId = 2,FirstName = "Victoria", LastName = "Kuch", Roles = new List<string>()
                    {
                        "Software Developer",
                        "Agile Coach",
                    },
                    Hardskills =
                        new List<Hardskill>()
                        {
                            new Hardskill {Description = "Java"}, new Hardskill() {Description = "JavaScript"},
                            new Hardskill() {Description = "Python"}, new Hardskill() {Description = "C#"}
                        },
                    Softskills = new List<string>()
                    {
                        "Interdisziplinärer Sachverstand",
                        "Kommunikationsfähigkeit",
                        "Problemlösungsfähigkeit"
                    }
                },
                new Employee()
                {
                    
                    EmployeeId = 3,FirstName = "Alex", LastName = "Xela", Roles = new List<string>() {"Product Owner"}, Hardskills =
                        new List<Hardskill>()
                        {
                            new Hardskill {Description = "Java"}, new Hardskill() {Description = "JavaScript"},
                            new Hardskill() {Description = "Python"}, new Hardskill() {Description = "C#"}
                        },
                    Softskills = new List<string>() {"none"}
                },
                new Employee()
                {
                    
                    EmployeeId = 4,FirstName = "Victoria", LastName = "Airotciv", Roles = new List<string>() {"Product Owner"},
                    Hardskills = new List<Hardskill>()
                    {
                        new Hardskill {Description = "Java"}, new Hardskill() {Description = "JavaScript"},
                        new Hardskill() {Description = "Python"}, new Hardskill() {Description = "C#"}
                    },
                    Softskills = new List<string>()
                    {
                        "Innovationsfreudigkeit",
                        "Soziales Engagement",
                        "Impulsgeben"
                    },
                    Field = new List<string>()
                    {
                        "Automobil",
                        "Bildung"
                    },
                    RelevantWorkExperience = new List<string>()
                    {
                        "Design",
                        "Test"
                    },
                    Projects = new List<Project>()
                    {
                        new Project() {Projecttitel = "Auto 3.0",Projectdescription = "Irgendwas mit Autos", ProjectID = 3, fields = new List<string>() {"Automobil","Smtn"}},
                        new Project() {Projecttitel = "BuildBsp",Projectdescription = "Was weiss ich", ProjectID = 4, fields = new List<string>(){"Bildung"}}
                    },
                    Language = new List<Tuple<string, LanguageLevel>>()
                    {
                        Tuple.Create<string,LanguageLevel>("Englisch", LanguageLevel.B2),
                        Tuple.Create<string,LanguageLevel>("Deutsch",LanguageLevel.A1)
                    }
                },
                new Employee()
                {
                    
                    EmployeeId = 5,FirstName = "Tobi", LastName = "Ibot", Roles = new List<string>() {"UI/UX-Designer"}, Hardskills =
                        new List<Hardskill>()
                        {
                            new Hardskill {Description = "Java"}, new Hardskill() {Description = "JavaScript"},
                            new Hardskill() {Description = "Python"}, new Hardskill() {Description = "C#"}
                        },
                    Softskills = new List<string>()
                    {
                        "Akquisitionsstärke",
                        "Beratungsfähigkeit",
                        "Rhetorik"
                    }
                }
            };
        }


        public List<Employee> GetAllEmployees()
        {
            return Employees;
        }

        public bool CreateEmployeeProfile(Employee emp)
        {
            Employee employee = new Employee();
            employee.Field = emp.Field;
            employee.FirstName = employee.LastName;
            employee.LastName = emp.LastName;
            employee.Language = emp.Language;
            employee.Roles = emp.Roles;
            employee.EmploymentTime = emp.EmploymentTime;
            employee.EmpType = emp.EmpType;
            employee.Hardskills = emp.Hardskills;
            employee.Softskills = emp.Softskills;
            employee.Rc = emp.Rc;
            employee.RelevantWorkExperience = emp.RelevantWorkExperience;
            Employees.Add(employee);
            return true;
        }

        public bool EditEmployeeProfile(Employee emp)
        {
            foreach (Employee employee in Employees)
            {
                if (emp.EmployeeId == employee.EmployeeId)
                {
                    employee.Field = emp.Field;
                    employee.FirstName = employee.FirstName;
                    employee.LastName = emp.LastName;
                    employee.Language = emp.Language;
                    employee.Roles = emp.Roles;
                    employee.EmploymentTime = emp.EmploymentTime;
                    employee.EmpType = emp.EmpType;
                    employee.Hardskills = emp.Hardskills;
                    employee.Softskills = emp.Softskills;
                    employee.Rc = emp.Rc;
                    employee.RelevantWorkExperience = emp.RelevantWorkExperience;
                    return true;
                }
            }

            return false;
        }

        public Employee GetEmployeeProfile(int employeeId)
        {
            foreach (Employee emp in Employees)
            {
                if (emp.EmployeeId == employeeId)
                {
                    return emp;
                }
            }

            return null;
        }

        public bool EditStatus(int employeeId, bool status)
        {
            foreach (Employee emp in Employees)
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
            foreach (Employee emp in Employees)
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