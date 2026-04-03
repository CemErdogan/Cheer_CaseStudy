using System;
using Abstractions.FigureSystem;
using Abstractions.GridSystem;
using Abstractions.MatchSystem;
using Common.Util;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.FigureSystem.Runtime
{
    public class DragManager : IInitializable, IDisposable, ITickable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly IGridManager _gridManager;
        [Inject] private readonly IFigureFactory _figureFactory;
        [Inject] private readonly TrayManager _trayManager;
        [Inject] private readonly Camera _camera;

        private bool _isDragging;
        private FigureSelected _dragSource;
        private FigureData _dragData;

        public void Initialize() { }
        public void Dispose() { }

        public void Tick()
        {
            if (!_isDragging)
            {
                if (!Input.GetMouseButtonDown(0)) return;

                var worldPos = GetWorldPos();
                var hit = Physics2D.OverlapPoint(worldPos);
                if (hit == null) return;

                var fs = hit.GetComponentInParent<FigureSelected>();
                if (fs == null) return;

                _dragSource = fs;
                _dragData = _trayManager.GetData(fs);
                _isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
            else
            {
                _dragSource.transform.position = GetWorldPos();
            }
        }

        private void EndDrag()
        {
            var worldPos = GetWorldPos();
            var coord = new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));

            var inBounds = coord.x >= 0 && coord.x < _gridManager.ColumnCount && coord.y >= 0 && coord.y < _gridManager.RowCount;

            if (inBounds && !_gridManager.IsOccupied(coord))
            {
                PlaceFigure(coord);
            }
            else
            {
                _dragSource.transform.position = _trayManager.GetTrayPosition(_dragSource);
            }

            _isDragging = false;
            _dragSource = null;
        }

        private void PlaceFigure(Vector2Int coord)
        {
            var figureData = _dragData.WithGridCoord(coord);

            var figure = _figureFactory.Create(figureData);
            figure.Transform.position = coord.ToPosition();
            _gridManager.PlaceFigure(figure, coord);

            _signalBus.Fire(new FigurePlacedSignal());
            _trayManager.OnFigurePlaced(_dragSource);
            Object.Destroy(_dragSource.gameObject);
        }

        private Vector3 GetWorldPos()
        {
            var pos = _camera.ScreenToWorldPoint(Input.mousePosition);
            pos.z   = 0f;
            return pos;
        }
    }
}
