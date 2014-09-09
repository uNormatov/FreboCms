using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;

namespace FUIControls.Helper
{
    public class LanguageHelper
    {
        private static readonly Object LockObject = new Object();

        private static LanguageHelper _instance;
        public static LanguageHelper Instance
        {
            get { return _instance ?? (_instance = new LanguageHelper()); }
        }

        private readonly List<string> _availableLanguages;
        private readonly GoodDictionary<string, GoodDictionary<string, string>> _translations;

        private readonly LocalizationProvider _localizationProvider;
        private bool _isCleared = false;

        public LanguageHelper()
        {
            _localizationProvider = new LocalizationProvider();
            _availableLanguages = new List<string>();
            _translations = new GoodDictionary<string, GoodDictionary<string, string>>();

            FillList();
        }

        public bool IsAvailableLanguage(string lang)
        {
            lock (LockObject)
            {
                if (_isCleared)
                    FillList();
                return _availableLanguages.Contains(lang);
            }
        }

        public string GetTranslate(string language, string keyword)
        {
            lock (LockObject)
            {
                if (_isCleared)
                    FillList();

                if (_translations.ContainsKey(language))
                {
                    if (_translations[language].ContainsKey(keyword))
                        return _translations[language][keyword];
                }
                return keyword;
            }
        }


        public string GetTranslateByPattern(string language, string keyword)
        {
            Regex regex = RegexHelper.GetRegex("\\{\\w+([A-Za-z]|_[A-Za-z])\\w*\\}");
            if (regex.IsMatch(keyword))
            {
                MatchCollection matchCollection = regex.Matches(keyword);
                foreach (Match match in matchCollection)
                {
                    keyword = keyword.Replace(match.Value, GetTranslate(language, match.Value.Replace("{", "").Replace("}", "")));
                }
            }
            return keyword;
        }

        public List<string> GetMetaColumnNames(string text)
        {
            List<string> result = new List<string>();
            Regex regex = RegexHelper.GetRegex("\\%\\w+([A-Za-z]|_[A-Za-z])\\w*\\%");
            if (regex.IsMatch(text))
            {
                MatchCollection matchCollection = regex.Matches(text);
                foreach (Match match in matchCollection)
                {
                    result.Add(match.Value.Replace("%", "").Replace("%", ""));
                }
            }
            return result;
        }

        public string EvaluateMetaData(string text, Dictionary<string, string> values)
        {
            Regex regex = RegexHelper.GetRegex("\\%\\w+([A-Za-z]|_[A-Za-z])\\w*\\%");
            if (regex.IsMatch(text))
            {
                MatchCollection matchCollection = regex.Matches(text);
                foreach (Match match in matchCollection)
                {
                    string columnName = match.Value.Replace("%", "").Replace("%", "");
                    text = text.Replace(match.Value, values[columnName]);
                }
            }
            return text;
        }

        private void FillList()
        {
            lock (LockObject)
            {
                _isCleared = false;
                List<LanguageInfo> languageInfos = _localizationProvider.SelectAll(new ErrorInfoList());
                DataTable dataTable = _localizationProvider.SelecAllTranslations(new ErrorInfoList());
                if (languageInfos != null && languageInfos.Count > 0)
                {
                    foreach (LanguageInfo info in languageInfos)
                    {
                        _availableLanguages.Add(info.Code);
                        FillTranslations(info.Code, dataTable);
                    }
                }
            }
        }

        private void FillTranslations(string language, DataTable dataTable)
        {
            GoodDictionary<string, string> translations = new GoodDictionary<string, string>();
            string keyword = string.Empty;
            string defaultValue = string.Empty;
            string value = string.Empty;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    keyword = ValidationHelper.GetString(dataTable.Rows[i]["Keyword"], string.Empty);
                    defaultValue = ValidationHelper.GetString(dataTable.Rows[i]["DefaultValue"], string.Empty);
                    value = ValidationHelper.GetString(dataTable.Rows[i][language], string.Empty);
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        if (string.IsNullOrEmpty(value))
                            value = defaultValue;
                        translations.Add(keyword, value);
                    }
                }

                _translations.Add(language, translations);
            }
        }

        public void Clear()
        {
            lock (LockObject)
            {
                _isCleared = true;
                _availableLanguages.Clear();
                _translations.Clear();
            }
        }


    }
}
