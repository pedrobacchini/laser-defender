public static class GameEventManager
{
    public delegate void GameEvent();

    public static event GameEvent GameStart, BossBattle, StartGameOver, FinishGameOver;

    #region Event methods
    public static void TriggerGameStart()
    {
        GameStart?.Invoke();
    }
    
    public static void TriggerBossBattle()
    {
        BossBattle?.Invoke();
    }

    public static void TriggerStartGameOver()
    {
        StartGameOver?.Invoke();
    }
    
    public static void TriggerFinishGameOver()
    {
        FinishGameOver?.Invoke();
    }
    #endregion
}