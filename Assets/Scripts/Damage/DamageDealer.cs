using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class DamageDealer : SerializedMonoBehaviour
{
    [OdinSerialize] public int Damage { get; private set; } = 100;
    [OdinSerialize] private bool isDestructible { get; set; } = true;
    [OdinSerialize] private GameObject ImpactVFX = null;

    public void Hit()
    {
        if (ImpactVFX)
        {
            var transformPosition = transform.position;
            Instantiate(ImpactVFX, new Vector2(transformPosition.x, transformPosition.y-0.5f), Quaternion.identity);   
        }
        if (isDestructible) gameObject.SetActive(false);
    }
}