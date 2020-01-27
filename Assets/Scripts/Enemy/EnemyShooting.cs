using System;
using GameSystem.ObjectPool;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyShooting
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void StartEnemyShooting(EnemyClass enemyClass, GameObject gameObject)
        {
            var shotCounter = Random.Range(enemyClass.TimeBetweenShoots.x, enemyClass.TimeBetweenShoots.y);
            gameObject.UpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(shotCounter))
                .Subscribe(_ => Shoot(enemyClass, gameObject, Camera.main.transform.position))
                .AddTo(_disposables);

            gameObject.OnDisableAsObservable()
                .Subscribe(_ => _disposables.Clear());
        }

        private static void Shoot(EnemyClass enemyClass, GameObject gameObject, Vector3 shootingSoundPosition)
        {
            var position = gameObject.transform.position;
            var laser = ObjectPooler.SpawnFromPool(enemyClass.ShootPrefabTag, new Vector2(position.x, position.y - 1),
                Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyClass.ShootSpeed);
            AudioSource.PlayClipAtPoint(enemyClass.ShootingSound, shootingSoundPosition,
                Random.Range(enemyClass.ShootingVolume.x, enemyClass.ShootingVolume.y));
        }
    }
}