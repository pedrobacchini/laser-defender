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
        public void StartEnemyPathing(List<Transform> wayPoints, float moveSpeed, GameObject gameObject,
            Action selfDestroy)
        {
            wayPointIndex = 0;

            gameObject.transform.position = wayPoints[0].position;

            bool IsFinishMovement() => wayPointIndex + 1 >= wayPoints.Count;

            gameObject.UpdateAsObservable()
                .Where(_ => !IsFinishMovement())
                .Select(_ => GetNextPosition(gameObject, wayPoints))
                .Subscribe(nextPosition => GoToNextPosition(gameObject, moveSpeed, nextPosition))
                .AddTo(_disposables);

            gameObject.UpdateAsObservable()
                .Where(_ => IsFinishMovement())
                .Subscribe(_ => selfDestroy())
                .AddTo(_disposables);

            gameObject.OnDisableAsObservable()
                .Subscribe(_ => _disposables.Clear());
        }

        private Vector3 GetNextPosition(GameObject gameObject, List<Transform> wayPoints)
        {
            if (gameObject.transform.position.Equals(wayPoints[wayPointIndex + 1].position))
                wayPointIndex++;
            return wayPoints[wayPointIndex + 1].position;
        }

        private static void GoToNextPosition(GameObject gameObject, float moveSpeed, Vector3 nextPosition)
        {
            var movementThisFrame = moveSpeed * Time.deltaTime;
            gameObject.transform.position =
                Vector2.MoveTowards(gameObject.transform.position, nextPosition, movementThisFrame);
        }
    }
}