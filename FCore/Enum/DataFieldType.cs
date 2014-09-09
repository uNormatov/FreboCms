using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCore.Enum
{
    /// <summary>
    /// Field data types.
    /// </summary>
    public enum DataFieldType
    {
        /// <summary>
        /// Varchar.
        /// </summary>
        Varchar = 1,

        /// <summary>
        /// Text.
        /// </summary>
        Text = 2,

        /// <summary>
        /// Char.
        /// </summary>
        Char = 3,

        /// <summary>
        /// Integer.
        /// </summary>
        Integer = 4,

        /// <summary>
        /// Double.
        /// </summary>
        Decimal = 5,

        /// <summary>
        /// Date and Time.
        /// </summary>
        DateTime = 6,

        /// <summary>
        /// Boolean.
        /// </summary>
        Boolean = 7,

        /// <summary>
        /// File.
        /// </summary>
        File = 8,

        /// <summary>
        /// GUID.
        /// </summary>
        GUID = 9,

        /// <summary>
        /// Numeric.
        /// </summary>
        Numeric = 10,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 10

    }


    /// <summary>
    /// Field data types - string constants.
    /// </summary>
    public class DataFieldTypeCode
    {
        /// <summary>
        /// Text.
        /// </summary>
        public const string VARCHAR = "nvarchar";

        /// <summary>
        /// Long text.
        /// </summary>
        public const string TEXT = "ntext";

        /// <summary>
        /// Char.
        /// </summary>
        public const string CHAR = "char";

        /// <summary>
        /// Integer.
        /// </summary>
        public const string INTEGER = "int";

        /// <summary>
        /// Double.
        /// </summary>
        public const string DECIMAL = "decimal";

        /// <summary>
        /// Date and Time.
        /// </summary>
        public const string DATETIME = "datetime";

        /// <summary>
        /// Boolean.
        /// </summary>
        public const string BOOLEAN = "bit";

        /// <summary>
        /// File.
        /// </summary>
        public const string FILE = "file";

        /// <summary>
        /// GUID.
        /// </summary>
        public const string GUID = "guid";

        /// <summary>
        /// Numeric.
        /// </summary>
        public const string NUMERIC = "numeric";

        /// <summary>
        /// Unknown.
        /// </summary>
        public const string UNKNOWN = "unknown";
    }
}
