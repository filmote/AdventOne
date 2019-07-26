using System.ComponentModel.DataAnnotations;

namespace AdventOne.Models {

    public class BaseEntity {

        //protected string GetEnumDisplayName<T>(T value) where T : struct {
        //    // Get the MemberInfo object for supplied enum value
        //    var memberInfo = value.GetType().GetMember(value.ToString());
        //    if (memberInfo.Length != 1)
        //        return null;

        //    // Get DisplayAttibute on the supplied enum value
        //    var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false)
        //                           as DisplayAttribute[];
        //    if (displayAttribute == null || displayAttribute.Length != 1)
        //        return value.ToString();

        //    return displayAttribute[0].Name;
        //}

    }

}