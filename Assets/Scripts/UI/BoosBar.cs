using SingletonScriptableObject;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BoosBar : MonoBehaviour
{
    private Image _image;
    private void Start()
    {
        _image = GetComponent<Image>();
        
        GameSession.CurrentScore
            .Subscribe(currentScore => _image.fillAmount = currentScore / 10000.0f)
            .AddTo(this);
    }
}
