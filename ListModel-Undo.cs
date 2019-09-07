using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class ListModel<TItem>
    {
        enum Action
        {
            AddItem,
            RemoveItem
        }

        public List<TItem> Items { get; }
        LimitedSizeStack<Tuple<Action, TItem, int>> actionsList;
        public int Limit;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            actionsList = new LimitedSizeStack<Tuple<Action, TItem, int>>(limit);
            Limit = limit;
        }

        public void AddItem(TItem item)
        {
            actionsList.Push(new Tuple<Action, TItem, int>(Action.AddItem, item, Items.Count));
            Items.Add(item);
        }

        public void RemoveItem(int index)
        {
            actionsList.Push(new Tuple<Action, TItem, int>(Action.RemoveItem, Items[index], index));
            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            return actionsList.Count != 0;
        }

        public void Undo()
        {
            if (CanUndo())
            {
                var lastAction = actionsList.Pop();
                switch (lastAction.Item1)
                {
                    case Action.AddItem: Items.RemoveAt(lastAction.Item3); break;
                    case Action.RemoveItem: Items.Insert(lastAction.Item3, lastAction.Item2); break;
                }
            }
        }
    }
}
