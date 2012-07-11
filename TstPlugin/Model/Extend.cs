using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using WRPlugIn;

namespace PxP
{
    public class ConfFile { public string Name { get; set; } }
    
  

    // 三角形, 倒三角形, 正方形, 圓形, 十字, 叉叉, 星號
    //public enum Shape { Triangle, Ellipse, Square, Cone, Cross, LineDiagonalCross, Star };
    public enum Shape {
        [DescriptionAttribute("▲"), EnumDescription("▲")]
        Triangle,
        [DescriptionAttribute("▼"), EnumDescription("▼")]
        Ellipse,
        [DescriptionAttribute("■"), EnumDescription("■")]
        Square,
        [DescriptionAttribute("●"), EnumDescription("●")]
        Cone,
        [DescriptionAttribute("+"), EnumDescription("+")]
        Cross,
        [DescriptionAttribute("╳"), EnumDescription("╳")]
        LineDiagonalCross,
        [DescriptionAttribute("★"), EnumDescription("★")]
        Star 
    };
    public class ImageInfo :IImageInfo
    {

        #region IImageInfo 成員

        public System.Drawing.Bitmap Image { set; get; }

        public int Station { set; get; }
        public ImageInfo(System.Drawing.Bitmap image, int station)
        {
            this.Image = image;
            this.Station = station;
        }

        #endregion
    }

    #region 增加列舉型別ENUM其他功能
    static class ExtensionMethods
    {
        public static string ToGraphic(this Enum en) //ext method
        {

            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(
                                              typeof(DescriptionAttribute),
                                              false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }
    }
    /// <summary>
    /// Provides a description for an enumerated type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field,
     AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        /// <value>The description stored in the attribute.</value>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnumDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The description to store in this attribute.
        /// </param>
        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }
    /// <summary>
    /// Provides a static utility object of methods and properties to interact
    /// with enumerated types.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Gets the <see cref="DescriptionAttribute" /> of an <see cref="Enum" />
        /// type value.
        /// </summary>
        /// <param name="value">The <see cref="Enum" /> type value.</param>
        /// <returns>A string containing the text of the
        /// <see cref="DescriptionAttribute"/>.</returns>
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            EnumDescriptionAttribute[] attributes =
               (EnumDescriptionAttribute[])
             fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        /// <summary>
        /// Converts the <see cref="Enum" /> type to an <see cref="IList" /> 
        /// compatible object.
        /// </summary>
        /// <param name="type">The <see cref="Enum"/> type.</param>
        /// <returns>An <see cref="IList"/> containing the enumerated
        /// type value and description.</returns>
        public static IList ToList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ArrayList list = new ArrayList();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
            }

            return list;
        }
        public static string GetItemString(string Description)
        {
            string result = "";
            switch (Description)
            {
                case "▲":
                    result = "Triangle";
                    break;
                case "▼":
                    result = "Ellipse";
                    break;
                case "■":
                    result = "Square";
                    break;
                case "●":
                    result = "Cone";
                    break;
                case "+":
                    result = "Cross";
                    break;
                case "╳":
                    result = "LineDiagonalCross";
                    break;
                case "★":
                    result = "Star";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
    #endregion
}
