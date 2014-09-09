using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCore.Class;
using FCore.Collection;

namespace FDataProvider
{
    public abstract class BaseProvider<T> : IDisposable
    {
        protected GeneralConnection Connection;
        protected DataConnection DataConnection;
        public abstract object Create(T info, ErrorInfoList errors);
        public abstract bool Update(T info, ErrorInfoList errors);
        public abstract bool Delete(int id, ErrorInfoList errors);
        public abstract T Select(int id, ErrorInfoList errors);
        public abstract List<T> SelectAll(ErrorInfoList errors);
        public abstract List<T> SelectPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors);
        protected void RegisterError(ErrorInfoList errors, ErrorInfo log)
        {
            if (errors == null)
                errors = new ErrorInfoList();

            if (!log.Ok)
                errors.Add(log);

        }

        public abstract void RegisterObjectToCache(T info);

        public abstract void DeleteObjectFromCache(T info);

        public abstract T GetObjectFromCache(int id);

        public abstract T GetObjectFromCache(string name);

        #region IDisposable
        private bool disposed;
        ~BaseProvider()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (DataConnection != null)
                    DataConnection.Dispose();
                Dispose(true);
                GC.SuppressFinalize(this);
                disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Connection != null)
                    Connection = null;
            }
        }
        #endregion
    }
}
