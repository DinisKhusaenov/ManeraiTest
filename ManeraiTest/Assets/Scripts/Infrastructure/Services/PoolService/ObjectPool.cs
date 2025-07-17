using System;
using System.Collections.Generic;
using Gameplay.VFX;
using Infrastructure.Services.PoolService.Factory;
using UnityEngine;

namespace Infrastructure.Services.PoolService
{
    public class ObjectPool<TComponent> where TComponent : MonoBehaviour, IPullObject
    {
        private readonly IPoolFactory _factory;
        
        private Transform _parent;
        private Stack<TComponent> _entries;

        private VFXType _vfxType = VFXType.None;

        public PoolObjectType Type { get; private set; }
        public IReadOnlyCollection<TComponent> Entries => _entries;
        
        public ObjectPool(IPoolFactory factory)
        {
            _factory = factory;
        }
        
        public void Initialize(int startCapacity, PoolObjectType type, Transform parent, VFXType vfxType = VFXType.None)
        {
            Type = type;
            _parent = parent;
            _vfxType = vfxType;

            _entries = new Stack<TComponent>(startCapacity);
        
            for (int i = 0; i < startCapacity; i++)
            {
                AddObject(type, vfxType);
            }
        }
        
        public TComponent Get(Vector3 position, Transform parent = null)
        {
            if (_entries.Count == 0)
            {
                AddObject(Type, _vfxType);
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
        
        private void AddObject(PoolObjectType type, VFXType vfxType = VFXType.None)
        {
            TComponent newObject;
            switch (type)
            {
                case PoolObjectType.Sound:
                    newObject = _factory.CreateSound<TComponent>(_parent.transform.position, _parent);
                    break;
                    
                case PoolObjectType.VFX:
                    newObject = _factory.CreateVFX<TComponent>(_parent.transform.position, vfxType, _parent);
                    break;
                    
                default:
                    throw new ArgumentException($"PoolObject with type {type} does not exist");
            }
            
            newObject.gameObject.SetActive(false);
            _entries.Push(newObject);
        }
    }
}