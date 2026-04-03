using System;
using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.FigureSystem.Runtime
{
    public class TrayManager : IInitializable
    {
        [Inject] private readonly FigureSelected.Factory _factory;
        [Inject] private readonly FigureData[]           _figurePool;
        [Inject] private readonly TrayConfig             _config;

        private readonly FigureSelected[] _slots    = new FigureSelected[3];
        private readonly FigureData[]     _slotData = new FigureData[3];

        public void Initialize() => GenerateTray();

        private void GenerateTray()
        {
            for (int i = 0; i < 3; i++)
            {
                var data = _figurePool[Random.Range(0, _figurePool.Length)];
                _slotData[i]             = data;
                _slots[i]                = _factory.Create(data);
                _slots[i].transform.position = _config.TrayPositions[i];
            }
        }

        public FigureData GetData(FigureSelected source)
        {
            for (int i = 0; i < 3; i++)
                if (_slots[i] == source) return _slotData[i];
            throw new System.InvalidOperationException("FigureSelected not found in tray");
        }

        public Vector3 GetTrayPosition(FigureSelected source)
        {
            for (int i = 0; i < 3; i++)
                if (_slots[i] == source) return _config.TrayPositions[i];
            return Vector3.zero;
        }

        public void OnFigurePlaced(FigureSelected source)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_slots[i] != source) continue;
                _slots[i] = null;
                break;
            }

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
