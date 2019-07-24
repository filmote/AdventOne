using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdventOne.Models {

    public class Project : BaseEntity {

        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string ProjectName { get; set; }
        public int? EmployeeID { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public Division Division { get; set; }
        public Location Location { get; set; }
        public Branch Branch { get; set; }
        public DateTime InvoiceDate { get; set; }
        public PaymentTerms PaymentTerms { get; set; }

        [EnumDataType(typeof(SalesStage))]
        public SalesStage SalesStage { get; set; }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        public decimal Revenue { get; set; }
        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        public decimal COS { get; set; }
        [DataType(DataType.Currency)]
        public decimal Margin { get; set; }

        //public String SalesStageDisplayName() {

        //    return base.GetEnumDisplayName(this.SalesStage);

        //}

        //public String ProjectStatusDisplayName() {

        //    return base.GetEnumDisplayName(this.ProjectStatus);

        //}

        //public String DivisionDisplayName() {

        //    return base.GetEnumDisplayName(this.ProjectStatus);

        //}

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

    }

}