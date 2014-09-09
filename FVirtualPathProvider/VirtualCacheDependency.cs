using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Threading;

namespace FVirtualPathProvider
{
    public class VirtualCacheDependency : CacheDependency
    {
        private Timer _timer;
        private readonly object _currentValue;
        private readonly int _pellSec;
        private string _objectId;
        private VirtualDirectoryType _virtualDirectoryType;

        public VirtualCacheDependency(string objectId, VirtualDirectoryType type, int time)
        {
            _objectId = objectId;
            _virtualDirectoryType = type;
            _pellSec = time;

            _currentValue = GetValue();

            if (_timer == null)
            {
                int milliSecund = _pellSec * 1000;
                TimerCallback callback = new TimerCallback(CallBackChecker);
                _timer = new Timer(callback, this, milliSecund, milliSecund);
            }
        }

        private void CallBackChecker(object sender)
        {
            VirtualCacheDependency dependency = sender as VirtualCacheDependency;
            object value = GetValue();
            if (!value.Equals(_currentValue) && dependency != null)
                dependency.NotifyDependencyChanged(dependency, EventArgs.Empty);
        }

        private object GetValue()
        {
            if (this.VirtualDirectory == VirtualDirectoryType.MasterPage || this.VirtualDirectory == VirtualDirectoryType.Layout)
            {
                //  return DataServiceFactory.GetPageTemplateInfoService().ById(int.Parse(_objectId));
            }
            return null;
        }

        protected override void DependencyDispose()
        {
            _timer = null;
            base.DependencyDispose();
        }


        public VirtualDirectoryType VirtualDirectory { get; set; }
    }
}
