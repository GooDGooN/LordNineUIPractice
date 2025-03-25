using UnityEngine;

public class CustomCircleLinkedList<T>
{
    private Node head;
    private Node tail;

    public CustomCircleLinkedList<T> AddNode(T node, bool addToHead = false)
    {
        var newNode = new Node(node);
        if (head == null)
        {
            tail = head = newNode;
        }
        else
        {
            if (addToHead)
            {
                var temp = head;

                head = newNode;
                head.next = temp;
                head.prev = tail;

                tail.next = head;
                temp.prev = head;
            }
            else
            {
                var temp = tail;

                tail = newNode;
                tail.next = head;
                tail.prev = temp;

                head.prev = tail;
                temp.next = tail;
            }
        }

        return this;
    }

    public CustomCircleLinkedList<T> HeadToTail()
    {
        var prevHead = head;
        var prevTail = tail;

        head = prevHead.next;
        tail = prevHead;

        prevTail.next = tail;
        tail.next = head;

        return this;
    }

    public CustomCircleLinkedList<T> TailToHead()
    {
        var prevHead = head;
        var prevTail = tail;

        head = prevTail;
        tail = prevTail.prev;

        prevHead.prev = head;
        head.prev = tail;
        
        return this;
    }

    public T GetHeadValue() => head.value;

    public T GetTailValue() => tail.value;

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
