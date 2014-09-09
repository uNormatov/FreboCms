using System.Collections;
using System.Threading;
using FCore.Collection;

namespace FUIControls.Context
{
    public static class ThreadStockHelper
    {
        #region "Classes"

        private class ThreadItemsDictionary : GoodDictionary<string, object>
        {
        }

        private class ThreadsDictionary : GoodDictionary<int, ThreadItemsDictionary>
        {
        }

        #endregion

        #region "Variables"

        private static ThreadsDictionary _allthreadsitems = new ThreadsDictionary();

        #endregion

        #region "Properties"

        private static ThreadsDictionary AllThreadsItems
        {
            get
            {
                if (_allthreadsitems == null)
                {
                    _allthreadsitems = new ThreadsDictionary();
                }
                return _allthreadsitems;
            }
        }

        public static IDictionary CurrentItems
        {
            get { return EnsureItems(Thread.CurrentThread.ManagedThreadId); }
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

        private static ThreadItemsDictionary EnsureItems(int threadId)
        {
            ThreadItemsDictionary items = AllThreadsItems[threadId];
            if (items == null)
            {
                items = new ThreadItemsDictionary();
                AllThreadsItems[threadId] = items;
            }

            return items;
        }

        public static void DeleteThreadItems(int threadId)
        {
            if (AllThreadsItems.Contains(threadId))
            {
                AllThreadsItems.Remove(threadId);
            }
        }

        public static void CopyCurrentItems(int destThreadId)
        {
            ThreadItemsDictionary items = EnsureItems(destThreadId);
            IDictionary sourceItems = RequestStockHelper.CurrentItems;
            if (sourceItems == null)
            {
                sourceItems = CurrentItems;
            }

            if (sourceItems != null)
            {
                foreach (DictionaryEntry item in sourceItems)
                {
                    items[item.Key] = item.Value;
                }
            }
        }

        #endregion
    }
}
