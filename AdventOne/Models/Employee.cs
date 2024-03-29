﻿using System.Collections.Generic;

namespace AdventOne.Models {

    public class Employee {

        public int ID { get; set; }
        public string EmployeeName { get; set; }
        public string EmailAddress { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }

    }

}