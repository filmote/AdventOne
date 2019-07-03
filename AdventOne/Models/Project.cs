using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventOne.Models {

    public class Project {

        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string ProjectName { get; set; }
        public int? EmployeeID { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }

    }

}