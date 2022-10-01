using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApp
{
    /// <summary>
    /// Provides utility methods for converting string values to other data types.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Removes dashes ("-") from the given object value represented as a string and returns an empty string ("")
        ///     when the instance type could not be represented as a string.
        ///     <para>
        ///         Note: This will return the type name of given isntance if the runtime type of the given isntance is not a
        ///         string!
        ///     </para>
        /// </summary>
        /// <param name="value">The object instance to undash when represented as its string value.</param>
        /// <returns></returns>
        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        /// <summary>
        ///     Removes dashes ("-") from the given string value.
        /// </summary>
        /// <param name="value">The string value that optionally contains dashes.</param>
        /// <returns></returns>
        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }
    }



    public static class ObjectExtensions
    {
    public static string ChineseMoney(this decimal x) {
      if (x < 0)
      {
        x = Math.Abs(x);
      }
      var s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
      s = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])
            |(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
      return Regex.Replace(s, ".", delegate (Match m)
      {
        return "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString();
      });

      }
        public static bool IsTrue(this object x) {
            var val = x.ToString().ToLower();
            var result = false;
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            else if (bool.TryParse(val, out result)) {
                return result;
            }
            else {
                switch (val) {
                    case "y":
                        return true;
                    case "n":
                        return false;
                    case "yes":
                        return true;
                    case "no":
                        return false;
                    case "1":
                        return true;
                    case "0":
                        return false;
                    case "true":
                        return true;
                    case "t":
                        return true;
                    case "false":
                        return false;
                    case "f":
                        return false;
                    default:
                        return false;
                }
            }
        }

        public static bool IsNumeric(this object x) { return (x == null ? false : IsNumeric(x.GetType())); }

        // Method where you know the type of the object
        public static bool IsNumeric(Type type) { return IsNumeric(type, Type.GetTypeCode(type)); }

        // Method where you know the type and the type code of the object
        public static bool IsNumeric(Type type, TypeCode typeCode) { return (typeCode == TypeCode.Decimal || (type.IsPrimitive && typeCode != TypeCode.Object && typeCode != TypeCode.Boolean && typeCode != TypeCode.Char)); }
    }
}