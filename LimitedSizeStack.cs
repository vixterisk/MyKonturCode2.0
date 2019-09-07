using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class StackItem<T>
    {
        public T Value { get; set; }
        public StackItem<T> Prev { get; set; }
        public StackItem<T> Next { get; set; }
    }

    public class LimitedSizeStack<T>
    {
        public readonly int Limit;
        StackItem<T> head;
        StackItem<T> tail;
        public bool IsEmpty { get { return head == null; } }

        public int Count { get; private set; }

        public LimitedSizeStack(int limit)
        {
            Limit = limit;
            Count = 0;
            head = tail = null;
        }

        public void Push(T item)
        {
            if (Limit > 0)
            {
                var newElement = new StackItem<T> { Value = item, Prev = tail, Next = null };
                Count++;
                if (IsEmpty)
                    tail = head = newElement;
                else
                {
                    tail.Next = newElement;
                    tail = newElement;
                }
                if (Count > Limit)
                {
                    head = head.Next;
                    head.Prev = null;
                    Count--;
                }
            }
        }

        public T Pop()
        {
            if (head == null) throw new InvalidOperationException();
            Count--;
            var value = tail.Value;
            if (head == tail)
                head = null;
            else
                (tail.Prev).Next = null;
            tail = tail.Prev;
            return value;
        }
    }
}
