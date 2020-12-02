using System.Collections.Generic;

namespace LRUCache
{
    public class LRUCache<K, V> where V : class
    {
        private int _capacity;
        private Dictionary<K, LinkedListNode<KeyValuePair<K, V>>> _data;
        private LinkedList<KeyValuePair<K, V>> _dataList;

        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _data = new Dictionary<K, LinkedListNode<KeyValuePair<K, V>>>(capacity);
            _dataList = new LinkedList<KeyValuePair<K, V>>();
        }

        public void Put(K key, V value)
        {
            if (_data.TryGetValue(key, out var node))
            {
                node.Value = new KeyValuePair<K, V>(key, value);
                MoveOnTop(node);
                return;
            }

            if (_data.Count == _capacity && _dataList.Last != null)
            {
                var lastKey = _dataList.Last.Value.Key;
                _dataList.RemoveLast();
                _data.Remove(lastKey);
            }

            var newNode = new LinkedListNode<KeyValuePair<K, V>>(new KeyValuePair<K, V>(key, value));
            _dataList.AddFirst(newNode);
            _data.Add(key, newNode);
        }
        
        public V Get(K key)
        {
             if (_data.TryGetValue(key, out var node))
             {
                 MoveOnTop(node);
                 return node.Value.Value;
             }

             return null;
        }
        
        public bool TryGetValue(K key, out V val)
        {
            val = Get(key);

            return val != null;
        }

        public bool Remove(K key)
        {
            if (_data.TryGetValue(key, out var node))
            {
                _dataList.Remove(node);
                _data.Remove(key);
                return true;
            }

            return false;
        }
        
        private void MoveOnTop(LinkedListNode<KeyValuePair<K, V>> node)
        {
            if (node.Previous == null)
            {
                return;
            }
            
            if (node.Next == null)
            {
                _dataList.RemoveLast();
                _dataList.AddFirst(node);
                return;
            }

            _dataList.Remove(node);
            _dataList.AddFirst(node);
        }
    }
}