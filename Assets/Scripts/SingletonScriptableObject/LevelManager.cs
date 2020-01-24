using System;
using JetBrains.Annotations;
using SingletonScriptableObject;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenario
{
    [CreateAssetMenu(fileName = "New Level Manager", menuName = "Managers/Level Manager")]
    public class LevelManager : SingletonScriptableObject<LevelManager>
    {
        [UsedImplicitly]
        public void LoadStartMenu()
        {
            SceneManager.LoadScene(0);
        }

        [UsedImplicitly]
        public void LoadGame()
        {
            SceneManager.LoadScene("Game");
        }

        public static void LoadGameOver()
        {
            // Wait some time and load a game over scene
            Observable.Timer(TimeSpan.FromSeconds(GameMaster.DelayGameOverInSeconds))
                .Subscribe(_ => SceneManager.LoadScene("Game Over"));
        }

        [UsedImplicitly]
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}