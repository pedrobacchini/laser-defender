using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;

namespace SingletonScriptableObject
{
    [CreateAssetMenu(fileName = "New Game Master", menuName = "Managers/Game Master")]
    public class GameMaster : SingletonScriptableObject<GameMaster>
    {
        [OdinSerialize] private float _delayGameOverInSeconds { get; set; } = 2f;
        public static float DelayGameOverInSeconds => Instance._delayGameOverInSeconds;

        [OdinSerialize] private float _pointsBossStage { get; set; } = 10000f;
        public static float PointsBossStage => Instance._pointsBossStage;

        [OdinSerialize] [ReadOnly] private IntReactiveProperty _currentScore = new IntReactiveProperty(0);
        public static ReactiveProperty<int> CurrentScore => Instance._currentScore;

        [OdinSerialize] [ReadOnly] private IntReactiveProperty _currentScore = new IntReactiveProperty(0);

        public static void ResetGame()
        {
            Instance._currentScore.Value = 0;
        }

        public static void AddScore(int score)
        {
            Instance._currentScore.Value += score;
        }
    }
}