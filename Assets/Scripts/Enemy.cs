using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class Enemy : SerializedMonoBehaviour
{
    [OdinSerialize] private float Health { get; set; } = 100f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer)
        {
            Health -= damageDealer.Damage;
            damageDealer.Hit();
        }
    }
}
