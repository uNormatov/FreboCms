using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FUIControls.Context
{
	public static class StockHelperFunctions
	{
		public static void Add(IDictionary items, string key, object value, bool caseSensitive)
		{
			if (key == null)
			{
				return;
			}
			if (items != null)
			{
				if (!caseSensitive)
				{
					key = key.ToLower();
				}
				items[key] = value;
			}
		}

		public static void Remove(IDictionary items, string key, bool caseSensitive)
		{
			if (key == null)
			{
				return;
			}

			if (items != null)
			{
				if (!caseSensitive)
				{
					key = key.ToLower();
				}
				items[key] = null;
			}
		}

		public static bool Contains(IDictionary items, string key, bool caseSensitive)
		{
			if (key == null)
			{
				return false;
			}

			if (items != null)
			{
				if (!caseSensitive)
				{
					key = key.ToLower();
				}
				return (items[key] != null);
			}
			return false;
		}

		public static object GetItem(IDictionary items, string key, bool caseSensitive)
		{
			if (key == null)
			{
				return null;
			}

			if (items != null)
			{
				if (!caseSensitive)
				{
					key = key.ToLower();
				}
				return items[key];
			}
			return null;
		}

		public static void Clear(IDictionary items, string startsWith)
		{
			if (startsWith == null)
			{
				return;
			}
			if (items != null)
			{
				startsWith = startsWith.ToLower();

				List<string> keyList = new List<string>();
				IDictionaryEnumerator ItemEnum = items.GetEnumerator();
				while (ItemEnum.MoveNext())
				{
					string itemKey = ItemEnum.Key.ToString();
					string key = itemKey.ToLower();
					if (key.StartsWith(startsWith))
					{
						keyList.Add(itemKey);
					}
				}
				foreach (string key in keyList)
				{
					Remove(items, key, false);
				}
			}
		}
	}
}
