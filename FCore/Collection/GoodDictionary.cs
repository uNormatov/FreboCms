using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FCore.Collection
{
    /// <summary>
    /// Hashtablega o'xshash custom collection;
    /// </summary>
    /// <typeparam name="TKeyType">Key type</typeparam>
    /// <typeparam name="TValueType">Value type</typeparam>
    [Serializable]
    public class GoodDictionary<TKeyType, TValueType> : Hashtable where TValueType : class
    {
        /// <summary>
        /// GoodDictionary constructor
        /// </summary>
        public GoodDictionary()
        {
        }

        /// <summary>
        /// Deserializatisiya uchun constructor
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        public GoodDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Dictionary Null qiymatlarni qabul qiladi.
        /// </summary>
        public bool AllowNulls { get; set; }

        /// <summary>
        /// Dictionary qiymati uchun Default qiymatni qabul qiladi.
        /// </summary>
        public TValueType DefaultValue { get; set; }

        /// <summary>
        /// Itemlar indexeri . Dictionaryga qiymatni qo'yib oishi mumkin.
        /// </summary>
        /// <param name="key">keyType key</param>
        public TValueType this[TKeyType key]
        {
            get { return (TValueType)base[key]; }
            set
            {
                if ((value == null) && AllowNulls)
                {
                    value = (TValueType)((object)DBNull.Value);
                }
                base[key] = value;
            }
        }


        /// <summary>
        /// Qiymatni olishga urinadi. Agar value olishda hato bo'lmasa True qaytadi .
        /// </summary>
        /// <param name="key">keyType key</param>
        /// <param name="value">valueType value</param>
        public bool TryGetValue(TKeyType key, out TValueType value)
        {
            // Get the data
            object obj = base[key];
            if ((obj == DBNull.Value) && AllowNulls)
            {
                value = default(TValueType);
                return true;
            }
            if (obj != null)
            {
                value = (TValueType)obj;
                return true;
            }
            value = DefaultValue;
            return false;
        }
    }
}
