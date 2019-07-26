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
        public DateTime DueDate { get; set; }
        public DateTime PaymentDate { get; set; }
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
        [NotMapped]
        public int ProductQuantity {
            get {
                return (this.RevenueType == RevenueType.COS || this.RevenueType == RevenueType.REV ? this.Quantity : 0); 
            }
            set {
                if (this.RevenueType == RevenueType.COS || this.RevenueType == RevenueType.REV) {
                    this.Quantity = value;
                    this.ExtendedPrice = this.Quantity * this.UnitPrice;
                }
            }
        }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [RequiredForCOSandREVTasks(ErrorMessage = "A unit price is mandatory for REV and COS items.")]
        [NotMapped]
        public decimal ProductUnitPrice {
            get {
                return (this.RevenueType == RevenueType.COS || this.RevenueType == RevenueType.REV ? this.UnitPrice : 0);
            }
            set {
                if (this.RevenueType == RevenueType.COS || this.RevenueType == RevenueType.REV) {
                    this.UnitPrice = value;
                    this.ExtendedPrice = this.Quantity * this.UnitPrice;
                }
            }
        }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [NotMapped]
        public decimal ProductExtendedPrice {
            get {
                return this.UnitPrice * this.Quantity;
            }
            set {
                this.ExtendedPrice = value;
            }
        }


        // Services

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [RequiredForSVCTasks(ErrorMessage="An hourly rate is mandatory for SVC items.")]
        [NotMapped]
        public decimal HourlyRate {
            get {
                return (this.RevenueType == RevenueType.SVC ? this.UnitPrice : 0);
            }
            set {
                if (this.RevenueType == RevenueType.SVC) {
                    this.UnitPrice = value;
                    this.ExtendedPrice = this.Quantity * this.UnitPrice;
                }
            }
        }

        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [RequiredForSVCTasks(ErrorMessage = "The number of hours is mandatory for SVC items.")]
        [NotMapped]
        public int ServicesQuantity {
            get {
                return (this.RevenueType == RevenueType.SVC ? this.Quantity : 0);
            }
            set {
                if (this.RevenueType == RevenueType.SVC) {
                    this.Quantity = value;
                    this.ExtendedPrice = this.Quantity * this.UnitPrice;
                }
            }
        }
        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        [NotMapped]
        public decimal ServicesExtendedPrice {

            get {
                return this.UnitPrice * this.Quantity;
            }
            set {
                this.ExtendedPrice = value;
            }

        }
               
        public object Clone() {

            Task newTask = new Task();
            newTask.ProjectID = this.ProjectID;
            newTask.Description = this.Description;
            newTask.RevenueType = this.RevenueType;
            newTask.UnitPrice = this.UnitPrice;
            newTask.ExtendedPrice = this.ExtendedPrice;
            newTask.Quantity = this.Quantity;
//            newTask.HourlyRate = this.HourlyRate;
            newTask.SalesStage = this.SalesStage;
            newTask.FullText = this.FullText;
            newTask.InvoiceDate = this.InvoiceDate;
            newTask.DueDate = this.DueDate;
            newTask.PaymentDate = this.PaymentDate;
            //newTask.PopulateFields();

            return newTask;

        }

        public void PopulateFields() {

            //if (this.RevenueType == RevenueType.REV || this.RevenueType == RevenueType.COS) {
            //    this.ProductQuantity = this.Quantity;
            //    this.ProductUnitPrice = this.UnitPrice;
            //    this.ProductExtendedPrice = this.ExtendedPrice;
            //}
            //else {
            //    this.ServicesQuantity = this.Quantity;
            //    this.HourlyRate = this.UnitPrice;
            //    this.ServicesExtendedPrice = this.ExtendedPrice;
            //}

        }


        public void CalculateFields(Project project) {

            //if (this.RevenueType == RevenueType.COS || this.RevenueType == RevenueType.REV) {

            //    this.Quantity = this.ProductQuantity;
            //    this.UnitPrice = this.ProductUnitPrice;
            //    this.ExtendedPrice = this.ProductExtendedPrice;

            //}
            //else {

            //    this.Quantity = this.ServicesQuantity;
            //    this.UnitPrice = this.HourlyRate;
            //    this.ExtendedPrice = this.ServicesExtendedPrice;

            //}

            switch (project.PaymentTerm.CalculationBasis) {

                case CalculationBasis.NETT:
                    this.DueDate = this.InvoiceDate.AddDays(project.PaymentTerm.NumberOfDays);
                    this.PaymentDate = this.DueDate.AddDays(project.Customer.TypicalPaymentDelay ?? Constants.TypicalPaymentDelay);
                    break;

                case CalculationBasis.EOM:
                    this.DueDate = new DateTime(this.InvoiceDate.Year, this.InvoiceDate.Month, DateTime.DaysInMonth(this.InvoiceDate.Year, this.InvoiceDate.Month));
                    this.DueDate = this.DueDate.AddDays(project.PaymentTerm.NumberOfDays);
                    this.PaymentDate = this.DueDate.AddDays(project.Customer.TypicalPaymentDelay ?? Constants.TypicalPaymentDelay);
                    break;

            }

        }

    }

}