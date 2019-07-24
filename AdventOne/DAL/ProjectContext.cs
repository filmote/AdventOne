using AdventOne.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AdventOne.DAL {

    public class ProjectContext : DbContext {

        public ProjectContext() : base("ProjectContext") { }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceNote> InvoiceNotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Customer>()
            //    .HasOptional<Employee>(s => s.Employee)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Project>()
            //    .HasOptional<Employee>(s => s.Employee)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

        }

    }

}