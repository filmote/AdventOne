using System.Collections.Generic;

namespace AdventOne.Models {

    public class Customer {

        public int ID { get; set; }
        public int? EmployeeID { get; set; }
        public string CustomerName { get; set; }
        public PaymentTerms? PaymentTerms { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<Project> Projects { get; set; } 

    }

}