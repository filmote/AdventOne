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
            var employees = new List<Employee>
            {
            new Employee{EmployeeName="Simon"},
            new Employee{EmployeeName="Kurt"},
            new Employee{EmployeeName="Bob"},
            };

            employees.ForEach(s => context.Employees.Add(s));
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
            new Project{EmployeeID=1, CustomerID=1,ProjectName="Project1"},
            new Project{EmployeeID=1, CustomerID=1,ProjectName="Project2"},
            new Project{EmployeeID=1, CustomerID=2,ProjectName="Project3"},
            };

            projects.ForEach(s => context.Projects.Add(s));
            context.SaveChanges();

            var tasks = new List<Task>
            {
            new Task{ProjectID=1,Description="P1, Task1",FullText="<p>Hello!</p>"},
            new Task{ProjectID=1,Description="P1, Task2"},
            new Task{ProjectID=1,Description="P1, Task3"},
            new Task{ProjectID=2,Description="P2, Task1"},
            new Task{ProjectID=2,Description="P2, Task2"},
            new Task{ProjectID=2,Description="P2, Task3"},
            new Task{ProjectID=3,Description="P3, Task1"},
            new Task{ProjectID=3,Description="P3, Task2"},
            new Task{ProjectID=3,Description="P3, Task1"},
            };
            tasks.ForEach(s => context.Tasks.Add(s));
            context.SaveChanges();
          
        }
    }
}