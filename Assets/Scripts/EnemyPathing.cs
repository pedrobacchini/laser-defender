using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class EnemyPathing : SerializedMonoBehaviour
{
    [OdinSerialize] private List<Transform> WayPoints { get; set; }

    [OdinSerialize] private float MoveSpeed { get; set; } = 2f;

    private int _wayPointIndex = 0;

    private void Start()
    {
        transform.position = WayPoints[0].position;

        bool IsFinishMovement() => _wayPointIndex + 1 >= WayPoints.Count;

        this.UpdateAsObservable()
            .Where(_ => !IsFinishMovement())
            .Subscribe(_ =>
            {
                var targetPosition = WayPoints[_wayPointIndex + 1].position;
                var movementThisFrame = MoveSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
                if (transform.position.Equals(WayPoints[_wayPointIndex + 1].position)) _wayPointIndex++;
            });

        this.UpdateAsObservable()
            .Where(_ => IsFinishMovement())
            .Subscribe(_ => Destroy(gameObject));
    }
}