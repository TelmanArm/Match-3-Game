using System.Collections.Generic;
using UnityEngine;

namespace Utility.ObjectPooling
{
    public class GameObjectPool
    {
        private GameObject _prefab;
        private Transform _holder;
        private Stack<GameObject> _poolActive;
        private Stack<GameObject> _poolInActive;

        public GameObjectPool(GameObject prefab,Transform holder)
        {
            _poolActive = new Stack<GameObject>();
            _poolInActive = new Stack<GameObject>();
            _holder = holder;
            _prefab = prefab;
        }

        public GameObject GetObjectFromPool()
        {
            if (_poolInActive.Count > 0)
            {
                GameObject obj = _poolInActive.Pop();
                obj.SetActive(true);
                return obj;
            }

            GameObject newObj = Object.Instantiate(_prefab, _holder);
            _poolActive.Push(newObj);
            return newObj;
        }

        public void ReturnObjectToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
            _poolInActive.Push(gameObject);
        }
    }
}