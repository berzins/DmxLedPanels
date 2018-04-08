using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.Util
{
    public class FixedSizeStack<T> : IEnumerable<T>

    {
        protected int size = 0;
        protected List<T> items;
       
        public FixedSizeStack(int size)
        {
            this.size = size;
            this.items = new List<T>();
        }

        public void Add(T item) {
            if (items.Count == size) {
                items.Remove(items[0]);
            }
            items.Add(item);
        }

        public void Clear() {
            items = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = items.Count - 1; i >= 0; i--) {
                yield return items[i]; 
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
