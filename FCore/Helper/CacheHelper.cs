using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.Xml.Linq;
using FCore.Class;
using FCore.Collection;
using FCore.Constant;

namespace FCore.Helper
{
    public static class CacheHelper
    {
        private static readonly PairGoodCollection<PageInfo> PairCollection = new PairGoodCollection<PageInfo>();
        private static readonly PairGoodCollection<ContentTypeInfo> ContentTypeCollection = new PairGoodCollection<ContentTypeInfo>();
        private static readonly PairGoodCollection<UserProfileInfo> UserProfileCollection = new PairGoodCollection<UserProfileInfo>();
        private static readonly Object LockObject = new Object();

        private static int _duration = 86400;
        public static int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public static bool Contains(string key)
        {
            lock (LockObject)
                return (HttpContext.Current.Cache[key] != null);
        }

        public static void Add(string key, object value)
        {
            lock (LockObject)
            {

                if (Contains(key))
                    HttpContext.Current.Cache[key] = value;
                else
                    HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddMinutes((double)Duration),
                                                      TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
            }
        }

        public static void Add(string key, object value, int duration)
        {
            lock (LockObject)
            {
                if (Contains(key))
                    HttpContext.Current.Cache[key] = value;
                else
                    HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddMinutes((double)duration), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
            }
        }

        public static void Delete(string key)
        {
            lock (LockObject)
            {
                if (Contains(key))
                    HttpContext.Current.Cache.Remove(key);
            }
        }

        public static void DeleteAll(string key)
        {
            lock (LockObject)
            {
                IDictionaryEnumerator keys = HttpContext.Current.Cache.GetEnumerator();
                string currentKey = string.Empty;

                while (keys.MoveNext())
                {
                    if (keys.Key.ToString().ToLower().StartsWith(key.ToLower()))
                    {
                        currentKey = keys.Key.ToString();
                        HttpContext.Current.Cache.Remove(currentKey);
                    }
                }
            }
        }

        public static void DeleteAll()
        {
            lock (LockObject)
            {
                IDictionaryEnumerator keys = HttpContext.Current.Cache.GetEnumerator();
                string currentKey = string.Empty;

                while (keys.MoveNext())
                {
                    currentKey = keys.Key.ToString();
                    HttpContext.Current.Cache.Remove(currentKey);
                }
            }
        }

        public static object Get(string key)
        {
            lock (LockObject)
            {
                if (Contains(key))
                    return HttpContext.Current.Cache[key];
                return null;
            }
        }

        public static void AddWebPartToCache()
        {
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(60));
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Private);
        }

        public static void AddPageToCache(PageInfo info)
        {
            lock (LockObject)
            {
                if (PairCollection.IntCollection.ContainsKey(info.Id))
                    PairCollection.IntCollection[info.Id] = info;
                else PairCollection.IntCollection.Add(info.Id, info);

                if (PairCollection.StringCollection.ContainsKey(info.SeoTemplate))
                    PairCollection.StringCollection[info.SeoTemplate] = info;
                else PairCollection.StringCollection.Add(info.SeoTemplate, info);
            }
        }

        public static void DeletePageFromCache(PageInfo info)
        {
            lock (LockObject)
            {

                if (PairCollection.IntCollection.ContainsKey(info.Id))
                    PairCollection.IntCollection.Remove(info.Id);

                if (PairCollection.StringCollection.ContainsKey(info.SeoTemplate))
                    PairCollection.StringCollection.Remove(info.SeoTemplate);
            }
        }

        public static PageInfo GetPageFromCache(int id)
        {
            lock (LockObject)
            {
                if (PairCollection.IntCollection.ContainsKey(id))
                    return PairCollection.IntCollection[id];
                return null;
            }
        }

        public static List<PageInfo> GetPagesFromCache(string urlPattern)
        {
            lock (LockObject)
            {
                List<PageInfo> result = new List<PageInfo>();
                IDictionaryEnumerator enumerator = PairCollection.StringCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.ToString().StartsWith(urlPattern))
                        result.Add((PageInfo)enumerator.Value);
                }
                return result;
            }
        }

        public static PageInfo GetPageFromCache(string name)
        {
            lock (LockObject)
            {
                if (PairCollection.StringCollection.ContainsKey(name))
                    return PairCollection.StringCollection[name];
                return null;
            }
        }

        public static void AddContentTypeToCache(ContentTypeInfo info)
        {
            lock (LockObject)
            {
                if (ContentTypeCollection.IntCollection.ContainsKey(info.Id))
                    ContentTypeCollection.IntCollection[info.Id] = info;
                else ContentTypeCollection.IntCollection.Add(info.Id, info);

                if (ContentTypeCollection.StringCollection.ContainsKey(info.Name))
                    ContentTypeCollection.StringCollection[info.Name] = info;
                else ContentTypeCollection.StringCollection.Add(info.Name, info);
            }
        }

        public static void DeleteContentTypeFromCache(ContentTypeInfo info)
        {
            lock (LockObject)
            {

                if (ContentTypeCollection.IntCollection.ContainsKey(info.Id))
                    ContentTypeCollection.IntCollection.Remove(info.Id);

                if (ContentTypeCollection.StringCollection.ContainsKey(info.Name))
                    ContentTypeCollection.StringCollection.Remove(info.Name);
            }
        }

        public static ContentTypeInfo GetContenTypeFromCache(int id)
        {
            lock (LockObject)
            {
                if (ContentTypeCollection.IntCollection.ContainsKey(id))
                    return ContentTypeCollection.IntCollection[id];
                return null;
            }
        }

        public static ContentTypeInfo GetContenTypeFromCache(string name)
        {
            lock (LockObject)
            {
                if (ContentTypeCollection.StringCollection.ContainsKey(name))
                    return ContentTypeCollection.StringCollection[name];
                return null;
            }
        }

        public static void AddUserProfileToCache(UserProfileInfo info)
        {
            lock (LockObject)
            {

                if (UserProfileCollection.StringCollection.ContainsKey(info.UserId))
                    UserProfileCollection.StringCollection[info.Name] = info;
                else UserProfileCollection.StringCollection.Add(info.UserId, info);
            }
        }

        public static void DeleteUserProfileFromCache(UserProfileInfo info)
        {
            lock (LockObject)
            {
                if (UserProfileCollection.StringCollection.ContainsKey(info.UserId))
                    UserProfileCollection.StringCollection.Remove(info.UserId);
            }
        }

        public static UserProfileInfo GetUserProfileFromCache(string name)
        {
            lock (LockObject)
            {
                if (UserProfileCollection.StringCollection.ContainsKey(name))
                    return UserProfileCollection.StringCollection[name];
                return null;
            }
        }

        public static void ClearTransformationCache()
        {
            lock (LockObject)
            {
                string realPath = HttpContext.Current.Server.MapPath(SiteConstants.TransformationCacheXmlPath);
                XDocument xDoc = XDocument.Load(realPath);
                XElement element = xDoc.Element("transformations");
                if (element != null)
                {
                    if (element.Attribute("updated") != null)
                        element.Attribute("updated").Value = DateTime.Now.ToString();
                    else
                        element.Add(new XAttribute("updated", DateTime.Now));
                    xDoc.Save(realPath);
                }
            }
        }

        public static void ClearLayoutCache()
        {
            lock (LockObject)
            {
                string realPath = HttpContext.Current.Server.MapPath(SiteConstants.LayoutCacheXmlPath);
                XDocument xDoc = XDocument.Load(realPath);
                XElement element = xDoc.Element("layouts");
                if (element != null)
                {
                    if (element.Attribute("updated") != null)
                        element.Attribute("updated").Value = DateTime.Now.ToString();
                    else
                        element.Add(new XAttribute("updated", DateTime.Now));
                    xDoc.Save(realPath);
                }
            }
        }

        public static void ClearArticleCache()
        {
            lock (LockObject)
            {
                string realPath = HttpContext.Current.Server.MapPath(SiteConstants.ArticleCacheXmlPath);
                XDocument xDoc = XDocument.Load(realPath);
                XElement element = xDoc.Element("articles");
                if (element != null)
                {
                    if (element.Attribute("updated") != null)
                        element.Attribute("updated").Value = DateTime.Now.ToString();
                    else
                        element.Add(new XAttribute("updated", DateTime.Now));
                    xDoc.Save(realPath);
                }
            }
        }

        public static void ClearPagesCache()
        {
            lock (LockObject)
            {
                PairCollection.IntCollection.Clear();
                PairCollection.StringCollection.Clear();
            }
        }

        public static void ClearContentTypesCache()
        {
            lock (LockObject)
            {
                ContentTypeCollection.IntCollection.Clear();
                ContentTypeCollection.StringCollection.Clear();
            }
        }

        public static void ClearUserProfileCache()
        {
            lock (LockObject)
            {

                UserProfileCollection.StringCollection.Clear();
            }
        }

        public static void ClearCaches()
        {
            lock (LockObject)
            {
                ClearTransformationCache();
                ClearLayoutCache();
                ClearArticleCache();
                ClearPagesCache();
                ClearContentTypesCache();
                DeleteAll();
                ClearUserProfileCache();
            }
        }
    }
}
