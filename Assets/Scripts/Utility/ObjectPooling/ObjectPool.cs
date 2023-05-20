using System.Collections.Generic;
using Board;
using Game;
using UnityEngine;

namespace CustomPooling
{
    public class ObjectPool<T> where T : class

    {
        private Stack<T> _poolObjects;
        public bool IsContain => _poolObjects.Count > 0;
        
        public ObjectPool()
        {
            _poolObjects = new Stack<T>();
        }

        public T GetPooledObject()
        {
            if (_poolObjects.Count > 0)
            {
                T gameObject = _poolObjects.Pop();
                return gameObject;
            }
            return null;
        }

        public void ReturnPoolObject(T gameObject)
        {
            _poolObjects.Push(gameObject);
        }
    }
}