using TMPro;
using UniRx;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    private void Start()
    {
        var textMeshProUgui = GetComponent<TextMeshProUGUI>();
        GameSession.CurrentScore.Subscribe(currentScore => textMeshProUgui.text = currentScore.ToString()).AddTo(this);
    }
}