using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

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
            .Subscribe(_ =>
            {
                var targetPosition = wayPoints[_wayPointIndex + 1].position;
                var movementThisFrame = WaveConfig.MoveSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
                if (transform.position.Equals(wayPoints[_wayPointIndex + 1].position)) _wayPointIndex++;
            });

        this.UpdateAsObservable()
            .Where(_ => IsFinishMovement())
            .Subscribe(_ => Destroy(gameObject));
    }
}