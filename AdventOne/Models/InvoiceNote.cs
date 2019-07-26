using System;

namespace AdventOne.Models {

    public class InvoiceNote {

        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public int EmployeeID { get; set; }

        public String Notes { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime OriginalExpectedPaymentDate { get; set; }
        public DateTime RevisedExpectedPaymentDate { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Employee Employee { get; set; }

    }

}