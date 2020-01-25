using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Enemy
{
    public class EnemyPathing : SerializedMonoBehaviour
    {
        private int _wayPointIndex = 0;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void StartEnemyPathing(WaveConfig WaveConfig)
        {
            _wayPointIndex = 0;
            
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
                .AddTo(_disposables);

            this.UpdateAsObservable()
                .Where(_ => IsFinishMovement())
                .Select(_ => GetComponent<Enemy>())
                .Subscribe(enemy => enemy.SelfDestroy())
                .AddTo(_disposables);
        }
        
        private void OnDisable()
        {
            _disposables.Clear();
        }
    }
}