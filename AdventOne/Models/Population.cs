using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventOne.Models {

    public class Population {

        public int ID { get; set; }
        public float Male { get; set; }
        public float Female { get; set; }
        public float Others { get; set; }
        public string Country { get; set; }
        public DateTime MonthAndYear { get; set; }

    }
}