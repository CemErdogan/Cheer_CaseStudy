using System;
using System.Collections;
using Abstractions.CameraSystem;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
            _targetGroup.enabled = true;
            var lookAt = _targetGroup.transform;
            var targets = signal.Targets;

            var center = Vector3.zero;
            _virtualCamera.LookAt = lookAt;
            _targetGroup.m_Targets = new CinemachineTargetGroup.Target[targets.Length];
            for (var i = 0; i < targets.Length; i++)
            {
                _targetGroup.m_Targets[i] = new CinemachineTargetGroup.Target { target = targets[i], weight = 0.5f, radius = 0f };
                center += targets[i].position;
            }
            center /= targets.Length;
            _virtualCamera.transform.position = new Vector3(center.x, center.y, _virtualCamera.transform.position.z);
            DelayClose().Forget();
            
            return;
            async UniTaskVoid DelayClose()
            {
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
                _targetGroup.enabled = false;
            }
        }
    }
}