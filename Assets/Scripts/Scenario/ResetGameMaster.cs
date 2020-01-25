using UnityEngine;

namespace SingletonScriptableObject
{
    public class ResetGameMaster : MonoBehaviour
    {
        private void Start()
        {
            GameMaster.ResetGame();
        }
    }
}