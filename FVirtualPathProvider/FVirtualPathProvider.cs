using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using FCore.Constant;

namespace FVirtualPathProvider
{
    public class FVirtualPathProvider : VirtualPathProvider
    {
        protected static Hashtable VirtualFiles = new Hashtable();
        protected static Hashtable VirtualDirectories = new Hashtable();

        public static void AppInitialize()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new FVirtualPathProvider());
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            FVirtualFile file = GetVirtualFile(virtualPath);
            if (file != null)
            {
                return file;
            }

            return Previous.GetFile(virtualPath);
        }

        public override bool FileExists(string virtualPath)
        {
            if (IsPathVirtual(virtualPath))
            {
                FVirtualFile file = (FVirtualFile)GetFile(virtualPath);
                return (file != null);
            }
            return Previous.FileExists(virtualPath);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            if (IsPathVirtual(virtualDir))
            {
                FVirtualDirectory dir = GetVirtualDirectory(virtualDir);
                return (dir != null);
            }
            return Previous.DirectoryExists(virtualDir);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {

            if (IsPathVirtual(virtualPath))
            {
                FVirtualFile file = GetVirtualFile(virtualPath);
                if (file.VirtualDirectory == VirtualDirectoryType.Layout || file.VirtualDirectory == VirtualDirectoryType.MasterPage)
                {
                    string realPath = HttpContext.Current.Server.MapPath(SiteConstants.LayoutCacheXmlPath);
                    return new CacheDependency(realPath, utcStart);
                }
                if (file.VirtualDirectory == VirtualDirectoryType.Transformation || file.VirtualDirectory == VirtualDirectoryType.EvaluableTransformation)
                {
                    string realPath = HttpContext.Current.Server.MapPath(SiteConstants.TransformationCacheXmlPath);
                    return new CacheDependency(realPath, utcStart);
                }
                if (file.VirtualDirectory == VirtualDirectoryType.Article)
                {
                    string realPath = HttpContext.Current.Server.MapPath(SiteConstants.ArticleCacheXmlPath);
                    return new CacheDependency(realPath, utcStart);
                }
                return null;
            }
            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

        }

        public static VirtualDirectoryType GetVirtualDirectoryType(string virtualpath)
        {
            string checkPath = VirtualPathUtility.ToAppRelative(virtualpath);


            if (checkPath.StartsWith(FVirtualDirectories.Layouts, StringComparison.InvariantCultureIgnoreCase))
            {
                return VirtualDirectoryType.Layout;
            }
            if (checkPath.StartsWith(FVirtualDirectories.Articles, StringComparison.InvariantCultureIgnoreCase))
            {
                return VirtualDirectoryType.Article;
            }
            if (checkPath.StartsWith(FVirtualDirectories.MasterPages))
            {
                return VirtualDirectoryType.MasterPage;
            }
            if (checkPath.StartsWith(FVirtualDirectories.Transformations))
            {
                return VirtualDirectoryType.Transformation;
            }
            if (checkPath.StartsWith(FVirtualDirectories.EvaluableTransformations))
            {
                return VirtualDirectoryType.EvaluableTransformation;
            }
            return VirtualDirectoryType.Unknown;
        }

        public FVirtualFile GetVirtualFile(string virtualPath)
        {
            FVirtualFile file = (FVirtualFile)VirtualFiles[virtualPath];
            if (file == null)
            {
                if (IsPathVirtual(virtualPath))
                {
                    file = new FVirtualFile(virtualPath, this);
                    VirtualFiles[virtualPath] = file;
                }
            }
            return file;
        }

        public FVirtualDirectory GetVirtualDirectory(string virtualPath)
        {
            FVirtualDirectory dir = (FVirtualDirectory)VirtualDirectories[virtualPath];
            if (dir == null)
            {
                if (IsPathVirtual(virtualPath))
                {
                    dir = new FVirtualDirectory(virtualPath, this);
                    VirtualDirectories[virtualPath] = dir;
                }
            }
            return dir;
        }

        private static bool IsPathVirtual(string path)
        {
            VirtualDirectoryType directory = GetVirtualDirectoryType(path);
            switch (directory)
            {
                case VirtualDirectoryType.Unknown:
                    return false;
                default:
                    return true;
            }
        }
    }
}
