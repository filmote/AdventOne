using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AdventOne.Models {

    public static class Constants {

        public const int PageSize = 3;

    }

    public class EnumHelpers {

        public static string GetEnumDisplayName<T>(T value) where T : struct {

            MemberInfo[] memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length != 1) return null;

            DisplayAttribute[] displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)  return value.ToString();

            return displayAttribute[0].Name;
        }
        
    }

    public enum PaymentTerms {
        [Display(Name = "Net 14 Days")]
        NET14DAYS,
        [Display(Name = "Net 30 Days")]
        NET30DAYS,
        [Display(Name = "Net 60 Days")]
        NET60DAYS,
        [Display(Name = "End of Month 14 Days")]
        EOM14DAYS,
        [Display(Name = "End of Month 15 Days")]
        EOM15DAYS,
        [Display(Name = "End of Month 30 Days")]
        EOM30DAYS,
        [Display(Name = "End of Month 60 Days")]
        EOM60DAYS,
    }

    public enum ProjectStatus {
        Canceled,
        Open
    }

    public enum Division {
        [Display(Name = "Consulting")]
        CON,
        [Display(Name = "Cyber")]
        CYB,
        [Display(Name = "ERP")]
        ERP,
        [Display(Name = "General")]
        GEN,
        [Display(Name = "GRC")]
        GRC,
        [Display(Name = "Infrastructure")]
        INF,
        [Display(Name = "Managed Services")]
        MSV
    }

    public enum Location {
        [Display(Name = "Australian Capital Territory")]
        ACT,
        [Display(Name = "General")]
        GEN,
        [Display(Name = "New South Wales")]
        NSW,
        [Display(Name = "Queensland")]
        QLD,
        [Display(Name = "South Australia")]
        SA,
        [Display(Name = "Tasmania")]
        TAS,
        [Display(Name = "Victoria")]
        VIC,
        [Display(Name = "Western Australia")]
        WA
    }

    public enum Branch {
        [Display(Name = "Advent One")]
        A1,
        [Display(Name = "Advent One Managed Services")]
        A1MS,
        [Display(Name = "Alata")]
        ALATA,
        [Display(Name = "Main")]
        MAIN,
        [Display(Name = "PLii Pty Ltd")]
        PLII
    }

    public enum InvoiceStatus {
        Open,
        Paid
    }

    public enum SalesStage {
        Lost,
        [Display(Name = "Pipe Dream")]
        PipeDream,
        [Display(Name = "Looking Good")]
        LookingGood,
        Sold
    }

    public enum RevenueType {
        COS = 0,
        REV,
        SVC
    }

}