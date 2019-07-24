using System.Collections.Generic;

namespace AdventOne.Models {

    public class Permission {

        public int ID { get; set; }
        public string PermissionName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}