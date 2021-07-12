using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AVATI.Data
{
    public class HardskillService: IHardskillService
    {
        private IConfiguration _config;
        private bool _toTest;
        
        public HardskillService(IConfiguration config)
        {
            _toTest = false;
            _config = config;
        }
        
        public HardskillService()
        {
            _toTest = true;
        }

        public async Task<Hardskill> GetHardskillOrCategory(string description)
        {
            return await GetHardskill(description) ?? await GetHardskillCategory(description);
        }
        
        private IDbConnection GetConnection()
        {
            if (!_toTest) return new SqlConnection(_config.GetConnectionString("AVATI-Database"));
            var json = File.ReadAllText("appsettings.json");
            var jObject = JObject.Parse(json);
            var name = (string) jObject["ConnectionStrings"]?["TEST-Database"];
            return new SqlConnection(name);
        }

        public async Task<bool> CreateHardskill(Hardskill hardskill)
        {
            using var db = GetConnection();
            
            var insertQuery = "INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 1)";
            var insertRows = await db.ExecuteAsync(insertQuery, new {description = hardskill.Description});
            if (insertRows != 1) return false;
            if (hardskill.Uppercat != null)
            {
                foreach (var cat in hardskill.Uppercat)
                {
                    var subcatQuery = "INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)";
                    var subcatrows = await db.ExecuteAsync(subcatQuery, 
                        new{ uppercat = cat, subcat = hardskill.Description});
                    if (subcatrows != 1) return false;
                }
            }

            return true;
        }

        public async Task<bool> UpdateHardskill(Hardskill newHardskill, Hardskill oldHardskill)
        {
            using var db = GetConnection();
            var updateDesc = "UPDATE Hardskill SET Description = @newDescription WHERE Description = @oldDescription;";
            if (await db.ExecuteAsync(updateDesc, 
                new{ newDescription = newHardskill.Description, oldDescription = oldHardskill.Description }) != 1) return false;
            
            var deleteUppercat = "DELETE FROM Hardskill_Subcat WHERE Subcat = @oldDescription";
            await db.ExecuteAsync(deleteUppercat, 
                new{ oldDescription = oldHardskill.Description});
            

            foreach (var cat in newHardskill.Uppercat)
            {
                var insertUppercat = "INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)";
                if (await db.ExecuteAsync(insertUppercat, 
                    new{ uppercat = cat, subcat = newHardskill.Description }) != 1) return false;
            }

            return true;
        }

        public async Task<bool> DeleteHardskill(string description)
        {
            using var db = GetConnection();
            var deleteUppercat = "DELETE FROM Hardskill_Subcat WHERE Subcat = @hardskill";
            await db.ExecuteAsync(deleteUppercat, new{ hardskill = description });
            var deleteSkill = "DELETE FROM Hardskill WHERE Description = @hardskill";
            var deleteSRows = await db.ExecuteAsync(deleteSkill, new{ hardskill = description });
            return deleteSRows == 1;
        }
        
        public async Task<Hardskill> GetHardskill(string description)
        {
            using var db = GetConnection();
            var hardskill = (await db.QueryAsync<Hardskill>("SELECT * FROM Hardskill WHERE Description = @hardskill AND IsHardskill = 1", 
                new{ hardskill = description })).SingleOrDefault();
            
            if (hardskill == null) return null;
            hardskill.Subcat = null;
            hardskill.Uppercat = (await db.QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat", 
                new{ subcat = description })).ToList();
            return hardskill;
        }
        
        public async Task<List<Hardskill>> GetAllHardskills()
        {
            using var db = GetConnection();
            var allHardskills = (await db.QueryAsync<Hardskill>("SELECT * FROM Hardskill WHERE IsHardskill = 1")).ToList();

            foreach (var hardskill in allHardskills)
            {
                hardskill.Subcat = null;
                hardskill.Uppercat = (await db.QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat",
                    new{ subcat = hardskill.Description })).ToList();
            }

            return allHardskills;
        }
        
        public async Task<bool> CreateHardskillCategory(Hardskill hardskillcat)
        {
            using var db = GetConnection();
            var insertQuery = "INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)";
            var insertRows = await db.ExecuteAsync(insertQuery, new {description = hardskillcat.Description});
            if (insertRows != 1) return false;
            
            int uppercatRows;

            foreach (var skill in hardskillcat.Subcat)
            {
                if (hardskillcat.Uppercat != null && hardskillcat.Uppercat.Any())
                {
                    var deletecatRows = await db.ExecuteAsync("DELETE FROM Hardskill_Subcat WHERE Subcat = @subcat AND Uppercat = @uppercat", 
                        new { uppercat = hardskillcat.Uppercat[0], subcat = skill });
                    if (deletecatRows > 1) return false;
                }

                uppercatRows = await db.ExecuteAsync("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)", 
                    new { uppercat = hardskillcat.Description, subcat = skill });

                if(uppercatRows != 1) return false;
            }

            if (hardskillcat.Uppercat != null && hardskillcat.Uppercat.Any())
            {
                uppercatRows = await db.ExecuteAsync("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)",
                    new{ uppercat = hardskillcat.Uppercat[0], subcat = hardskillcat.Description });
               return uppercatRows == 1;
            }
            
            return true;
        }

        public async Task<bool> RenameHardskillCategory(string oldDescription, string newDescription)
        {
            using var db = GetConnection();
            var updateCat = "UPDATE Hardskill SET Description = @newD WHERE Description = @oldD;" +
                            "UPDATE Hardskill_Subcat SET Subcat = @newD WHERE Subcat = @oldD;" +
                            "UPDATE Hardskill_Subcat SET Uppercat = @newD WHERE Uppercat = @oldD";
            return await db.ExecuteAsync(updateCat, new{ newD = newDescription, oldD = oldDescription }) > 0;
        }

        public async Task<bool> DeleteHardskillCategory(string description)
        {
            using var db = GetConnection();
            var cat = await GetHardskillCategory(description);
            
            if (cat?.Subcat == null) return false;
            
            foreach (var skill in cat.Subcat)
            {
                var upperCatsOfSkill = (await db.QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat",
                    new {subcat = skill})).ToList();
                if (cat.Uppercat != null && cat.Uppercat.Any() && !upperCatsOfSkill.Contains(cat.Uppercat[0]))
                {
                    var insertUpper = await db.ExecuteAsync("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)", 
                        new{ uppercat = cat.Uppercat[0], subcat = skill });
                    if (insertUpper != 1) return false;
                }
            }
            
            await db.ExecuteAsync("DELETE FROM Hardskill_Subcat WHERE Uppercat = @category OR Subcat = @category",
                new{category = description});

            var delete = await db.ExecuteAsync("DELETE FROM Hardskill WHERE Description = @category", 
                new{category = description});

            return delete == 1;
        }

        public async Task<bool> ContainsAnyHardskills(string description)
        {
            using var db = GetConnection();
            var subCategorys = (await db.QueryAsync<string>("SELECT Subcat FROM Hardskill_Subcat WHERE Uppercat = @cat", 
                new{ cat = description })).ToList();

            foreach (var subCat in subCategorys)
            {
                if ((await db.QueryAsync<bool>("SELECT IsHardskill FROM Hardskill WHERE Description = @sub",
                    new {sub = subCat})).Single()) return true;
            }

            return false;
        }

        public async Task<bool> ContainsJustHardskills(string description)
        {
            using var db = GetConnection();
            var subCategorys = (await db.QueryAsync<string>("SELECT Subcat FROM Hardskill_Subcat WHERE Uppercat = @cat", 
                new{ cat = description })).ToList();

            if (!subCategorys.Any()) return false;

            foreach (var subCat in subCategorys)
            {
                var isHardskill = (await db.QueryAsync<bool>("SELECT IsHardskill FROM Hardskill WHERE Description = @sub",
                    new {sub = subCat})).Single();
                if (!isHardskill) return false;
            }

            return true;
        }

        public async Task<List<Hardskill>> GetAllHardskillCategorys()
        {
            using var db = GetConnection();
            var allCategorys = (await GetConnection().QueryAsync<Hardskill>("SELECT * FROM Hardskill WHERE IsHardskill = 0")).ToList();

            foreach (var cat in allCategorys)
            {
                cat.Subcat = (await db.QueryAsync<string>("SELECT Subcat FROM Hardskill_Subcat WHERE Uppercat = @uppercat",
                    new{ uppercat = cat.Description })).ToList();
                cat.Uppercat = (await db.QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat",
                    new {subcat = cat.Description})).ToList();
            }

            return allCategorys;
        }

        public async Task<Hardskill> GetHardskillCategory(string description)
        {
            using var db = GetConnection();
            var cat = (await db.QueryAsync<Hardskill>("SELECT * FROM Hardskill WHERE Description = @cat AND IsHardskill = 0",
                new {cat = description})).SingleOrDefault();
            if (cat == null) return null;
            
            cat.Subcat = (await db.QueryAsync<string>("SELECT Subcat FROM Hardskill_Subcat WHERE Uppercat = @uppercat",
                new {uppercat = description})).ToList();
            cat.Uppercat = (await db.QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat", 
                new{ subcat = description })).ToList();
            return cat;
        }
        
        public async Task<bool> EditHardskillsCategory(string hardskillcat, List<string> hardskills)
        {
            using var db = GetConnection();
            var cat = await GetHardskillCategory(hardskillcat);
            var copyHardskills = new List<string>(hardskills);
            
            if (cat?.Subcat == null) return false;
            
            foreach (var skill in cat.Subcat)
            {
                var isHardskill = (await db.QueryAsync<bool>(
                    "SELECT IsHardskill FROM Hardskill WHERE Description = @skillOrCat",
                    new {skillOrCat = skill})).Single();
                if (!isHardskill) continue;
                if (copyHardskills.Exists(x => x == skill))
                {
                    copyHardskills.Remove(skill);
                    continue;
                }
                
                var deleteSub = await db.ExecuteAsync("DELETE FROM Hardskill_Subcat WHERE Subcat = @subcat AND Uppercat = @uppercat",
                    new { subcat = skill, uppercat = hardskillcat });
                if (deleteSub != 1) return false; 
            }
            
            foreach (var hardskill in copyHardskills)
            {
                var isHardskill = (await db.QueryAsync<bool>(
                    "SELECT IsHardskill FROM Hardskill WHERE Description = @skillOrCat",
                    new {skillOrCat = hardskill})).Single();
                if (!isHardskill) continue;
                var uppercatRows = await db.ExecuteAsync("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)", 
                    new {uppercat = hardskillcat, subcat = hardskill });
                if(uppercatRows != 1) return false;
            }

            return true;
        }

        public async Task<List<string>> GetHardskillsOfCategory(string description)
        {
            using var db = GetConnection();
            var subDescription = (await db.QueryAsync<string>(
                "SELECT Subcat FROM Hardskill_Subcat WHERE Uppercat = @uppercat ", new{ uppercat = description})).ToList();

            var hardskills = new List<string>();
            
            foreach (var subcat in subDescription)
            {
                var isHardskill = (await db.QueryAsync<bool>(
                    "SELECT IsHardskill FROM Hardskill WHERE Description = @sub ", new{ sub = subcat})).Single();
                if (isHardskill && !hardskills.Contains(subcat))
                    hardskills.Add(subcat);
                else
                {
                    var next = await GetHardskillsOfCategory(subcat);
                    foreach (var hardskill in next.Where(x => !hardskills.Contains(x)))
                        hardskills.Add(hardskill);
                }
            }

            return hardskills;
        }

        public async Task<List<string>> GetAllDesCategorys()
        {
            using var db = GetConnection();
            return (await db.QueryAsync<string>("SELECT Description FROM Hardskill WHERE IsHardskill = 0")).ToList();
        }

        public async Task<List<string>> GetAllDesHardskills()
        {
            using var db = GetConnection();
            return (await db.QueryAsync<string>("SELECT Description FROM Hardskill WHERE IsHardskill = 1")).ToList();
        }

        public async Task<List<string>> GetAllRoots()
        {
            using var db = GetConnection();
            var allRoots = new List<string>();
            var allCategorys = await GetAllHardskillCategorys();

            foreach (var cat in allCategorys.Where(x => x.Uppercat == null || !x.Uppercat.Any()))
            {
                if(!allRoots.Contains(cat.Description))
                    allRoots.Add(cat.Description);
            }

            return allRoots;
        }

        public async Task<bool> CheckExistHardskill(string description)
        {
            using var db = GetConnection();
            var trimDesc = description.Replace(" ", "");
            var allCatsHardskills = (await db.QueryAsync<string>("SELECT Description FROM Hardskill")).ToList();
            foreach (var skillOrCat in allCatsHardskills)
            {
                if (trimDesc.Equals(skillOrCat.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        public bool CheckEmptyHardskill(string description)
        {
            return !(string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description));
        }

        public bool CheckLengthHardskill(string description)
        {
            return description.Length <= 150;
        }

        public async Task<bool> DeleteHardskillOutOfCategory(string category, string hardskill)
        {
            using var db = GetConnection();
            var deleteRows = await db.ExecuteAsync("DELETE FROM Hardskill_Subcat WHERE Uppercat = @upperCat AND Subcat = @subCat",
                new{ upperCat = category, subcat = hardskill });
            return deleteRows == 1;
        }
    }
}