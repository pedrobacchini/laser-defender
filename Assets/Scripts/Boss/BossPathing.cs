using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace DefaultNamespace
{
    public class BossPathing
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private int wayPointIndex;

        public void StartBossPathing(BossClass bossClass, GameObject gameObject)
        {
            wayPointIndex = 0;

            gameObject.transform.position = bossClass.BossWayPoints[0].position;

            gameObject.UpdateAsObservable()
                .Select(_ => GetNextPosition(gameObject, bossClass, bossClass.BossWayPoints))
                .Subscribe(nextPosition => GoToNextPosition(gameObject, bossClass.MoveSpeed, nextPosition))
                .AddTo(_disposables);

            gameObject.OnDisableAsObservable()
                .Subscribe(_ => _disposables.Clear());
        }

        private Vector3 GetNextPosition(GameObject gameObject, BossClass bossClass, IReadOnlyList<Transform> wayPoints)
        {
            if (gameObject.transform.position.Equals(wayPoints[wayPointIndex].position))
            {
                wayPointIndex++;
                if (wayPointIndex == wayPoints.Count)
                    wayPointIndex = bossClass.StartLoopPath;
            }

            return bossClass.BossWayPoints[wayPointIndex].position;
        }

        private static void GoToNextPosition(GameObject gameObject, float moveSpeed, Vector3 nextPosition)
        {
            var movementThisFrame = moveSpeed * Time.deltaTime;
            gameObject.transform.position =
                Vector2.MoveTowards(gameObject.transform.position, nextPosition, movementThisFrame);
        }
    }
}