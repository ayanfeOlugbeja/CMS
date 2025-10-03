//using CMS.Data;
//using CMS.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace CMS.Seed
//{
//    public static class DbSeeder
//    {
//        public static async Task Seed(AppDbContext context)
//        {
//            await context.Database.MigrateAsync();

            
//            if (!context.Branches.Any())
//            {
//                context.Branches.AddRange(
//                    new Branch { BranchName = "Headquarters", IsActive = true },
//                    new Branch { BranchName = "Lagos Branch", IsActive = true },
//                    new Branch { BranchName = "Abuja Branch", IsActive = true }
//                );
//                await context.SaveChangesAsync();
//            }

            
//            if (!context.Departments.Any())
//            {
//                context.Departments.AddRange(
//                    new Department { DepartmentName = "Human Resources", IsActive = true },
//                    new Department { DepartmentName = "Finance", IsActive = true },
//                    new Department { DepartmentName = "IT", IsActive = true }
//                );
//                await context.SaveChangesAsync();
//            }

          
//            if (!context.Towns.Any())
//            {
//                context.Towns.AddRange(
//                    new Town { TownName = "Ikoyi", IsActive = true },
//                    new Town { TownName = "Victoria Island", IsActive = true },
//                    new Town { TownName = "Wuse", IsActive = true }
//                );
//                await context.SaveChangesAsync();
//            }

          
//            if (!context.Employees.Any())
//            {
//                var branch = context.Branches.First();
//                var dept = context.Departments.First();
//                var town = context.Towns.First();

//                context.Employees.AddRange(
//                    new Employee { FirstName = "John", LastName = "Doe", BranchId = branch.Id, DepartmentId = dept.Id, TownId = town.Id, IsActive = true },
//                    new Employee { FirstName = "Jane", LastName = "Smith", BranchId = branch.Id, DepartmentId = dept.Id, TownId = town.Id, IsActive = true }
//                );
//                await context.SaveChangesAsync();
//            }
//        }
//    }
//}