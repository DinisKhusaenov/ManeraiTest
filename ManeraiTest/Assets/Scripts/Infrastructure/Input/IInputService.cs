using System;
using UnityEngine;
using Zenject;

namespace Infrastructure.Input
{
    public interface IInputService
    {
        event Action<Vector3> OnHitClicked;
    }
}