using DefaultNamespace;
using SingletonScriptableObject;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BoosBar : SerializedMonoBehaviour
{
    [OdinSerialize] private Boss Boss { get; set; }
    private Image _image;
    private void Start()
    {
        _image = GetComponent<Image>();
        
        GameMaster.LevelScore
            .Where(_ => GameMaster.CurrentStage.Value == GameStage.Enemies)
            .Subscribe(levelScore =>
            {
                _image.color = Color.blue;
                _image.fillAmount = levelScore / GameMaster.PointsBossStage;
            })
            .AddTo(this);
        
        Boss.CurrentHealth
            .Where(_ => GameMaster.CurrentStage.Value == GameStage.BossBattle)
            .Subscribe(bossHealth =>
            {
                _image.color = Color.red;
                _image.fillAmount = bossHealth / (Boss.BossClass.MaxHealth * GameMaster.Level.Value);
            })
            .AddTo(this);
    }
}
