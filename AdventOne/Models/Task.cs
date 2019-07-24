using System;
using System.ComponentModel.DataAnnotations;
using AdventOne.Validators;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventOne.Models {

    public class Task : ICloneable {

        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int? SupplierID { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }

        public DateTime InvoiceDate { get; set; }
        public SalesStage SalesStage { get; set; }
        public RevenueType RevenueType { get; set; }

        public virtual Project Project { get; set; }
        public virtual Supplier Supplier { get; set; }

        [AllowHtml]
        [UIHint("tinymce_jquery_full")]
        public string FullText { get; set; }
        

        // Base values 

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        public decimal ExtendedPrice { get; set; }

        [Range(0, 1000000)]
        public int Quantity { get; set; }



        // Product

        [Range(0, 1000000)]
        [RequiredForCOSandREVTasks(ErrorMessage = "A quantity is mandatory for REV and COS items.")]
        public int ProductQuantity { get; set; }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [RequiredForCOSandREVTasks(ErrorMessage = "A unit price is mandatory for REV and COS items.")]
        [NotMapped]
        public decimal ProductUnitPrice { get; set; }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [NotMapped]
        public decimal ProductExtendedPrice {
            get {
                return this.ProductQuantity * this.ProductUnitPrice;
            }
            set { }
        }


        // Services

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [RequiredForSVCTasks(ErrorMessage="An hourly rate is mandatory for SVC items.")]
        public decimal HourlyRate { get; set; }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [RequiredForSVCTasks(ErrorMessage = "The number of hours is mandatory for SVC items.")]
        public int ServicesQuantity { get; set; }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [NotMapped]
        public decimal ServicesExtendedPrice {
            get {
                return this.HourlyRate * this.ServicesQuantity;
            }
            set { }
        }



        public object Clone() {

            Task newTask = new Task();
            newTask.ProjectID = this.ProjectID;
            newTask.Description = this.Description;
            newTask.RevenueType = this.RevenueType;
            newTask.UnitPrice = this.UnitPrice;
            newTask.ExtendedPrice = this.ExtendedPrice;
            newTask.Quantity = this.Quantity;
            newTask.HourlyRate = this.HourlyRate;
            newTask.SalesStage = this.SalesStage;
            newTask.FullText = this.FullText;
            newTask.InvoiceDate = this.InvoiceDate;

            return newTask;

        }

        public void PopulateDisplayFields() {

            if (this.RevenueType == RevenueType.COS || this.RevenueType == RevenueType.REV) {
                this.ProductQuantity = this.Quantity;
                this.ProductUnitPrice = this.UnitPrice;
                this.ProductExtendedPrice = this.ExtendedPrice;
            }
            else {
                this.ServicesQuantity = this.Quantity;
                this.HourlyRate = this.UnitPrice;
                this.ServicesExtendedPrice = this.ExtendedPrice;

            }

        }

    }

}