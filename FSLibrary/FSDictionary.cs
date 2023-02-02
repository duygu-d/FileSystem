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
        public FSList<TKey> Keys;
        public FSList<TValue> Values;

        public FSDictionary()
        {
            Keys = new FSList<TKey>();
            Values = new FSList<TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            Keys.Add(key);
            Values.Add(value);
        }

        public TValue this[TKey key]
        {
            get
            {
                int index = Keys.IndexOf(key);
                if (index == -1)
                {
                    throw new IndexOutOfRangeException();
                }
                return Values[index];
            }
            set
            {
                int index = Keys.IndexOf(key);
                if (index == -1)
                {
                    throw new IndexOutOfRangeException();
                }
                Values[index] = value;
            }
        }
        public bool ContainsKey(TKey key)
        {
            return Keys.Contains(key);
        }

        public bool Remove(TKey key)
        {
            int index = Keys.IndexOf(key);
            if (index == -1)
            {
                return false;
            }
            Keys.RemoveAt(index);
            Values.RemoveAt(index);
            return true;
        }

        public int Count
        {
            get { return Keys.Count; }
        }
    }
}
