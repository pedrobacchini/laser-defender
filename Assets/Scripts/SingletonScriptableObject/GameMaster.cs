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
        BossBattle
    }

    [CreateAssetMenu(fileName = "New Game Master", menuName = "Singletons/Game Master")]
    public class GameMaster : SingletonScriptableObject<GameMaster>
    {
        [OdinSerialize] private int _startLevel { get; set; } = 1;
        [OdinSerialize] private float _delayGameOverInSeconds { get; set; } = 2f;
        [OdinSerialize] private float _pointsBossStage { get; set; } = 10000f;
        [OdinSerialize] private GameStage _startStage { get; set; }
        public static float PointsBossStage => Instance._pointsBossStage * Instance._level.Value;

        [OdinSerialize] [ReadOnly] private IntReactiveProperty _currentScore = new IntReactiveProperty(0);
        public static ReactiveProperty<int> CurrentScore => Instance._currentScore;
        
        [OdinSerialize] [ReadOnly] private IntReactiveProperty _levelScore = new IntReactiveProperty(0);
        public static ReactiveProperty<int> LevelScore => Instance._levelScore;

        [OdinSerialize] [ReadOnly] private ReactiveProperty<GameStage> _currentStage = new ReactiveProperty<GameStage>();
        public static ReactiveProperty<GameStage> CurrentStage => Instance._currentStage;

        [OdinSerialize] [ReadOnly] private IntReactiveProperty _level = new IntReactiveProperty();
        public static IntReactiveProperty Level => Instance._level;

        public static void ResetGame()
        {
            Instance._level.Value = Instance._startLevel;
            Instance._currentScore.Value = 0;
            Instance._levelScore.Value = 0;
            Instance._currentStage.Value = Instance._startStage;
        }

        public static void AddScore(int score)
        {
            Instance._currentScore.Value += score;
            Instance._levelScore.Value += score;
        }
        
        public static void InitBossBattle()
        {
            Instance._currentStage.Value = GameStage.BossBattle;
        }
        
        public static void FinishBossBattle()
        {
            Instance._currentStage.Value = GameStage.Enemies;
            Instance._level.Value++;
            Instance._levelScore.Value = 0;
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
            Observable.Timer(TimeSpan.FromSeconds(Instance._delayGameOverInSeconds))
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