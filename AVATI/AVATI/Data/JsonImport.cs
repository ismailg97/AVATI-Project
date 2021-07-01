using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace AVATI.Data
{
    public class JsonImport
    {
        //Loads JSon file into respective class
        public JSonStructure.Rootclass ImportJsonFile()
        {
            JSonStructure.Rootclass jSonContainer =
                JsonSerializer.Deserialize<JSonStructure.Rootclass>(File.ReadAllText("Data\\datenbasis.json"));
            
            
            return jSonContainer;
        }

        private void HardskillToDatabase(IDbConnection db, string root, string upperCat, List<string> json)
        {
            db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0);" 
                       + "INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @description)",
                new { description = upperCat, uppercat = root });

            foreach (var hardskill in json)
            {
                if (db.Query<int>("SELECT COUNT(*) FROM Hardskill WHERE Description = @description",
                    new {description = @hardskill}).Single() == 0) 
                {
                    db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 1)", 
                               new { description = hardskill });
                }

                db.Execute("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @description)",
                    new { description = hardskill, uppercat = upperCat });
            }
        }

        public void JsonFileToDatabase(IConfiguration configuration)
        {
            var jsonFile = ImportJsonFile();
            var db = new SqlConnection(configuration.GetConnectionString("AVATI-Database"));

            if (db.Query<int>("SELECT COUNT(*) FROM Field").Single() == 0)
            {
                foreach (var field in jsonFile.fields)
                {
                    db.Execute("INSERT INTO Field (Description) VALUES (@description)",
                        new { description = field });
                }
            }

            if (db.Query<int>("SELECT COUNT(*) FROM dbo.Role").Single() == 0)
            {
                foreach (var role in jsonFile.roles)
                {
                    db.Execute("INSERT INTO dbo.Role (Description) VALUES (@description)",
                        new { description = role });
                }
            }
            
            if (db.Query<int>("SELECT COUNT(*) FROM dbo.Language").Single() == 0)
            {
                foreach (var language in jsonFile.languages)
                {
                    db.Execute("INSERT INTO dbo.Language (Description) VALUES (@description)",
                        new { description = language });
                }
            }
            
            if (db.Query<int>("SELECT COUNT(*) FROM Softskill").Single() == 0)
            {
                foreach (var softskill in jsonFile.Softskills)
                {
                    db.Execute("INSERT INTO Softskill (Description) VALUES (@description)",
                        new { description = softskill });
                }
            }

            if (db.Query<int>("SELECT COUNT(*) FROM Hardskill").Single() == 0)
            {
                //Sprachen und Frameworks
                db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)",
                    new { description = "Sprachen und Frameworks" });
                    
                    //Sprachen
                    HardskillToDatabase(db, "Sprachen und Frameworks", "Sprachen", 
                        jsonFile.skills.SprachenundFrameworks.Sprachen);
                    
                    //Frameworks
                    HardskillToDatabase(db, "Sprachen und Frameworks", "Frameworks", 
                        jsonFile.skills.SprachenundFrameworks.Frameworks);
                    
                    //Bibliotheken
                    HardskillToDatabase(db, "Sprachen und Frameworks", "Bibliotheken", 
                        jsonFile.skills.SprachenundFrameworks.Bibliotheken);
                
                //Tools
                db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)",
                    new { description = "Tools" });
                
                    //Projektmanagement Tools
                    HardskillToDatabase(db, "Tools", "Projektmanagement Tools", 
                        jsonFile.skills.Tools.ProjektmanagementTools);
                    
                    //Konzept- und Ideenmanagement
                    HardskillToDatabase(db, "Tools", "Konzept- und Ideenmanagement", 
                        jsonFile.skills.Tools.KonzeptUndIdeenmanagement);
                    
                    //CI/CD
                    HardskillToDatabase(db, "Tools", "CI/CD", 
                        jsonFile.skills.Tools.CICD);
                    
                    //IDEs
                    HardskillToDatabase(db, "Tools", "IDEs", 
                        jsonFile.skills.Tools.IDEs);
                    
                    //Versionsverwaltung
                    HardskillToDatabase(db, "Tools", "Versionsverwaltung", 
                        jsonFile.skills.Tools.Versionsverwaltung);
                    
                    //Digitales Design
                    HardskillToDatabase(db, "Tools", "Digitales Design", 
                        jsonFile.skills.Tools.DigitalesDesign);
                    
                    //3D-Visualisierung
                    HardskillToDatabase(db, "Tools", "3D-Visualisierung", 
                        jsonFile.skills.Tools._3DVisualisierung);
                    
                    //Virtualisierung
                    HardskillToDatabase(db, "Tools", "Virtualisierung", 
                        jsonFile.skills.Tools.Virtualisierung);
                    
                    //Server- und Infrastruktur-Management
                    HardskillToDatabase(db, "Tools", "Server- und Infrastruktur-Management", 
                        jsonFile.skills.Tools.ServerUndInfrastrukturManagement);
                    
                    //QA
                    HardskillToDatabase(db, "Tools", "QA", 
                        jsonFile.skills.Tools.QA);
                    
                    //CAD
                    HardskillToDatabase(db, "Tools", "CAD", 
                        jsonFile.skills.Tools.CAD);
                    
                    //Package-Management
                    HardskillToDatabase(db, "Tools", "Package-Management", 
                        jsonFile.skills.Tools.PackageManagement);
                    
                    //Build-Management
                    HardskillToDatabase(db, "Tools", "Build-Management", 
                        jsonFile.skills.Tools.BuildManagement);
                    
                    //Datenvisualisierung/-analyse
                    HardskillToDatabase(db, "Tools", "Datenvisualisierung/-analyse", 
                        jsonFile.skills.Tools.DatenvisualisierungAnalyse);
                    
                    //Simulation
                    HardskillToDatabase(db, "Tools", "Simulation", 
                        jsonFile.skills.Tools.Simulation);
                    
                    //Optimierung/technische Mathematik
                    HardskillToDatabase(db, "Tools", "Optimierung/technische Mathematik", 
                        jsonFile.skills.Tools.OptimierungTechnischeMathematik);
                    
                    //Datenbankverwaltung
                    HardskillToDatabase(db, "Tools", "Datenbankverwaltung", 
                        jsonFile.skills.Tools.Datenbankverwaltung);
                    
                //Methoden und Prozesse
                db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)",
                    new { description = "Methoden und Prozesse" });
                
                    //Agile Methoden
                    HardskillToDatabase(db, "Methoden und Prozesse", "Agile Methoden", 
                        jsonFile.skills.MethodenundProzesse.AgileMethoden);
                    
                    //Agile Scaling
                    HardskillToDatabase(db, "Methoden und Prozesse", "Agile Scaling", 
                        jsonFile.skills.MethodenundProzesse.AgileScaling);
                    
                    //UI/UX
                    HardskillToDatabase(db, "Methoden und Prozesse", "UI/UX", 
                        jsonFile.skills.MethodenundProzesse.UIUX);
                    
                    //Software Engineering
                    HardskillToDatabase(db, "Methoden und Prozesse", "Software Engineering", 
                        jsonFile.skills.MethodenundProzesse.SoftwareEngineering);
                    
                    //Modellierung
                    HardskillToDatabase(db, "Methoden und Prozesse", "Modellierung", 
                        jsonFile.skills.MethodenundProzesse.Modellierung);
                    
                    //Projektmanagement
                    HardskillToDatabase(db, "Methoden und Prozesse", "Projektmanagement", 
                        jsonFile.skills.MethodenundProzesse.Projektmanagement);
                    
                //Datenbanken
                db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)",
                    new { description = "Datenbanken" });
                
                    foreach (var hardskill in jsonFile.skills.Datenbanken)
                    {   
                        if (db.Query<int>("SELECT COUNT(*) FROM Hardskill WHERE Description = @description",
                            new {description = @hardskill}).Single() == 0) 
                        {
                            db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 1)", 
                                new { description = hardskill });
                        }

                        db.Execute("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @description)",
                            new { description = hardskill, uppercat = "Datenbanken" });
                    }
                    
                //Betriebssysteme/Cloud/Plattformen/Hardware
                db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)",
                    new { description = "Betriebssysteme/Cloud/Plattformen/Hardware" });
                
                    //Betriebssysteme
                    HardskillToDatabase(db, "Betriebssysteme/Cloud/Plattformen/Hardware", "Betriebssysteme", 
                        jsonFile.skills.BetriebssystemeCloudPlattformenHardware.Betriebssysteme);
                    
                    //Cloud
                    HardskillToDatabase(db, "Betriebssysteme/Cloud/Plattformen/Hardware", "Cloud", 
                        jsonFile.skills.BetriebssystemeCloudPlattformenHardware.Cloud);
                    
                    //Hardware
                    HardskillToDatabase(db, "Betriebssysteme/Cloud/Plattformen/Hardware", "Hardware", 
                        jsonFile.skills.BetriebssystemeCloudPlattformenHardware.Hardware);
                    
                    //Plattformen
                    HardskillToDatabase(db, "Betriebssysteme/Cloud/Plattformen/Hardware", "Plattformen", 
                        jsonFile.skills.BetriebssystemeCloudPlattformenHardware.Plattformen);
                    
                //Schnittstellen und Protokolle
                db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)",
                    new { description = "Schnittstellen und Protokolle" });
                
                    foreach (var hardskill in jsonFile.skills.SchnittstellenundProtokolle)
                    {   
                        db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 1);" 
                            + "INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @description)",
                        new { description = hardskill, uppercat = "Schnittstellen und Protokolle" });
                    }
                    
                //Expertise
                db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)",
                    new { description = "Expertise" });
                
                    foreach (var hardskill in jsonFile.skills.Expertise)
                    {
                        if (hardskill == "Projektmanagement")
                        {
                            db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 1)", 
                                new { description = "Projektleitung" });
                            db.Execute("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @description)",
                                new { description = "Projektleitung", uppercat = "Expertise" });
                            continue;
                        }

                        if (db.Query<int>("SELECT COUNT(*) FROM Hardskill WHERE Description = @description",
                            new {description = @hardskill}).Single() == 0) 
                        {
                            db.Execute("INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 1)", 
                                new { description = hardskill });
                        }

                        db.Execute("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @description)",
                            new { description = hardskill, uppercat = "Expertise" });
                    }
            }
        }
    }
}