using System.Collections;

namespace FUIControls.Context
{
    public static class RequestStockHelper
    {
        #region "Properties"

        public static IDictionary CurrentItems
        {
            get
            {
                IDictionary items = ContextStockHelper.CurrentItems;
                if (items == null)
                {
                    items = ThreadStockHelper.CurrentItems;
                }

                return items;
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
