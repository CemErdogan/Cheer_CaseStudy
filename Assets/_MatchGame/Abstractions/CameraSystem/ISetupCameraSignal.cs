using UnityEngine;

namespace Abstractions.CameraSystem
{
    public interface ISetupCameraSignal
    {
        Transform LookAt { get; }
        Transform[] Targets { get; }
    }
    
    public struct SetupCameraSignal : ISetupCameraSignal
    {
        public Transform LookAt { get; }
        public Transform[] Targets { get; }

        public SetupCameraSignal(Transform lookAt, Transform[] targets)
        {
            LookAt = lookAt;
            Targets = targets;
        }
    }
}