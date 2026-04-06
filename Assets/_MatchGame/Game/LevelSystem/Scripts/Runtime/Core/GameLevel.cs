using System.Collections.Generic;
using System.Linq;
using Abstractions.CameraSystem;
using Abstractions.FigureSystem;
using Abstractions.GridSystem;
using Abstractions.LevelSystem;
using Common.Util;
using UnityEngine;
using Zenject;

namespace Game.LevelSystem.Runtime
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Game/LevelSystem/Level/GameLevel", order = 0)]
    public class GameLevel : ScriptableObject, ILevel
    {
        [field: SerializeField] public GoalData[] GoalData { get; private set; }
        [field: SerializeField] public Vector2Int GridSize { get; private set; }
        [field: SerializeField] public FigureData[] InitialFigures { get; private set; }

        [Inject] private IFigureFactory _figureFactory;
        [Inject] private IGridManager _gridManager;
        [Inject] private SignalBus _signalBus;

        public void Load()
        {
            _gridManager.SetGridSize(GridSize.x, GridSize.y);

            if (InitialFigures == null) return;

            var figures = new List<Transform>();
            foreach (var figureData in InitialFigures)
            {
                var figure   = _figureFactory.Create(figureData);
                var worldPos = figureData.GridCoord.ToPosition();
                figure.Transform.position = worldPos;
                _gridManager.PlaceFigure(figure, figureData.GridCoord);
                figures.Add(figure.Transform);
            }
            
            _signalBus.AbstractFire(new SetupCameraSignal(figures.ToArray()));
        }

        public void Unload()
        {
            foreach (var figure in _gridManager.GetAllFigures())
            {
                if (figure?.GameObject != null)
                    Destroy(figure.GameObject);
            }
        }
    }
}
