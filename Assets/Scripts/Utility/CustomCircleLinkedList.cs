using System.Collections.Generic;
using UnityEngine;

public class CustomCircleLinkedList<T>
{
    public Node Head;
    public Node Tail;

    public CustomCircleLinkedList<T> AddNode(T node, bool addToHead = false)
    {
        var newNode = new Node(node);
        if (Head == null)
        {
            Tail = Head = newNode;
        }
        else
        {
            if (addToHead)
            {
                var temp = Head;

                Head = newNode;
                Head.next = temp;
                Head.prev = Tail;

                Tail.next = Head;
                temp.prev = Head;
            }
            else
            {
                var temp = Tail;

                Tail = newNode;
                Tail.next = Head;
                Tail.prev = temp;

                Head.prev = Tail;
                temp.next = Tail;
            }
        }

        return this;
    }

    public CustomCircleLinkedList<T> HeadToTail()
    {
        var prevHead = Head;
        var prevTail = Tail;

        Head = prevHead.next;
        Tail = prevHead;

        prevTail.next = Tail;
        Tail.next = Head;

        return this;
    }

    public CustomCircleLinkedList<T> TailToHead()
    {
        var prevHead = Head;
        var prevTail = Tail;

        Head = prevTail;
        Tail = prevTail.prev;

        prevHead.prev = Head;
        Head.prev = Tail;
        
        return this;
    }

    public CustomCircleLinkedList<T> GoToTail(T targetValue)
    {
        var node = Head;

        while (!EqualityComparer<T>.Default.Equals(node.value, targetValue))
        {
            if (node == Tail)
            {
                return null;
            }

            node = node.next;
        }

        if (node == Head)
        {
            HeadToTail();
            return this;
        }

        node.prev.next = node.next;
        node.next.prev = node.prev;

        Tail.next = node;
        Head.prev = node;

        node.next = Head;
        node.prev = Tail;

        Tail = node;

        return this;
    }

    public T GetHeadValue() => Head.value;

    public T GetTailValue() => Tail.value;

    public class Node
    {
        public T value;
        public Node next;
        public Node prev;

        public Node(T value)
        {
            this.value = value;
        }
    }
}
