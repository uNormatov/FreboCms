using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FCore.Helper
{
    public static class RegexHelper
    {
        #region "Variables"

        /// <summary>
        /// Default regular expression options
        /// </summary>
        private static RegexOptions _defaultOptions = RegexOptions.Compiled;

        #endregion

        #region "Properties"

        /// <summary>
        /// Default regular expression options
        /// </summary>
        public static RegexOptions DefaultOptions
        {
            get
            {
                return _defaultOptions;
            }
            set
            {
                _defaultOptions = value;
            }
        }

        #endregion

        #region "Methods"

        public static Regex GetRegex(string pattern)
        {
            return GetRegex(pattern, DefaultOptions);
        }

        public static Regex GetRegex(string pattern, RegexOptions options)
        {
            Regex result = new Regex(pattern, options);

            return result;
        }

        #endregion
    }
}
