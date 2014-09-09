using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using System.Web;

namespace FCore.Helper
{
    public static class ValidationHelper
    {
        #region "Variables"

        private static Hashtable _doubleExps = new Hashtable();

        /// <summary>
        /// Regular expression to match the integer
        /// </summary>
        private static Regex _intRegExp = null;


        /// <summary>
        /// Regular expression to match the filename
        /// </summary>
        private static Regex _filenameRegExp = null;


        /// <summary>
        /// Regular expression to match the email
        /// </summary>
        private static Regex _emailRegExp = null;


        /// <summary>
        /// Regular expression to match the URL expression
        /// </summary>
        private static Regex _URLRegExp = null;

        #endregion


        #region "Properties"

        /// <summary>
        /// Integer regular expression
        /// </summary>
        public static Regex IntRegExp
        {
            get
            {
                if (_intRegExp == null)
                {
                    _intRegExp = RegexHelper.GetRegex("^(?:\\+|-)?1?\\d{1,9}$");
                }
                return _intRegExp;
            }
        }

        /// <summary>
        /// Filename regular expression
        /// </summary>
        public static Regex FilenameRegExp
        {
            get
            {
                if (_filenameRegExp == null)
                {
                    _filenameRegExp = RegexHelper.GetRegex("^[A-Za-z0-9\\._-]+$");
                }
                return _filenameRegExp;
            }
        }


        /// <summary>
        /// Email regular expression
        /// </summary>
        public static Regex EmailRegExp
        {
            get
            {
                if (_emailRegExp == null)
                {
                    _emailRegExp = RegexHelper.GetRegex(@"^[A-Za-z0-9_\-\+]+(?:\.[A-Za-z0-9_\-\+]+)*@[A-Za-z0-9_-]+(?:\.[A-Za-z0-9_-]+)+$");
                }
                return _emailRegExp;
            }
        }

        /// <summary>
        /// URL regular expression
        /// </summary>
        public static Regex URLRegExp
        {
            get
            {
                if (_URLRegExp == null)
                {
                    // Expression groups: none
                    _URLRegExp = RegexHelper.GetRegex("^(?:(?#Protocol)(?:(?:ht|f)tp(?:s?)\\:\\/\\/|(?=www\\.|\\/|~\\/|\\.\\.))(?#Username:Password)(?:\\w+:\\w+@)?(?#Subdomains)(?:(?:[-\\w]+\\.)*(?#TopLevel Domains)(?:\\w+))(?#Port)(?::[\\d]{1,5})?|~|\\.\\.)?(?#Directories)(?:(?:(?:\\/(?:[-\\w~!$+|.,=]|%[a-fA-F0-9]{2})+)+|\\/)+|\\?|#)?(?#Query)(?:(?:\\?(?:[-\\w~!$+|.,*:]|%[a-fA-F0-9]{2})+=(?:[-\\w~!$+|.,*:=;]|%[a-fA-F0-9]{2})*)(?:&(?:[-\\w~!$+|.,*:]|%[a-fA-F0-9]{2})+=(?:[-\\w~!$+|.,*:=]|%[a-fA-F0-9]{2})*)*)*(?#Anchor)(?:#(?:[-\\w~!$+|.,*:=]|%[a-fA-F0-9]{2})*)?$", RegexHelper.DefaultOptions | RegexOptions.IgnoreCase);
                }
                return _URLRegExp;
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Gets the culture info
        /// </summary>
        /// <param name="culture">Culture to get</param>
        private static CultureInfo GetCultureInfo(ref string culture)
        {
            if (culture == null)
            {
                culture = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
                return Thread.CurrentThread.CurrentUICulture;
            }
            else
            {
                return new CultureInfo(culture);
            }
        }


        /// <summary>
        /// Returns the regular expression for double validation with current UI culture
        /// </summary>
        /// <param name="culture">Culture</param>
        /// <param name="cultureInfo">Returning culture info used for conversion</param>
        private static Regex GetDoubleExp(string culture, out CultureInfo cultureInfo)
        {
            // Get the culture info
            cultureInfo = GetCultureInfo(ref culture);

            Regex expr = (Regex)_doubleExps[culture];
            if (expr == null)
            {
                expr = RegexHelper.GetRegex("^(?:\\+|-)?(?:\\d*\\" + cultureInfo.NumberFormat.NumberDecimalSeparator + "?\\d+)(?:(?:e|E)(?:\\+|-)?\\d+)?$");
                _doubleExps[culture] = expr;
            }

            return expr;
        }

        #endregion


        #region "Validation methods"

        /// <summary>
        /// Returns true if the object representation matches the Boolean type
        /// </summary>
        /// <param name="value">Value to check</param>
        public static bool IsBoolean(object value)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else if (value is bool)
            {
                return true;
            }
            else
            {
                switch (value.ToString().ToLower())
                {
                    case "0":
                    case "1":
                    case "true":
                    case "false":
                        return true;

                    default:
                        return false;
                }
            }
        }


        /// <summary>
        /// Returns true if the object representation matches the Integer type
        /// </summary>
        /// <param name="value">Value to check</param>
        public static bool IsInteger(object value)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else if (value is int)
            {
                return true;
            }
            else
            {
                return IntRegExp.Match(value.ToString()).Success;
            }
        }


        /// <summary>
        /// Returns true if the object representation matches the positive number
        /// </summary>
        /// <param name="value">Value to check</param>
        public static bool IsPositiveNumber(object value)
        {
            return IsPositiveNumber(value, null);
        }


        /// <summary>
        /// Returns true if the object representation matches the positive number
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="culture">Culture to check</param>
        public static bool IsPositiveNumber(object value, string culture)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else
            {
                string stringValue = value.ToString();

                // Minus sign
                if (stringValue.StartsWith("-"))
                {
                    return false;
                }

                // Integer or double number
                return (IsInteger(value) || IsDouble(value));
            }
        }


        /// <summary>
        /// Returns true if the object representation matches the Double type
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="culture">Culture code</param>
        public static bool IsDouble(object value, string culture)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else if (value is double)
            {
                return true;
            }
            else
            {
                // Get the expression and culture info
                CultureInfo ci = null;
                Regex regExp = GetDoubleExp(culture, out ci);

                return regExp.IsMatch(Convert.ToString(value, ci));
            }
        }


        /// <summary>
        /// Returns true if the object representation matches the Double type
        /// </summary>
        /// <param name="value">Value to check</param>
        public static bool IsDouble(object value)
        {
            return IsDouble(value, null);
        }


        /// <summary>
        /// Returns true if the object representation matches given regular expression.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="regExp">Regular expression</param>
        public static bool IsRegularExp(object value, string regExp)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else
            {
                // Ensure proper form of regular expression
                if (!regExp.StartsWith("^"))
                {
                    regExp = "^" + regExp;
                }
                if (!regExp.EndsWith("$"))
                {
                    regExp += "$";
                }

                // Create new regular expression
                Regex regularExp = RegexHelper.GetRegex(regExp, RegexOptions.None);

                return regularExp.IsMatch(value.ToString());
            }
        }


        /// <summary>
        /// Returns true if the object representation matches the file name
        /// </summary>
        /// <param name="value">Value to check</param>
        public static bool IsFileName(object value)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else
            {
                return FilenameRegExp.IsMatch(value.ToString());
            }
        }


        /// <summary>
        /// Returns true if the object representation matches the Email
        /// </summary>
        /// <param name="value">Value to check</param>
        public static bool IsEmail(object value)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else
            {
                return EmailRegExp.IsMatch(value.ToString());
            }
        }


        /// <summary>
        /// Returns true if the object representation matches the Email list (email adresses separated by semicolon)
        /// </summary>
        /// <param name="value">Value to check</param>
        public static bool AreEmails(object value)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else
            {
                // Validate the email list
                string[] emails = value.ToString().Split(';');
                foreach (string email in emails)
                {
                    if (!IsEmail(email))
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        /// <summary>
        /// Returns true if value is Guid
        /// </summary>
        /// <param name="value">Value to check</param>
        public static bool IsGuid(object value)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return false;
            }
            else
            {
                if (ValidationHelper.GetGuid(value, Guid.Empty) != Guid.Empty)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion


        #region "Conversion methods"

        /// <summary>
        /// Returns the boolean representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        public static bool GetBoolean(object value, bool defaultValue)
        {
            if ((value == null) || (value == DBNull.Value))
            {
                return defaultValue;
            }
            else if (value is bool)
            {
                return (bool)value;
            }
            else
            {
                if (value is string)
                {
                    switch (((string)value).ToLower())
                    {
                        case "true":
                        case "1":
                            return true;

                        case "false":
                        case "0":
                            return false;

                        default:
                            return defaultValue;
                    }
                }

                return Convert.ToBoolean(value);
            }
        }


        /// <summary>
        /// Returns the integer representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        public static int GetInteger(object value, int defaultValue)
        {
            if (!IsInteger(value))
            {
                return defaultValue;
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }


        /// <summary>
        /// Returns the integer representation of an object or default value if not.
        /// Consumes all exceptions.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        public static int GetSafeInteger(object value, int defaultValue)
        {
            try
            {
                return GetInteger(value, defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// Returns the double representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="culture">Culture code</param>
        public static double GetDouble(object value, double defaultValue, string culture)
        {
            if (!IsDouble(value, culture))
            {
                return defaultValue;
            }
            else
            {
                if (culture != null)
                {
                    // Get with specific culture
                    CultureInfo ci = new CultureInfo(culture);
                    return Convert.ToDouble(value, ci);
                }
                else
                {
                    // Get with default culture
                    return Convert.ToDouble(value);
                }
            }
        }


        /// <summary>
        /// Returns the double representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        public static double GetDouble(object value, double defaultValue)
        {
            return GetDouble(value, defaultValue, null);
        }


        /// <summary>
        /// Returns the string representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        public static string GetString(object value, string defaultValue)
        {
            return GetString(value, defaultValue, null);
        }


        /// <summary>
        /// Returns the string representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="culture">Culture</param>
        public static string GetString(object value, string defaultValue, string culture)
        {
            if (value is string)
            {
                return (string)value;
            }
            else if ((value == DBNull.Value) || (value == null))
            {
                return defaultValue;
            }
            else
            {
                if (culture != null)
                {
                    // Get with specific culture
                    CultureInfo ci = GetCultureInfo(ref culture);
                    return Convert.ToString(value, ci);
                }
                else
                {
                    // Get with default culture
                    return Convert.ToString(value);
                }
            }
        }


        /// <summary>
        /// Returns the string representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="culture">Culture</param>
        /// <param name="format">Formatting string</param>
        public static string GetString(object value, string defaultValue, string culture, string format)
        {
            if (format != null)
            {
                // Load default value
                if ((value == null) || (value == DBNull.Value))
                {
                    value = defaultValue;
                }

                // Get by specified format
                CultureInfo ci = GetCultureInfo(ref culture);
                return String.Format(ci, format, value);
            }
            else
            {
                // Get as standard expression
                return GetString(value, defaultValue, culture);
            }
        }


        /// <summary>
        /// Returns the GUID representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="defaultValue">Default value</param>
        public static Guid GetGuid(object value, Guid defaultValue)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return defaultValue;
            }
            else if (value is Guid)
            {
                return (Guid)value;
            }
            else
            {
                try
                {
                    return new Guid(value.ToString());
                }
                catch
                {
                    return defaultValue;
                }
            }
        }


        /// <summary>
        /// Returns the byte[] representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="defaultValue">Default value.</param>
        public static byte[] GetBinary(object value, byte[] defaultValue)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return defaultValue;
            }

            try
            {
                return (byte[])value;
            }
            catch
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// Returns the DateTime representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="culture">Culture to use for processing of the string</param>
        public static DateTime GetDateTime(object value, DateTime defaultValue, string culture)
        {
            if (culture != null)
            {
                // Get with specific culture
                CultureInfo ci = new CultureInfo(culture);
                return GetDateTime(value, defaultValue, ci);
            }
            else
            {
                // Get with default culture
                return GetDateTime(value, defaultValue);
            }
        }


        /// <summary>
        /// Returns the DateTime representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="defaultValue">Default value.</param>
        public static DateTime GetDateTime(object value, DateTime defaultValue)
        {
            return GetDateTime(value, defaultValue, (IFormatProvider)null);
        }


        /// <summary>
        /// Returns the DateTime representation of an object or default value if not
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="format">Format provider</param>
        public static DateTime GetDateTime(object value, DateTime defaultValue, IFormatProvider format)
        {
            if ((value == DBNull.Value) || (value == null))
            {
                return defaultValue;
            }
            else if (value is DateTime)
            {
                return (DateTime)value;
            }
            else
            {
                try
                {
                    return DateTime.Parse(value.ToString(), format);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }


        /// <summary>
        /// Converts the value to specified type
        /// </summary>
        /// <typeparam name="ReturnType">Result type</typeparam>
        /// <param name="value">Value to convert</param>
        public static ReturnType GetValue<ReturnType>(object value)
        {
            if ((value == null) || (value == DBNull.Value))
            {
                return default(ReturnType);
            }

            if (typeof(ReturnType) == typeof(string))
            {
                value = ValidationHelper.GetString(value, "");
            }
            else if (typeof(ReturnType) == typeof(bool))
            {
                value = ValidationHelper.GetBoolean(value, false);
            }
            else if (typeof(ReturnType) == typeof(int))
            {
                value = ValidationHelper.GetInteger(value, 0);
            }
            else if (typeof(ReturnType) == typeof(double))
            {
                value = ValidationHelper.GetDouble(value, 0);
            }

            return (ReturnType)value;
        }


        /// <summary>
        /// Gets the code name created from the given string
        /// </summary>
        /// <param name="name">Display name</param>
        public static string GetCodeName(object name)
        {
            return GetCodeName(name, null);
        }


        /// <summary>
        /// Gets the code name created from the given string
        /// </summary>
        /// <param name="name">Display name</param>
        /// <param name="replacement">Replacement string for invalid characters</param>
        public static string GetCodeName(object name, string replacement)
        {
            if (replacement == null)
            {
                replacement = "_";
            }
            string stringName = ValidationHelper.GetString(name, "");
            stringName = Regex.Replace(stringName, "[^a-zA-Z0-9_.]+", replacement);
            return stringName;
        }


        /// <summary>
        /// Gets the language created from the given string
        /// </summary>
        /// <param name="lang">Language code</param>
        /// <param name="replacement">Replacement string for invalid characters</param>
        public static string GetLanguage(object lang, string replacement)
        {
            if (replacement == null)
            {
                replacement = "_";
            }
            string stringName = ValidationHelper.GetString(lang, "");
            stringName = Regex.Replace(stringName, "[^a-zA-Z\\-]+", replacement);
            return stringName;
        }


        /// <summary>
        /// Gets the code name created from the given string
        /// </summary>
        /// <param name="name">Display name</param>
        public static string GetIdentificator(object name)
        {
            return GetIdentificator(name, null);
        }


        /// <summary>
        /// Gets the code name created from the given string
        /// </summary>
        /// <param name="name">Display name</param>
        /// <param name="replacement">Replacement string for invalid characters</param>
        public static string GetIdentificator(object name, string replacement)
        {
            if (replacement == null)
            {
                replacement = "_";
            }
            string stringName = ValidationHelper.GetString(name, "");
            stringName = Regex.Replace(stringName, "[^a-zA-Z0-9_]", replacement);
            return stringName;
        }


        /// <summary>
        /// Gets the code name created from the given display name
        /// </summary>
        /// <param name="name">Display name</param>
        /// <param name="prefix">Prefix of the display name</param>
        /// <param name="suffix">Suffix of the display name</param>
        public static string GetCodeName(string name, string prefix, string suffix)
        {
            return GetCodeName(prefix + name + suffix);
        }

        #endregion
    }
}
