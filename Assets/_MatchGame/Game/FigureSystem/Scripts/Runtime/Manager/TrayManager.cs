using System;
using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.FigureSystem.Runtime
{
    public class TrayManager : IInitializable
    {
        [Inject] private readonly IFigureFactory _figureFactory;
        [Inject] private readonly FigureData[] _figurePool;
        [Inject] private readonly TrayConfig _config;

        private readonly IFigure[] _slots = new IFigure[3];
        private readonly FigureData[] _slotData = new FigureData[3];

        public void Initialize() => GenerateTray();

        private void GenerateTray()
        {
            for (int i = 0; i < 3; i++)
            {
                var data = _figurePool[Random.Range(0, _figurePool.Length)];
                _slotData[i] = data;
                _slots[i] = _figureFactory.Create(data);
                _slots[i].Transform.position = _config.TrayPositions[i];
            }
        }

        public bool IsTrayItem(IFigure figure)
        {
            foreach (var slot in _slots)
                if (slot == figure) return true;
            return false;
        }

        public int GetSlotIndex(IFigure figure)
        {
            for (int i = 0; i < 3; i++)
                if (_slots[i] == figure) return i;
            throw new InvalidOperationException("Figure not found in tray");
        }

        public FigureData GetSlotData(int index) => _slotData[index];

        public Vector3 GetSlotPosition(int index) => _config.TrayPositions[index];

        public void BeginDrag(int index)
        {
            _slots[index].GameObject.SetActive(false);
        }

        public void CancelDrag(int index)
        {
            _slots[index].GameObject.SetActive(true);
        }

        public void ConfirmDrag(int index)
        {
            Object.Destroy(_slots[index].GameObject);
            _slots[index] = null;

            if (AllSlotsEmpty()) GenerateTray();
        }

        private bool AllSlotsEmpty()
        {
            foreach (var slot in _slots)
                if (slot != null) return false;
            return true;
        }
    }
}