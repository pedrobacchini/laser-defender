using System;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level Manager", menuName = "Managers/Level Manager")]
public class LevelManager : SingletonScriptableObject<LevelManager>
{
    [OdinSerialize] private float DelayGameOverInSeconds { get; set; } = 2f;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadGameOver()
    {
        // Wait some time and load a game over scene
        Observable.Timer(TimeSpan.FromSeconds(DelayGameOverInSeconds))
            .Subscribe(_ => SceneManager.LoadScene("Game Over"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}