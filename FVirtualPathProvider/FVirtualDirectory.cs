using System.Collections;
using System.Web.Hosting;

namespace FVirtualPathProvider
{
    public class FVirtualDirectory : VirtualDirectory
    {
        #region "Variables"

        protected FVirtualPathProvider _provider;

        private readonly ArrayList children = new ArrayList();
        private readonly ArrayList directories = new ArrayList();
        private readonly ArrayList files = new ArrayList();

        #endregion

        #region "Properties"

        public override IEnumerable Children
        {
            get { return children; }
        }

        public override IEnumerable Directories
        {
            get { return directories; }
        }

        public override IEnumerable Files
        {
            get { return files; }
        }

        #endregion

        public FVirtualDirectory(string virtualDir, FVirtualPathProvider provider)
            : base(virtualDir)
        {
            _provider = provider;
        }
    }
}
