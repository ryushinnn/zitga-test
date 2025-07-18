using System;
using System.Collections.Generic;

public class PriorityQueue<TElement,TPriority> where TPriority : IComparable<TPriority> {
    List<(TElement element, TPriority priority)> queue = new();
    
    public int Count => queue.Count;

    public void Enqueue(TElement element, TPriority priority) {
        queue.Add((element, priority));
        queue.Sort((a,b)=>a.priority.CompareTo(b.priority));
    }

    public TElement Dequeue() {
        var item = queue[0];
        queue.RemoveAt(0);
        return item.element;
    }
}