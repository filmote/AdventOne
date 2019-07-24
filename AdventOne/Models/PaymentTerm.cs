using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdventOne.Models {

    public class PaymentTerm {

        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public int NumberOfDays {

            get {
                return int.Parse(this.Code.Substring(3, this.Code.Length - 7));
            }
            set { }

        }

        [NotMapped]
        public CalculationBasis CalculationBasis {

            get {

                if (this.Code.StartsWith("EOM")) {
                    return CalculationBasis.EOM;
                }

                return CalculationBasis.NETT;

            }
            set { }

        }

    }

}