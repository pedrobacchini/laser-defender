using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class EnemyPathing : SerializedMonoBehaviour
{
    [OdinSerialize] private List<Transform> WayPoints { get; set; }
}
