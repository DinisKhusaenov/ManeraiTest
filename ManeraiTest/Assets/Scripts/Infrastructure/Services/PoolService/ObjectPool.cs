using System.Collections.Generic;
using Infrastructure.Services.PoolService.Factory;
using UnityEngine;

namespace Infrastructure.Services.PoolService
{
    public class ObjectPool<TComponent> where TComponent : MonoBehaviour, IPullObject
    {
        private readonly IPoolFactory _factory;
        
        private Transform _parent;
        private Stack<TComponent> _entries;

        public PoolObjectType Type { get; private set; }
        public IReadOnlyCollection<TComponent> Entries => _entries;
        
        public ObjectPool(IPoolFactory factory)
        {
            _factory = factory;
        }
        
        public void Initialize(int startCapacity, PoolObjectType type, Transform parent)
        {
            Type = type;
            _parent = parent;

            _entries = new Stack<TComponent>(startCapacity);
        
            for (int i = 0; i < startCapacity; i++)
            {
                AddObject();
            }
        }
        
        public TComponent Get(Vector3 position, Transform parent = null)
        {
            if (_entries.Count == 0)
            {
                AddObject();
            }
        
            TComponent poolObject = _entries.Pop();
            
            poolObject.transform.position = position;
            if (parent != null)
            {
                poolObject.transform.SetParent(parent);
            }
            poolObject.gameObject.SetActive(true);
            
            return poolObject;
        }
        
        public void Return(TComponent poolObject)
        {
            poolObject.gameObject.SetActive(false);
            poolObject.transform.position = _parent.transform.position;
            poolObject.transform.SetParent(_parent);
            poolObject.Reset();
            
            _entries.Push(poolObject);
        }
        
        private void AddObject()
        {
            TComponent newObject = _factory.Create<TComponent>(Type, _parent.transform.position, _parent);
            newObject.gameObject.SetActive(false);
            _entries.Push(newObject);
        }
    }
}