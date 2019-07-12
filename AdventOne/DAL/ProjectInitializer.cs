using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AdventOne.Models;

namespace AdventOne.DAL
{
    public class ProjectInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ProjectContext>
    {
        protected override void Seed(ProjectContext context)
        {

            Permission admin = new Permission { PermissionName = "Admin" };
            var permissions = new List<Permission>
            {
            admin,
            new Permission{PermissionName="DoStuff"},
            new Permission{PermissionName="DoMoreStuff"},
            };

            permissions.ForEach(s => context.Permissions.Add(s));
            context.SaveChanges();
            Employee employee = new Employee { EmployeeName = "Simon", EmailAddress = "simon.holmes@adventone.com" };

            var employees = new List<Employee>
            {
            employee,
            new Employee{EmployeeName="Kurt", EmailAddress="kurt.teo@adventone.com"},
            new Employee{EmployeeName="Bob", EmailAddress="robert.bassat@adventone.com"},
            };

            employees.ForEach(s => context.Employees.Add(s));
            context.SaveChanges();

            int employeeId = employee.ID;
//            employee = null;
  //          employee = context.Employees.Find(employeeId);
            employee.Permissions = new List<Permission>();
            employee.Permissions.Add(admin);
            context.SaveChanges();

            var customers = new List<Customer>
            {
            new Customer{EmployeeID=1, CustomerName="Customer A"},
            new Customer{EmployeeID=1, CustomerName="Customer B"},
            new Customer{EmployeeID=1, CustomerName="Customer C"},
            new Customer{EmployeeID=2, CustomerName="Customer D"},
            new Customer{EmployeeID=2, CustomerName="Customer E"},
            new Customer{EmployeeID=2, CustomerName="Customer F"},
            new Customer{EmployeeID=2, CustomerName="Customer G"},
            new Customer{EmployeeID=3, CustomerName="Customer H"},
            new Customer{EmployeeID=3, CustomerName="Customer I"},
            new Customer{EmployeeID=3, CustomerName="Customer J"},
            new Customer{EmployeeID=3, CustomerName="Customer K"},
            };

            customers.ForEach(s => context.Customers.Add(s));
            context.SaveChanges();


            var projects = new List<Project>
            {
            new Project{EmployeeID=1, CustomerID=1,ProjectName="Project1",Status=Status.LookingGood, Revenue=2M, COS=1M, Margin=1M},
            new Project{EmployeeID=1, CustomerID=1,ProjectName="Project2",Status=Status.LookingGood, Revenue=3M, COS=0M, Margin=3M},
            new Project{EmployeeID=1, CustomerID=2,ProjectName="Project3",Status=Status.PipeDream, Revenue=3M, COS=0M, Margin=3M},
            };

            projects.ForEach(s => context.Projects.Add(s));
            context.SaveChanges();

            var tasks = new List<Task>
            {
            new Task{ProjectID=1,Description="P1, Task1",FullText="<p>Hello!</p>", Sequence=0, Price=100M, RevenueType=RevenueType.REV},
            new Task{ProjectID=1,Description="P1, Task2", Sequence=1, Price=100M, RevenueType=RevenueType.REV},
            new Task{ProjectID=1,Description="P1, Task3", Sequence=2, Price=234M, RevenueType=RevenueType.COS},
            new Task{ProjectID=2,Description="P2, Task1", Sequence=0, Price=100M, RevenueType=RevenueType.REV},
            new Task{ProjectID=2,Description="P2, Task2", Sequence=1, Price=100M, RevenueType=RevenueType.REV},
            new Task{ProjectID=2,Description="P2, Task3", Sequence=2, Price=100M, RevenueType=RevenueType.REV},
            new Task{ProjectID=3,Description="P3, Task1", Sequence=0, Price=100M, RevenueType=RevenueType.REV},
            new Task{ProjectID=3,Description="P3, Task2", Sequence=1, Price=100M, RevenueType=RevenueType.REV},
            new Task{ProjectID=3,Description="P3, Task1", Sequence=2, Price=100M, RevenueType=RevenueType.REV},
            };
            tasks.ForEach(s => context.Tasks.Add(s));
            context.SaveChanges();
          
        }
    }
}