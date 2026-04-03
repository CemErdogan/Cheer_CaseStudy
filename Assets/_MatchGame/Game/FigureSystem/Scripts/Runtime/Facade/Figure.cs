using System.Collections.Generic;
using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class Figure : MonoBehaviour, IFigure
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly Point.Factory _pointFactory;
        [Inject] private readonly ConnectionDatabase _connectionDb;

        private readonly Dictionary<SlotPosition, IPoint> _pointMap = new();

        public IReadOnlyDictionary<SlotPosition, IPoint> PointMap => _pointMap;
        public Vector2Int GridCoord { get; set; }
        public bool       IsBigSquare { get; private set; }
        public Transform  Transform => transform;
        public GameObject GameObject => gameObject;

        public void Prepare(FigureData data)
        {
            GridCoord = data.GridCoord;

            foreach (var pointData in data.Points)
            {
                var slotData = _connectionDb.GetData(pointData.Position);
                var point    = _pointFactory.Create(pointData);
                point.Initialize(pointData, this, slotData);
                _pointMap[pointData.Position] = point;
            }

            if (data.IsSquare)
                ApplyBigSquare();

            _signalBus.Fire(new FigureSpawnSignal(data));
        }

        public bool TryGetPoint(SlotPosition slot, out IPoint point) => _pointMap.TryGetValue(slot, out point);

        public void AddPoint(SlotPosition slot, IPoint point)
        {
            _pointMap[slot] = point;
        }

        public void RemovePoint(SlotPosition slot)
        {
            if (!_pointMap.Remove(slot, out var point)) return;
            if (point is Component comp && comp != null)
                Destroy(comp.gameObject);
        }

        public bool IsEmpty() => _pointMap.Count == 0;

        public void ApplyBigSquare()
        {
            IsBigSquare = true;
            foreach (var point in _pointMap.Values)
            {
                point.SetBigSquare();
            }
            transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        }

        public class Factory : PlaceholderFactory<FigureData, Figure>, IFigureFactory
        {
            public override Figure Create(FigureData data)
            {
                var figure = base.Create(data);
                figure.Prepare(data);
                return figure;
            }

            IFigure IFigureFactory.Create(FigureData data) => Create(data);
        }
    }
}
