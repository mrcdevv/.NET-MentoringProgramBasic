namespace RecentlyUsedList.Logic
{
    public class RUL
    {

            private readonly LinkedList<string> _list = new();
            private readonly int _capacity;

            public RUL(int capacity = 5)
            {
                if (capacity < 0)
                    throw new ArgumentException("Capacity cannot be negative.");

                _capacity = capacity;
            }

            public int Count => _list.Count;

            public void Add(string item)
            {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ArgumentException("Item cannot be null or empty.");

                if (_list.Contains(item))
                {
                    _list.Remove(item);
                }

                _list.AddFirst(item);

                if (_capacity > 0 && _list.Count > _capacity)
                {
                    _list.RemoveLast();
                }
            }

            public string Get(int index)
            {
                if (index < 0 || index >= _list.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

                using var enumerator = _list.GetEnumerator();
                for (int i = 0; i <= index; i++)
                {
                    enumerator.MoveNext();
                }

                return enumerator.Current;
            }
    }
}
