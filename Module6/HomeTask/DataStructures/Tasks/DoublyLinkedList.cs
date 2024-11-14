using System;
using System.Collections;
using System.Collections.Generic;
using Tasks.DoNotChange;

namespace Tasks
{
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        private class Node
        {
            public T Data;
            public Node Previous;
            public Node Next;

            public Node(T data)
            {
                Data = data;
            }
        }

        private Node head;
        private Node tail;
        private int count;

        public int Length => count;

        public void Add(T e)
        {
            var newNode = new Node(e);

            if (head == null)
            {
                head = tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Previous = tail;
                tail = newNode;
            }

            count++;
        }

        public void AddAt(int index, T e)
        {
            if (index < 0 || index > count)
                throw new IndexOutOfRangeException();

            var newNode = new Node(e);

            if (index == 0)
            {
                if (head == null)
                {
                    head = tail = newNode;
                }
                else
                {
                    newNode.Next = head;
                    head.Previous = newNode;
                    head = newNode;
                }
            }
            else if (index == count)
            {
                Add(e);
                return;
            }
            else
            {
                var current = GetNodeAt(index);
                newNode.Next = current;
                newNode.Previous = current.Previous;

                if (current.Previous != null)
                    current.Previous.Next = newNode;

                current.Previous = newNode;
            }

            count++;
        }

        public T ElementAt(int index)
        {
            var node = GetNodeAt(index);
            return node.Data;
        }

        public void Remove(T item)
        {
            var current = head;

            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Data, item))
                {
                    RemoveNode(current);
                    return;
                }

                current = current.Next;
            }
        }

        public T RemoveAt(int index)
        {
            var node = GetNodeAt(index);
            var data = node.Data;
            RemoveNode(node);
            return data;
        }

        private Node GetNodeAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            var current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }

            return current;
        }

        private void RemoveNode(Node node)
        {
            if (node.Previous != null)
                node.Previous.Next = node.Next;
            else
                head = node.Next;

            if (node.Next != null)
                node.Next.Previous = node.Previous;
            else
                tail = node.Previous;

            count--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            List<T> elements = new List<T>();

            while (current != null)
            {
                elements.Add(current.Data);
                current = current.Next;
            }

            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
