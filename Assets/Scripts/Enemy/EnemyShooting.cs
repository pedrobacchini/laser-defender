using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyShooting : SerializedMonoBehaviour
    {
        [Title("Shooting")]
        [ReadOnly]
        [OdinSerialize]
        private float ShotCounter { get; set; }

        [OdinSerialize] private GameObject ShootPrefab { get; set; }
        [OdinSerialize] private float ShootSpeed { get; set; } = 8;
        [MinMaxSlider(0, 5)] [OdinSerialize] private Vector2 TimeBetweenShoots { get; set; }
        
        [Title("Sound Effects")]
        [OdinSerialize]
        private AudioClip ShootingSound { get; set; }

        [MinMaxSlider(0, 1)] [OdinSerialize] private Vector2 ShootingVolume { get; set; }
        
        private Camera _mainCamera;
        
        private void Start()
        {
            _mainCamera = Camera.main;
            ShotCounter = Random.Range(TimeBetweenShoots.x, TimeBetweenShoots.y);
            this.UpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(ShotCounter))
                .Subscribe(_ => Shoot())
                .AddTo(this);
        }
        
        private void Shoot()
        {
            var position = transform.position;
            var laser = Instantiate(ShootPrefab, new Vector2(position.x, position.y - 1), Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -ShootSpeed);
            AudioSource.PlayClipAtPoint(ShootingSound, _mainCamera.transform.position,
                Random.Range(ShootingVolume.x, ShootingVolume.y));
        }
    }
}