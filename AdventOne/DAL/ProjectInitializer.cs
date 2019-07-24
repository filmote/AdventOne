using System;
using System.Collections.Generic;
using AdventOne.Models;

namespace AdventOne.DAL {
    public class ProjectInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ProjectContext>
    {
        protected override void Seed(ProjectContext context)
        {

            var suppliers = new List<Supplier>
            {
            new Supplier{SupplierName="IBM Australia"},
            new Supplier{SupplierName="Supplier 2"},
            new Supplier{SupplierName="Supplier 3"},
            new Supplier{SupplierName="Supplier 4"},
            new Supplier{SupplierName="Supplier 5"},
            new Supplier{SupplierName="Supplier 6"},
            };

            suppliers.ForEach(s => context.Suppliers.Add(s));
            context.SaveChanges();



            Permission admin = new Permission { PermissionName = "Admin" };
            var permissions = new List<Permission>
            {
            admin,
            new Permission{PermissionName="SalesManager"},
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
            new Project{EmployeeID=1, CustomerID=1,ProjectName="Project1",SalesStage=SalesStage.LookingGood, Revenue=2M, COS=1M, Margin=1M, InvoiceDate=DateTime.Now },
            new Project{EmployeeID=1, CustomerID=1,ProjectName="Project2",SalesStage=SalesStage.LookingGood, Revenue=3M, COS=0M, Margin=3M, InvoiceDate=DateTime.Now},
            new Project{EmployeeID=1, CustomerID=2,ProjectName="Project3",SalesStage=SalesStage.PipeDream, Revenue=3M, COS=0M, Margin=3M, InvoiceDate=DateTime.Now},
            };

            projects.ForEach(s => context.Projects.Add(s));
            context.SaveChanges();

            //var tasks = new List<Task>
            //{
            //new Task{ProjectID=1,Description="P1, Task1",FullText="<p>Hello!</p>", Sequence=0, ExtendedPrice=100M, RevenueType=RevenueType.REV, InvoiceDate=DateTime.Now},
            //new Task{ProjectID=1,Description="P1, Task2", Sequence=1, UnitPrice = 100M, ExtendedPrice=100M, RevenueType=RevenueType.REV, InvoiceDate=DateTime.Now},
            //new Task{ProjectID=1,Description="P1, Task3", Sequence=2, UnitPrice = 100M, ExtendedPrice=234M, RevenueType=RevenueType.COS, InvoiceDate=DateTime.Now},
            //new Task{ProjectID=2,Description="P2, Task1", Sequence=0, UnitPrice = 100M, ExtendedPrice=100M, RevenueType=RevenueType.REV, InvoiceDate=DateTime.Now},
            //new Task{ProjectID=2,Description="P2, Task2", Sequence=1, UnitPrice = 100M, ExtendedPrice=100M, RevenueType=RevenueType.REV, InvoiceDate=DateTime.Now},
            //new Task{ProjectID=2,Description="P2, Task3", Sequence=2, UnitPrice = 100M, ExtendedPrice=100M, RevenueType=RevenueType.REV, InvoiceDate=DateTime.Now},
            //new Task{ProjectID=3,Description="P3, Task1", Sequence=0, UnitPrice = 100M, ExtendedPrice=100M, RevenueType=RevenueType.REV, InvoiceDate=DateTime.Now},
            //new Task{ProjectID=3,Description="P3, Task2", Sequence=1, UnitPrice = 100M, ExtendedPrice=100M, RevenueType=RevenueType.REV, InvoiceDate=DateTime.Now},
            //new Task{ProjectID=3,Description="P3, Task1", Sequence=2, UnitPrice = 100M, ExtendedPrice=100M, RevenueType=RevenueType.REV, InvoiceDate=DateTime.Now},
            //};
            //tasks.ForEach(s => context.Tasks.Add(s));
            //context.SaveChanges();
          
        }
    }
}