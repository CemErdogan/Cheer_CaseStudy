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
        [Inject] private readonly FigureSelected.Factory _figureSelectedFactory;
        [Inject] private readonly TrayManager _trayManager;
        [Inject] private readonly Camera _camera;
        [Inject] private readonly TrayConfig _trayConfig;

        private bool _isDragging;
        private int _dragSlotIndex;
        private FigureSelected _dragCopy;
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

                var figure = hit.GetComponentInParent<Figure>();
                if (figure == null || !_trayManager.IsTrayItem(figure)) return;

                _dragSlotIndex = _trayManager.GetSlotIndex(figure);
                _dragData = _trayManager.GetSlotData(_dragSlotIndex);

                _trayManager.BeginDrag(_dragSlotIndex);

                _dragCopy = _figureSelectedFactory.Create(_dragData);
                _dragCopy.transform.position = _trayConfig.SelectionPosition;

                _isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
            else
            {
                _dragCopy.transform.position = GetWorldPos();
            }
        }

        private void EndDrag()
        {
            var worldPos = GetWorldPos();
            var coord = new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));

            var inBounds = coord.x >= 0 && coord.x < _gridManager.ColumnCount &&
                           coord.y >= 0 && coord.y < _gridManager.RowCount;

            if (inBounds && !_gridManager.IsOccupied(coord))
            {
                PlaceFigure(coord);
            }
            else
            {
                Object.Destroy(_dragCopy.gameObject);
                _trayManager.CancelDrag(_dragSlotIndex);
            }

            _isDragging = false;
            _dragCopy = null;
        }

        private void PlaceFigure(Vector2Int coord)
        {
            var figureData = _dragData.WithGridCoord(coord);

            var figure = _figureFactory.Create(figureData);
            figure.Transform.position = coord.ToPosition();
            _gridManager.PlaceFigure(figure, coord);

            Object.Destroy(_dragCopy.gameObject);
            _trayManager.ConfirmDrag(_dragSlotIndex);

            _signalBus.Fire(new FigurePlacedSignal());
        }

        private Vector3 GetWorldPos()
        {
            var pos = _camera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
            return pos;
        }
    }
}