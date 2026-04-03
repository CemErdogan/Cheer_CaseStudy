using System;
using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class Point : MonoBehaviour, IPoint
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly IPointView _view;

        public SlotPosition SlotPos { get; private set; }
        public ColorType Color { get; private set; }
        public bool IsConnected { get; private set; }
        public SlotPosition ConnectedWith { get; private set; }
        public bool IsBigSquare { get; private set; }
        public IFigure ParentFigure { get; private set; }
        public Transform Transform => transform;

        public void Prepare(PointData data)
        {
            _signalBus.Fire(new PointSpawnSignal(data));
        }

        public void Initialize(PointData data, IFigure parent, ConnectionData slotData)
        {
            SlotPos = data.Position;
            Color = data.Color;
            IsBigSquare = data.IsBigSquare;
            ParentFigure = parent;

            if (parent != null)
                transform.SetParent(parent.Transform, false);

            if (data.IsConnected)
            {
                IsConnected   = true;
                ConnectedWith = data.ConnectedWith;
                _view.Prepare(data.Position, data.Color, slotData, isConnected: true);
            }
            else
            {
                _view.Prepare(data.Position, data.Color, slotData, isConnected: false);
            }
        }

        public void SetConnection(SlotPosition targetSlot, ConnectionData mySlotData)
        {
            if (IsBigSquare) return;
            IsConnected   = true;
            ConnectedWith = targetSlot;
            _view.SetConnectedVisual(mySlotData);
        }

        public void SetBigSquare()
        {
            IsBigSquare = true;
        }

        public void AnimateDestroy(Vector3 center, float duration, Action onComplete)
        {
            _view.AnimateDestroy(center, duration, onComplete);
        }

        public class Factory : PlaceholderFactory<PointData, Point>
        {
            public override Point Create(PointData data)
            {
                var point = base.Create(data);
                point.Prepare(data);
                return point;
            }
        }
    }
}
