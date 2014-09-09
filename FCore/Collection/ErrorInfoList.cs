using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FCore.Class;

namespace FCore.Collection
{
    [Serializable]
    public class ErrorInfoList : ICollection<ErrorInfo>
    {
        private readonly List<ErrorInfo> _array;

        public ErrorInfoList()
        {
            _array = new List<ErrorInfo>();
        }

        public void Add(ErrorInfo item)
        {
            _array.Add(item);
        }

        public void AddRange(ErrorInfoList errors)
        {
            if (errors != null)
            {
                foreach (ErrorInfo t in errors)
                {
                    _array.Add(t);
                }
            }
        }

        public ErrorInfo this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }

        public ErrorInfo this[string name]
        {
            get
            {
                foreach (ErrorInfo item in _array)
                {
                    if (item.Name == name)
                        return item;
                }
                return new ErrorInfo();
            }
            set
            {
                int index = 0;
                foreach (ErrorInfo item in _array)
                {
                    if (item.Name == name)
                        break;
                    index++;
                }
                _array[index] = value;
            }
        }

        public void Clear()
        {
            _array.Clear();
        }

        public bool Contains(ErrorInfo item)
        {
            return _array.Contains(item);
        }

        public void CopyTo(ErrorInfo[] array)
        {
            for (int i = 0; i < array.Count(); i++)
            {
                _array[i] = array[i];
            }
        }

        public void CopyTo(ErrorInfo[] array, int arrayIndex)
        {
            for (int i = 0; i < arrayIndex; i++)
            {
                _array[i] = array[i];
            }
        }

        public int Count
        {
            get { return _array.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ErrorInfo item)
        {
            _array.Remove(item);
            return true;
        }

        public IEnumerator<ErrorInfo> GetEnumerator()
        {
            return _array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _array.GetEnumerator();
        }

        public bool HasError()
        {
            for (int i = 0; i < _array.Count; i++)
            {
                if (!_array[i].Ok)
                    return true;
            }
            return false;
        }
    }
}
