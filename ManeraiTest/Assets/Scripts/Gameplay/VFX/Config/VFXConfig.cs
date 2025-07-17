using System;
using System.Linq;
using UnityEngine;

namespace Gameplay.VFX.Config
{
    [CreateAssetMenu(menuName = "Configs/VFXConfig", fileName = "VFXConfig")]
    public class VFXConfig : ScriptableObject
    {
        [SerializeField] private VFXItem[] _items;
        
        [field: SerializeField, Range(1, 100)] public int PoolCapacity { get; private set; }

        public VFXItem GetPrefabByType(VFXType type)
        {
            var item = _items.FirstOrDefault(x => x.Type == type);

            if (item != null)
            {
                return item;
            }

            throw new ArgumentException($"Нет такого типа как {type}");
        }
    }
}