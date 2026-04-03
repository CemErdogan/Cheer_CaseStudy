using System;
using System.Collections.Generic;
using Abstractions.GridSystem;
using Abstractions.MatchSystem;
using Cysharp.Threading.Tasks;
using Game.FigureSystem.Runtime;
using UnityEngine;
using Zenject;

namespace Game.MatchSystem.Runtime
{
    public class FlowManager : IFlowManager, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus          _signalBus;
        [Inject] private readonly IGridManager       _gridManager;
        [Inject] private readonly MatchFinder        _matchFinder;
        [Inject] private readonly DestroyAnimator    _destroyAnimator;
        [Inject] private readonly ExpandHandler      _expandHandler;
        [Inject] private readonly BigSquareHandler   _bigSquareHandler;
        [Inject] private readonly GravityHandler     _gravityHandler;
        [Inject] private readonly Point.Factory      _pointFactory;
        [Inject] private readonly ConnectionDatabase _connectionDb;

        public void Initialize()
        {
            _signalBus.Subscribe<FigurePlacedSignal>(OnFigurePlaced);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<FigurePlacedSignal>(OnFigurePlaced);
        }

        private void OnFigurePlaced(FigurePlacedSignal signal)
        {
            StartFlow().Forget();
        }

        private async UniTask StartFlow()
        {
            bool hadMatches;

            do
            {
                var allFigures  = new List<Abstractions.FigureSystem.IFigure>(_gridManager.GetAllFigures());
                var destroyList = _matchFinder.FindPointsToDestroy(allFigures, _gridManager);

                hadMatches = destroyList.Count > 0;
                if (!hadMatches) break;

                await _destroyAnimator.AnimateAndDestroy(destroyList, OnPointDestroyed);

                await UniTask.Yield();

                var liveFigures = new List<Abstractions.FigureSystem.IFigure>(_gridManager.GetAllFigures());
                foreach (var figure in liveFigures)
                {
                    _expandHandler.CheckExpand(figure, _pointFactory, _connectionDb);
                    _bigSquareHandler.TryMakeBigSquare(figure, _pointFactory, _connectionDb);

                    if (figure.IsEmpty())
                    {
                        _gridManager.RemoveFigure(figure.GridCoord);
                        UnityEngine.Object.Destroy(figure.GameObject);
                    }
                }

                await _gravityHandler.ApplyGravity(_gridManager);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            } while (hadMatches);

            CheckWinCondition();
            _signalBus.Fire(new FlowCompletedSignal());
        }

        private void OnPointDestroyed(Abstractions.FigureSystem.IPoint point)
        {
            point?.ParentFigure?.RemovePoint(point.SlotPos);
        }

        private void CheckWinCondition()
        {
            foreach (var figure in _gridManager.GetAllFigures())
            {
                if (!figure.IsEmpty()) return;
            }
            _signalBus.Fire(new GridClearedSignal());
        }
    }
}
