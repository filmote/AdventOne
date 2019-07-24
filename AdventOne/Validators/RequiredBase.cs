﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AdventOne.Validators {

    public abstract class RequiredBase : ValidationAttribute {

        protected static bool IsNumericType(object o) {

            switch (Type.GetTypeCode(o.GetType())) {

                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                default:
                    return false;

            }

        }

    }

}