using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;

namespace SingletonScriptableObject
{
    [CreateAssetMenu(fileName = "New Game Session", menuName = "Managers/Game Session")]
    public class GameSession : SingletonScriptableObject<GameSession>
    {
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
