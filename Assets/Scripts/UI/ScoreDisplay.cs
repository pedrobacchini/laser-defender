using SingletonScriptableObject;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        private void Start()
        {
            var textMeshProUgui = GetComponent<TextMeshProUGUI>();
            GameSession.CurrentScore.Subscribe(currentScore => textMeshProUgui.text = currentScore.ToString()).AddTo(this);
        }
    }
}