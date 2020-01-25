using System;
using GameSystem.ObjectPool;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyShooting : SerializedMonoBehaviour
    {
        private EnemyClass _enemyClass;
        private Camera _mainCamera;
        private float _shotCounter;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void StartEnemyShooting(EnemyClass enemyClass)
        {
            _mainCamera = Camera.main;
            _enemyClass = enemyClass;
            _shotCounter = Random.Range(_enemyClass.TimeBetweenShoots.x, _enemyClass.TimeBetweenShoots.y);
            this.UpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_shotCounter))
                .Subscribe(_ => Shoot())
                .AddTo(_disposables);
        }
        
        private void Shoot()
        {
            var position = transform.position;
            var laser = ObjectPooler.SpawnFromPool(_enemyClass.ShootPrefabTag, new Vector2(position.x, position.y - 1),
                Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -_enemyClass.ShootSpeed);
            AudioSource.PlayClipAtPoint(_enemyClass.ShootingSound, _mainCamera.transform.position,
                Random.Range(_enemyClass.ShootingVolume.x, _enemyClass.ShootingVolume.y));
        }

        private void OnDisable()
        {
            _disposables.Clear();
        }
    }
}