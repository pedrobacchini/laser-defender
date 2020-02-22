public static class GameEventManager
{
    public delegate void GameEvent();

    public static event GameEvent GameStart, StartGameOver, FinishGameOver;

    #region Event methods
    public static void TriggerGameStart()
    {
        GameStart?.Invoke();
    }

    public static void TriggerBeginGameOver()
    {
        StartGameOver?.Invoke();
    }
    
    public static void TriggerFinishGameOver()
    {
        FinishGameOver?.Invoke();
    }
    #endregion
}