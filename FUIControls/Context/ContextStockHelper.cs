using System.Collections;
using System.Web;

namespace FUIControls.Context
{
    public static class ContextStockHelper
    {
        #region "Properties"

        public static IDictionary CurrentItems
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Items;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region "Methods"

        public static void Add(string key, object value)
        {
            Add(key, value, false);
        }

        public static void Remove(string key)
        {
            Remove(key, false);
        }

        public static bool Contains(string key)
        {
            return Contains(key, false);
        }

        public static object GetItem(string key)
        {
            return GetItem(key, false);
        }

        public static void Add(string key, object value, bool caseSensitive)
        {
            StockHelperFunctions.Add(CurrentItems, key, value, caseSensitive);
        }


        public static void Remove(string key, bool caseSensitive)
        {
            StockHelperFunctions.Remove(CurrentItems, key, caseSensitive);
        }


        public static bool Contains(string key, bool caseSensitive)
        {
            return StockHelperFunctions.Contains(CurrentItems, key, caseSensitive);
        }

        public static object GetItem(string key, bool caseSensitive)
        {
            return StockHelperFunctions.GetItem(CurrentItems, key, caseSensitive);
        }

        public static void Clear(string startsWith)
        {
            StockHelperFunctions.Clear(CurrentItems, startsWith);
        }

        #endregion
    }
}