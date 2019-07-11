using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventOne.Models {

    public class Permission {

        public int ID { get; set; }
        public string PermissionName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}