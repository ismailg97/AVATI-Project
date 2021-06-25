using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AVATI.Data
{
    public class HardskillService: IHardskillService
    {
        private IConfiguration _config;
        
        public HardskillService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Hardskill> GetHardskillOrCategory(string description)
        {
            return (await GetHardskill(description)) ?? await GetHardskillCategory(description);
        }
        
        private IDbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("AVATI-Database"));
        }

        public async Task<bool> CreateHardskill(Hardskill hardskill)
        {
            if ((await GetConnection().QueryAsync<int>("SELECT COUNT(*) FROM Hardskill WHERE Description = @description",
                new {description = hardskill.Description})).Single() != 0) return false;
            
            var insertQuery = "INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 1)";
            var insertRows = await GetConnection().ExecuteAsync(insertQuery, new {description = hardskill.Description});
            if (insertRows != 1) return false;
            if (hardskill.Uppercat != null)
            {
                foreach (var cat in hardskill.Uppercat)
                {
                    var subcatQuery = "INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)";
                    var subcatrows = await GetConnection().ExecuteAsync(subcatQuery, 
                        new{ uppercat = cat, subcat = hardskill.Description});
                    if (subcatrows != 1) return false;
                }
            }

            return true;
        }

        public async Task<bool> UpdateHardskill(Hardskill newHardskill, Hardskill oldHardskill)
        {
            var updateDesc = "UPDATE Hardskill SET Description = @newDescription WHERE Description = @oldDescription;";
            if (await GetConnection().ExecuteAsync(updateDesc, 
                new{ newDescription = newHardskill.Description, oldDescription = oldHardskill.Description }) != 1) return false;
            
            if (newHardskill.Uppercat == null || !newHardskill.Uppercat.Any())
            {
                var deleteUppercat = "DELETE FROM Hardskill_Subcat WHERE Subcat = @oldDescription";
                await GetConnection().ExecuteAsync(deleteUppercat, 
                    new{ oldDescription = oldHardskill.Description});
                return true;
            }

            foreach (var cat in oldHardskill.Uppercat)
            {
                if (newHardskill.Uppercat.Exists(x => x == cat))
                {
                    var updateUppercat = 
                        "UPDATE Hardskill_Subcat SET Subcat = @newDescription WHERE Uppercat = @uppercat AND Subcat = @oldDescription";
                    if(await GetConnection().ExecuteAsync(updateUppercat, 
                        new{ newDescription = newHardskill.Description, uppercat = cat, oldDescription = oldHardskill.Description})!= 1) return false;
                    newHardskill.Uppercat.Remove(cat);

                } else {
                    
                    var deleteUppercat = "DELETE FROM Hardskill_Subcat WHERE Uppercat = @uppercat AND Subcat = @oldDescription";
                    if(await GetConnection().ExecuteAsync(deleteUppercat, 
                        new{ oldDescription = oldHardskill.Description, uppercat = cat}) != 1) return false;
                }
            }

            foreach (var cat in newHardskill.Uppercat)
            {
                var insertUppercat = "INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)";
                if (await GetConnection().ExecuteAsync(insertUppercat, 
                    new{ uppercat = cat, subcat = newHardskill.Description }) != 1) return false;
            }

            return true;
        }

        public async Task<bool> DeleteHardskill(string description)
        {
            var deleteUppercat = "DELETE FROM Hardskill_Subcat WHERE Subcat = @hardskill";
            var deleteURows = await GetConnection().ExecuteAsync(deleteUppercat, new{ hardskill = description });
            if (deleteURows > 1) return false;
            var deleteSkill = "DELETE FROM Hardskill WHERE Description = @hardskill";
            var deleteSRows = await GetConnection().ExecuteAsync(deleteSkill, new{ hardskill = description });
            return deleteSRows == 1;
        }
        
        public async Task<Hardskill> GetHardskill(string description)
        {
            var hardskill = (await GetConnection().QueryAsync<Hardskill>("SELECT * FROM Hardskill WHERE Description = @hardskill AND IsHardskill = 1", 
                new{ hardskill = description })).SingleOrDefault();
            
            if (hardskill == null) return null;
            hardskill.Subcat = null;
            hardskill.Uppercat = (await GetConnection().QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat", 
                new{ subcat = description })).ToList();
            return hardskill;
        }
        
        public async Task<List<Hardskill>> GetAllHardskills()
        {
            var allHardskills = (await GetConnection().QueryAsync<Hardskill>("SELECT * FROM Hardskill WHERE IsHardskill = 1")).ToList();

            foreach (var hardskill in allHardskills)
            {
                hardskill.Subcat = null;
                hardskill.Uppercat = (await GetConnection().QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat",
                    new{ subcat = hardskill.Description })).ToList();
            }

            return allHardskills;
        }
        
        public async Task<bool> CreateHardskillCategory(Hardskill hardskillcat)
        {
            var insertQuery = "INSERT INTO Hardskill (Description, IsHardskill) VALUES (@description, 0)";
            var insertRows = await GetConnection().ExecuteAsync(insertQuery, new {description = hardskillcat.Description});
            if (insertRows != 1) return false;
            
            int uppercatRows;

            foreach (var skill in hardskillcat.Subcat)
            {
                uppercatRows = await GetConnection().ExecuteAsync("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)", 
                    new { uppercat = hardskillcat.Description, subcat = skill });
                if(uppercatRows != 1) return false;
            }

            if (hardskillcat.Uppercat != null && hardskillcat.Uppercat.Any())
            {
                uppercatRows = await GetConnection().ExecuteAsync("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)",
                    new{ uppercat = hardskillcat.Uppercat[0], subcat = hardskillcat.Description });
               return uppercatRows == 1;
            }
            
            return true;
        }

        public async Task<bool> UpdateHardskillCategory(string oldDescription, string newDescription)
        {
            var updateCat = "UPDATE Hardskill SET Description = @newD WHERE Description = @oldD;" +
                            "UPDATE Hardskill_Subcat SET Subcat = @newD WHERE Subcat = @oldD;" +
                            "UPDATE Hardskill_Subcat SET Uppercat = @newD WHERE Uppercat = @oldD";
            return await GetConnection().ExecuteAsync(updateCat, new{ newD = newDescription, oldD = oldDescription }) > 0;
        }

        public async Task<bool> DeleteHardskillCategory(string description)
        {
            var cat = await GetHardskillCategory(description);
            
            if (cat?.Subcat == null) return false;
            
            foreach (var skill in cat.Subcat)
            {
                if (cat.Uppercat != null && cat.Uppercat.Any())
                {
                    var insertUpper = await GetConnection().ExecuteAsync("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)", 
                        new{ uppercat = cat.Uppercat, subcat = skill });
                    if (insertUpper != 1) return false;
                }
            }
            
            await GetConnection().ExecuteAsync("DELETE FROM Hardskill_Subcat WHERE Uppercat = @category OR Subcat = @category",
                new{category = description});

            var delete = await GetConnection().ExecuteAsync("DELETE FROM Hardskill WHERE Description = @category", 
                new{category = description});

            return delete == 1;
        }

        public async Task<bool> ContainsAnyHardskills(string description)
        {
            var subCategorys = (await GetConnection().QueryAsync<string>("SELECT Subcat FROM Hardskill_Subcat WHERE Uppercat = @cat", 
                new{ cat = description })).ToList();

            foreach (var subCat in subCategorys)
            {
                if ((await GetConnection().QueryAsync<bool>("SELECT IsHardskill FROM Hardskill WHERE Description = @sub",
                    new {sub = subCat})).Single()) return true;
            }

            return false;
        }

        public async Task<List<Hardskill>> GetAllHardskillCategorys()
        {
            var allCategorys = (await GetConnection().QueryAsync<Hardskill>("SELECT * FROM Hardskill WHERE IsHardskill = 0")).ToList();

            foreach (var cat in allCategorys)
            {
                cat.Subcat = (await GetConnection().QueryAsync<string>("SELECT Subcat FROM Hardskill_Subcat WHERE Uppercat = @uppercat",
                    new{ uppercat = cat.Description })).ToList();
                cat.Uppercat = (await GetConnection().QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat",
                    new {subcat = cat.Description})).ToList();
            }

            return allCategorys;
        }

        public async Task<Hardskill> GetHardskillCategory(string description)
        {
            var cat = (await GetConnection().QueryAsync<Hardskill>("SELECT * FROM Hardskill WHERE Description = @cat AND IsHardskill = 0",
                new {cat = description})).SingleOrDefault();
            if (cat == null) return null;
            
            cat.Subcat = (await GetConnection().QueryAsync<string>("SELECT Subcat FROM Hardskill_Subcat WHERE Uppercat = @uppercat",
                new {uppercat = description})).ToList();
            cat.Uppercat = (await GetConnection().QueryAsync<string>("SELECT Uppercat FROM Hardskill_Subcat WHERE Subcat = @subcat", 
                new{ subcat = description })).ToList();
            return cat;
        }
        
        public async Task<bool> EditHardskillsCategory(string hardskillcat, List<string> hardskills)
        {
            var cat = await GetHardskillCategory(hardskillcat);
            
            if (cat?.Subcat == null) return false;
            
            foreach (var skill in cat.Subcat.ToList())
            {
                if (!(await GetHardskillOrCategory(skill)).IsHardskill) continue;
                if (hardskills.Exists(x => x == skill))
                {
                    hardskills.Remove(skill);
                    continue;
                }
                
                var deleteSub = await GetConnection().ExecuteAsync("DELETE FROM Hardskill_Subcat WHERE Subcat = @subcat",
                    new { subcat = skill });
                if (deleteSub != 1) return false; 
            }
            
            foreach (var hardskill in hardskills.ToList())
            {
                var uppercatRows = await GetConnection().ExecuteAsync("INSERT INTO Hardskill_Subcat (Uppercat, Subcat) VALUES (@uppercat, @subcat)", 
                    new {uppercat = hardskillcat, subcat = hardskill });
                if(uppercatRows != 1) return false;
            }

            return true;
        }
    }
}