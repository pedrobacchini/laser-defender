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
            .Subscribe(EnemyStageBar)
            .AddTo(this);

        Boss.EnemyHealth.CurrentHealth
            .Where(_ => GameMaster.CurrentStage.Value == GameStage.BossBattle)
            .Subscribe(BossBattleStageBar)
            .AddTo(this);
    }

    private void BossBattleStageBar(float bossHealth)
    {
        _image.color = Color.red;
        _image.fillAmount = bossHealth / Boss.MaxHealth;
    }

    private void EnemyStageBar(int levelScore)
    {
        _image.color = Color.blue;
        _image.fillAmount = levelScore / GameMaster.PointsBossStage;
    }
}