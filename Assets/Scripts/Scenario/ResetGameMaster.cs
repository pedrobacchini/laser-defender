using UnityEngine;

namespace SingletonScriptableObject
{
    public class ResetGameMaster : MonoBehaviour
    {
        private void Awake()
        {
            GameMaster.ResetGame();
        }
    }
}