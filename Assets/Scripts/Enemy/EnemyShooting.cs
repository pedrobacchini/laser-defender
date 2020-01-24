using System;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyShooting : SerializedMonoBehaviour
    {
        public EnemyClass EnemyClass { private get; set; }
        
        private Camera _mainCamera;
        private float _shotCounter;
        
        private void Start()
        {
            _mainCamera = Camera.main;
            _shotCounter = Random.Range(EnemyClass.TimeBetweenShoots.x, EnemyClass.TimeBetweenShoots.y);
            this.UpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_shotCounter))
                .Subscribe(_ => Shoot())
                .AddTo(this);
        }
        
        private void Shoot()
        {
            var position = transform.position;
            var laser = Instantiate(EnemyClass.ShootPrefab, new Vector2(position.x, position.y - 1), Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -EnemyClass.ShootSpeed);
            AudioSource.PlayClipAtPoint(EnemyClass.ShootingSound, _mainCamera.transform.position,
                Random.Range(EnemyClass.ShootingVolume.x, EnemyClass.ShootingVolume.y));
        }
    }
}