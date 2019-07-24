using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdventOne.Models {

    public class Invoice {

        public int ID { get; set; }
        public int ProjectID { get; set; }

        [EnumDataType(typeof(InvoiceStatus))]
        public InvoiceStatus Status { get; set; }

        public String InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ExpectedPaymentDate { get; set; }
        public int DaysOverdue { get; set; }


        [Range(0, 10000000000)]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<InvoiceNote> InvoiceNotes { get; set; }

    }

}