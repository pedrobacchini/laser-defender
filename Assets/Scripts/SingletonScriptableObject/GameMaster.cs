using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SingletonScriptableObject
{
    public enum GameStage
    {
        Enemies,
        Boss
    }

    [CreateAssetMenu(fileName = "New Game Master", menuName = "Singletons/Game Master")]
    public class GameMaster : SingletonScriptableObject<GameMaster>
    {
        [OdinSerialize] private float _delayGameOverInSeconds { get; set; } = 2f;
        public static float DelayGameOverInSeconds => Instance._delayGameOverInSeconds;

        [OdinSerialize] private float _pointsBossStage { get; set; } = 10000f;
        public static float PointsBossStage => Instance._pointsBossStage;

        [OdinSerialize] [ReadOnly] private IntReactiveProperty _currentScore = new IntReactiveProperty(0);
        public static ReactiveProperty<int> CurrentScore => Instance._currentScore;

        [OdinSerialize] [ReadOnly] private GameStage _currentStage = GameStage.Enemies;

        public static void ResetGame()
        {
            Instance._currentScore.Value = 0;
        }

        public static void AddScore(int score)
        {
            Instance._currentScore.Value += score;
        }

        [UsedImplicitly]
        public void LoadStartMenu()
        {
            SceneManager.LoadScene(0);
        }

        [UsedImplicitly]
        public void LoadGame()
        {
            GameEventManager.TriggerGameStart();
            SceneManager.LoadScene("Game");
        }

        public static void LoadGameOver()
        {
            GameEventManager.TriggerStartGameOver();
            // Wait some time and load a game over scene
            Observable.Timer(TimeSpan.FromSeconds(DelayGameOverInSeconds))
                .Subscribe(_ =>
                {
                    GameEventManager.TriggerFinishGameOver();
                    SceneManager.LoadScene("Game Over");
                });
        }

        [UsedImplicitly]
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}