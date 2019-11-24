using UnityEngine;

public class ResetGameSession : MonoBehaviour
{
    private void Start()
    {
        GameSession.ResetGame();
    }
}
