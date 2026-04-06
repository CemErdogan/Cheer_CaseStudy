using UnityEngine;

namespace Abstractions.CameraSystem
{
    public interface ISetupCameraSignal
    {
        Transform[] Targets { get; }
    }
    
    public struct SetupCameraSignal : ISetupCameraSignal
    {
        public Transform[] Targets { get; }

        public SetupCameraSignal(Transform[] targets)
        {
            Targets = targets;
        }
    }
}