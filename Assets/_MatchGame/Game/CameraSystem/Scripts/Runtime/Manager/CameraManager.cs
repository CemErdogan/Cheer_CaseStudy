using System;
using Abstractions.CameraSystem;
using Cinemachine;
using Zenject;

namespace Game.CameraSystem.Runtime
{
    public class CameraManager : ICameraManager, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject(Id = "VirtualCamera")] private readonly CinemachineVirtualCamera  _virtualCamera;
        [Inject(Id = "TargetGroup")] private readonly CinemachineTargetGroup  _targetGroup;
        
        public void Initialize()
        {
            _signalBus.Subscribe<ISetupCameraSignal>(OnSetupCamera);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ISetupCameraSignal>(OnSetupCamera);
        }
        
        private void OnSetupCamera(ISetupCameraSignal signal)
        {
            _targetGroup.transform.position = signal.LookAt.position;
            
            var lookAt = _targetGroup.transform;
            var targets = signal.Targets;
            
            _virtualCamera.LookAt = lookAt;
            _targetGroup.m_Targets = new CinemachineTargetGroup.Target[targets.Length];
            for (var i = 0; i < targets.Length; i++)
            {
                _targetGroup.m_Targets[i] = new CinemachineTargetGroup.Target { target = targets[i], weight = 0.5f, radius = 0f };
            }
        }
    }
}