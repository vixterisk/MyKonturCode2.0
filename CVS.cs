using System;
using System.Collections.Generic;

namespace Clones
{
    public class StackItem<T>
    {
        public T Value { get; set; }
        public StackItem<T> Prev { get; set; }
        public StackItem<T> Next { get; set; }
    }

    public class Stack<T>
    {
        StackItem<T> head;
        StackItem<T> tail;
        public bool IsEmpty { get { return head == null; } }

        public int Count { get; private set; }

        public Stack()
        {
            Count = 0;
            head = tail = null;
        }

        public void Push(T item)
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

        public Stack<T> Copy()
        {
            return new Stack<T>() { head = this.head, tail = this.tail};
        }
    }

    public class CloneItem
    {
        Stack<string> programs;
        Stack<string> history;

        public CloneItem()
        {
            programs = new Stack<string>();
            history = new Stack<string>();
        }

        public void Learn(string program)
        {
            programs.Push(program);
        }

        public void Rollback()
        {
            var lastLearnedProgram = programs.Pop();
            history.Push(lastLearnedProgram);
        }

        public void Relearn()
        {
            programs.Push(history.Pop());
        }

        public CloneItem Clone()
        {
            return new CloneItem() { programs = programs.Copy(), history = history.Copy() };
        }

        public string Check()
        {
            if (programs.IsEmpty) return "basic";
            var lastProgram = programs.Pop();
            programs.Push(lastProgram);
            return lastProgram;
        }
    }

    public class CloneVersionSystem : ICloneVersionSystem
	{
        List<CloneItem> clones = new List<CloneItem>() { new CloneItem() };

		public string Execute(string query)
		{
            var instruction = query.Split();
            var command = instruction[0];
            var cloneNumber = Convert.ToInt32(instruction[1]) - 1;
            switch (command)
            {
                case "learn": clones[cloneNumber].Learn(instruction[2]);  break;
                case "rollback": clones[cloneNumber].Rollback(); break;
                case "relearn": clones[cloneNumber].Relearn();  break;
                case "clone": clones.Add(clones[cloneNumber].Clone()); break;
                case "check": return clones[cloneNumber].Check();
            }
            return null;
		}
    }
}
