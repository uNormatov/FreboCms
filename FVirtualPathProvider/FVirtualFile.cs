using System.IO;
using System.Text;
using System.Web.Hosting;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;

namespace FVirtualPathProvider
{
    public class FVirtualFile : VirtualFile
    {
        private FVirtualPathProvider _provider;
        private readonly LayoutProvider _layoutProvider;
        private readonly TransformationProvider _transformationProvider;
        private readonly ArticleProvider _articleProvider;
        private string _objectid;
        public string ObjectId
        {
            get
            {
                if (string.IsNullOrEmpty(_objectid))
                    _objectid = VirtualPathHelper.GetFileName(VirtualPath);
                return _objectid;
            }
            set
            {
                _objectid = value;
            }
        }

        private VirtualDirectoryType _virtualdirectory;
        public VirtualDirectoryType VirtualDirectory
        {
            get
            {
                if (_virtualdirectory == 0)
                    _virtualdirectory = FVirtualPathProvider.GetVirtualDirectoryType(VirtualPath);
                return _virtualdirectory;
            }
        }

        public FVirtualFile(string virtualpath, FVirtualPathProvider virtualpathpprovider)
            : base(virtualpath)
        {
            _provider = virtualpathpprovider;
            _layoutProvider = new LayoutProvider();
            _transformationProvider = new TransformationProvider();
            _articleProvider = new ArticleProvider();
        }

        public override Stream Open()
        {
            Stream stream = new MemoryStream();
            string content = GetContent();
            if (!string.IsNullOrEmpty(content))
            {
                StreamWriter writer = new StreamWriter(stream, Encoding.Unicode);
                writer.Write(content);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
            }

            return stream;
        }

        public string GetContent()
        {

            string result;
            if (VirtualDirectory == VirtualDirectoryType.MasterPage || VirtualDirectory == VirtualDirectoryType.Layout)
            {
                result = "<%@ Control Language=\"C#\" ClassName=\"Simple\" Inherits=\"FUIControls.PortalControl.FAbstractLayout\" %>  \n"
        + "<%@ Register Assembly=\"FUIControls\" Namespace=\"FUIControls.PortalControl\" TagPrefix=\"fr\" %> \n  {0} \n";
                LayoutInfo layoutInfo = _layoutProvider.Select(ValidationHelper.GetInteger(ObjectId, 0), new ErrorInfoList());

                result = string.Format(result, layoutInfo != null ? layoutInfo.Layout : "");
            }
            else if (VirtualDirectory == VirtualDirectoryType.Transformation)
            {
                result = "<%@ Control Language=\"C#\" ClassName=\"Simple\" Inherits=\"FUIControls.PortalControl.FAbstractTransformation\" %>  \n"
        + "<%@ Register Assembly=\"FUIControls\" Namespace=\"FUIControls.PortalControl\" TagPrefix=\"fr\" %> \n  {0} \n";
                TransformationInfo info = _transformationProvider.SelectByName(ObjectId, new ErrorInfoList());
                result = string.Format(result, info != null ? info.Text.ToHtmlDecode() : "");
            }
            else if (VirtualDirectory == VirtualDirectoryType.EvaluableTransformation)
            {
                result = "<%@ Control Language=\"C#\" ClassName=\"Simple\" Inherits=\"FUIControls.PortalControl.FAbstractEvaluableTransformation\" %>  \n"
        + "<%@ Register Assembly=\"FUIControls\" Namespace=\"FUIControls.PortalControl\" TagPrefix=\"fr\" %> \n  {0} \n";
                TransformationInfo info = _transformationProvider.SelectByName(ObjectId, new ErrorInfoList());
                result = string.Format(result, info != null ? info.Text.ToHtmlDecode() : "");
            }
            else
            {
                result = "<%@ Control Language=\"C#\" ClassName=\"Simple\" Inherits=\"FUIControls.PortalControl.AbstractControl\" %>  \n"
       + "<%@ Register Assembly=\"FUIControls\" Namespace=\"FUIControls.PortalControl\" TagPrefix=\"fr\" %> \n  {0} \n";
                ArticleInfo info = _articleProvider.Select(ValidationHelper.GetInteger(ObjectId, 0), new ErrorInfoList());
                result = string.Format(result, info != null ? info.Text.ToHtmlDecode() : "");
            }
            return result;
        }
    }
}
