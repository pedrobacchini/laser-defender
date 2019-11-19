using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class Enemy : SerializedMonoBehaviour
{
    [OdinSerialize] private float Health { get; set; } = 100f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer) ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();
        Health -= damageDealer.Damage;
        if (Health <= 0) Destroy(gameObject);
    }
}
