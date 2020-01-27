using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Enemy
{
    public class EnemyPathing
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private int wayPointIndex;

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public void StartEnemyPathing(WaveConfig WaveConfig, GameObject gameObject, Action selfDestroy)
        {
            wayPointIndex = 0;

            var wayPoints = WaveConfig.WaveWayPoints;

            gameObject.transform.position = wayPoints[0].position;

            bool IsFinishMovement() => wayPointIndex + 1 >= wayPoints.Count;

            gameObject.UpdateAsObservable()
                .Where(_ => !IsFinishMovement())
                .Select(_ => wayPoints[wayPointIndex + 1].position)
                .Subscribe(nextPosition => GoToNextPosition(WaveConfig, gameObject, nextPosition, wayPoints))
                .AddTo(_disposables);

            gameObject.UpdateAsObservable()
                .Where(_ => IsFinishMovement())
                .Subscribe(_ => selfDestroy())
                .AddTo(_disposables);

            gameObject.OnDisableAsObservable()
                .Subscribe(_ => _disposables.Clear());
        }

        private void GoToNextPosition(WaveConfig WaveConfig, GameObject gameObject, Vector3 nextPosition,
            IReadOnlyList<Transform> wayPoints)
        {
            var movementThisFrame = WaveConfig.MoveSpeed * Time.deltaTime;
            gameObject.transform.position =
                Vector2.MoveTowards(gameObject.transform.position, nextPosition, movementThisFrame);
            if (gameObject.transform.position.Equals(wayPoints[wayPointIndex + 1].position)) wayPointIndex++;
        }
    }
}