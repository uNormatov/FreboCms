using System;
using System.Collections.Generic;

namespace FCore.Collection
{
    [Serializable]
    public class PairGoodCollection<T> where T : class
    {
        private readonly GoodDictionary<string, T> _stringCollection;
        private readonly GoodDictionary<int, T> _intCollection;

        public PairGoodCollection()
        {
            _stringCollection = new GoodDictionary<string, T>();
            _intCollection = new GoodDictionary<int, T>();
        }

        public GoodDictionary<string, T> StringCollection
        {
            get { return _stringCollection; }
        }

        public GoodDictionary<int, T> IntCollection
        {
            get { return _intCollection; }
        }
    }
}
