using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Enemy
{
    public class EnemyPathing : SerializedMonoBehaviour
    {
        public WaveConfig WaveConfig { get; set; }

        private int _wayPointIndex = 0;

        private void Start()
        {
            var wayPoints = WaveConfig.WaveWayPoints;

            transform.position = wayPoints[0].position;

            bool IsFinishMovement() => _wayPointIndex + 1 >= wayPoints.Count;

            this.UpdateAsObservable()
                .Where(_ => !IsFinishMovement())
                .Select	(_ => wayPoints[_wayPointIndex + 1].position)
                .Subscribe(nextPosition =>
                {
                    var movementThisFrame = WaveConfig.MoveSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, nextPosition, movementThisFrame);
                    if (transform.position.Equals(wayPoints[_wayPointIndex + 1].position)) _wayPointIndex++;
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => IsFinishMovement())
                .Select(_ => GetComponent<Enemy>())
                .Subscribe(enemy => enemy.SelfDestroy())
                .AddTo(this);
        }
    }
}