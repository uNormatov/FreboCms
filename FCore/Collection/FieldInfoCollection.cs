using System;
using System.Collections.Generic;
using System.Linq;
using FCore.Class;

namespace FCore.Collection
{

    [Serializable]
    public class FieldInfoCollection : ICollection<FieldInfo>
    {
        private readonly List<FieldInfo> _itemArray;

        public FieldInfoCollection()
        {
            _itemArray = new List<FieldInfo>();
        }

        public FieldInfoCollection(IEnumerable<FieldInfo> array)
        {
            _itemArray = array != null ? array.ToList() : new List<FieldInfo>();
        }

        public void Add(FieldInfo item)
        {
            _itemArray.Add(item);
        }

        public FieldInfo this[int index]
        {
            get
            {
                return _itemArray[index];
            }
            set
            {
                _itemArray[index] = value;
            }
        }

        public FieldInfo this[string name]
        {
            get
            {
                foreach (FieldInfo item in _itemArray)
                {
                    if (item.Name == name)
                        return item;
                }
                return new FieldInfo();
            }
            set
            {
                int index = _itemArray.TakeWhile(item => item.Name != name).Count();
                _itemArray[index] = value;
            }
        }

        public void Clear()
        {
            _itemArray.Clear();
        }

        public bool Contains(FieldInfo item)
        {
            return _itemArray.Contains(item);
        }

        public void CopyTo(FieldInfo[] array)
        {
            for (int i = 0; i < array.Count(); i++)
            {
                _itemArray[i] = array[i];
            }
        }

        public void CopyTo(FieldInfo[] array, int arrayIndex)
        {
            for (int i = 0; i < arrayIndex; i++)
            {
                _itemArray[i] = array[i];
            }
        }

        public int Count
        {
            get { return _itemArray.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(FieldInfo item)
        {
            _itemArray.Remove(item);
            return true;
        }

        public IEnumerator<FieldInfo> GetEnumerator()
        {
            return _itemArray.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _itemArray.GetEnumerator();
        }
    }
}

