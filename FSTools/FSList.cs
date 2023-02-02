namespace FSTools
{
    public class FSList<T> : IEnumerable<T>
    {
        private const int chunkSize = 100;
        private T[] array;
        private int lastIndex = 0;
        public FSList()
        {
            array = new T[chunkSize];
        }

        public T this[int i]
        {
            get { return array[i]; }
            set
            {
                if (value == null)
                {
                    RemoveAt(i);
                }
                else
                {
                    array[i] = value;
                }
            }
        }

        public int Count { get => lastIndex; }

        public void Add(T item)
        {
            lastIndex++;
            if (lastIndex == array.Length)
            {
                Expand();
            }
            array[lastIndex] = item;
        }
        public int IndexOf(T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Contains(T element)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(element))
                {
                    return true;
                }
            }
            return false;
        }
        public void Remove(T item)
        {
            int i = 0;
            while (i < array.Length)
            {
                if (array[i]!.Equals(item))
                {
                    RemoveAt(i);
                }
            }
        }

        public void RemoveAt(int index)
        {
            int newSize = array.Length;
            while (lastIndex < newSize - chunkSize)
            {
                newSize -= chunkSize;
            }
            T[] newArray = new T[newSize];
            Array.Copy(array, newArray, index);
            Array.Copy(array, index + 1, newArray, index, array.Length - index);
            array = newArray;
            lastIndex--;
        }

        public T[] ToArray()
        {
            T[] result = new T[lastIndex];
            Array.Copy(array, result, lastIndex);
            return result;
        }

        private void Expand()
        {
            T[] newArray = new T[chunkSize];
            Array.Copy(array, newArray, array.Length);
            array = newArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in array)
            {
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            FSList<T>? list = obj as FSList<T>;
            if (list == null)
            {
                return false;
            }

            if (lastIndex != list.Count)
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                if (!array[i].Equals(list[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (T item in array)
            {
                hashCode = hashCode ^ item.GetHashCode();
            }
            return hashCode;
        }
    }
}
