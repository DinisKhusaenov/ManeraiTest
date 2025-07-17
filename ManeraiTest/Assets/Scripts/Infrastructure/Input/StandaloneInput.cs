using System;
using UnityEngine;
using Zenject;

namespace Infrastructure.Input
{
    public class StandaloneInput : IInputService, ITickable
    {
        public event Action<Vector3> OnHitClicked;
        
        public void Tick()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                OnHitClicked?.Invoke(UnityEngine.Input.mousePosition);
            }
        }
    }
}