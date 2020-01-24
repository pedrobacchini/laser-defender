using Enemy;
using UnityEngine;

namespace SingletonScriptableObject
{
    public class ResetSingletonScriptableObject : MonoBehaviour
    {
        private void Start()
        {
            GameMaster.ResetGame();
            EnemyRuntimeSet.Clear();
        }
    }
}