using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.FigureSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.MatchSystem.Runtime
{
    public class DestroyAnimator
    {
        private const float DestroyDuration = 0.4f;

        public async UniTask AnimateAndDestroy(List<IPoint> points, Action<IPoint> onPointDestroyed)
        {
            if (points.Count == 0) return;

            var center  = CalculateCenter(points);
            var pending = points.Count;
            var tcs     = new UniTaskCompletionSource();

            foreach (var point in points)
            {
                point.AnimateDestroy(center, DestroyDuration, () =>
                {
                    onPointDestroyed?.Invoke(point);
                    pending--;
                    if (pending == 0)
                        tcs.TrySetResult();
                });
            }

            await tcs.Task;
        }

        private Vector3 CalculateCenter(List<IPoint> points)
        {
            var sum = points.Aggregate(Vector3.zero, (acc, p) => acc + p.Transform.position);
            return sum / points.Count;
        }
    }
}
