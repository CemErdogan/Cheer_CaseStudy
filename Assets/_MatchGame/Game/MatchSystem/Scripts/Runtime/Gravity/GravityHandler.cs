using System.Collections.Generic;
using Abstractions.GridSystem;
using Common.Util;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.MatchSystem.Runtime
{
    public class GravityHandler
    {
        private const float FallDuration = 0.25f;

        public async UniTask ApplyGravity(IGridManager grid)
        {
            var tasks = new List<UniTask>();
            for (int col = 0; col < grid.ColumnCount; col++)
            {
                tasks.Add(ProcessColumn(grid, col));
            }
            await UniTask.WhenAll(tasks);
        }

        private async UniTask ProcessColumn(IGridManager grid, int col)
        {
            for (int row = 0; row < grid.RowCount - 1; row++)
            {
                var emptyCoord = new Vector2Int(col, row);
                if (grid.IsOccupied(emptyCoord)) continue;

                for (int aboveRow = row + 1; aboveRow < grid.RowCount; aboveRow++)
                {
                    var aboveCoord = new Vector2Int(col, aboveRow);
                    var figure = grid.GetFigureAt(aboveCoord);
                    if (figure == null) continue;

                    grid.RemoveFigure(aboveCoord);
                    grid.PlaceFigure(figure, emptyCoord);
                    figure.GridCoord = emptyCoord;

                    var targetPos = emptyCoord.ToPosition();
                    await AnimateFall(figure.Transform, targetPos);
                    break;
                }
            }
        }

        private UniTask AnimateFall(Transform t, Vector3 target)
        {
            var tcs = new UniTaskCompletionSource();
            t.DOMove(target, FallDuration)
             .SetEase(Ease.InQuad)
             .OnComplete(() => tcs.TrySetResult());
            return tcs.Task;
        }
    }
}
