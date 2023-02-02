using FSTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FSLibrary
{
    public class FSDictionary<TKey, TValue>
    {
        private FSList<TKey> keys;
        private FSList<TValue> values;

        public FSDictionary()
        {
            keys = new FSList<TKey>();
            values = new FSList<TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            keys.Add(key);
            values.Add(value);
        }

        public TValue this[TKey key]
        {
            get
            {
                int index = keys.IndexOf(key);
                if (index == -1)
                {
                    throw new IndexOutOfRangeException();
                }
                return values[index];
            }
            set
            {
                int index = keys.IndexOf(key);
                if (index == -1)
                {
                    throw new IndexOutOfRangeException();
                }
                values[index] = value;
            }
        }
        public bool ContainsKey(TKey key)
        {
            return keys.Contains(key);
        }

        public bool Remove(TKey key)
        {
            int index = keys.IndexOf(key);
            if (index == -1)
            {
                return false;
            }
            keys.RemoveAt(index);
            values.RemoveAt(index);
            return true;
        }

        public int Count
        {
            get { return keys.Count; }
        }
    }
}
