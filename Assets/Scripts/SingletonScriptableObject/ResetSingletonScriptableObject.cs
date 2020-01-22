using Enemy;
using UnityEngine;

namespace SingletonScriptableObject
{
    public class ResetSingletonScriptableObject : MonoBehaviour
    {
        private void Start()
        {
            GameSession.ResetGame();
            EnemyRuntimeSet.Clear();
        }
    }
}