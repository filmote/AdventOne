﻿namespace AdventOne.Models {

    public class WorkOrder {

        public int ID { get; set; }
        public string Description { get; set; }
        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }

    }

}