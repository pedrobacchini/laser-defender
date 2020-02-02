using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyBase : SerializedMonoBehaviour
    {
        protected void DeathEffect(GameObject deathPrefab, float durationOfDeathEffect, AudioClip deathSound,
            Vector2 deathSoundVolume)
        {
            var explosion = Instantiate(deathPrefab, transform.position, transform.rotation);
            Destroy(explosion, durationOfDeathEffect);
            var volume = Random.Range(deathSoundVolume.x, deathSoundVolume.y);
            AudioSource.PlayClipAtPoint(deathSound, Vector3.zero, volume);
        }
    }
}