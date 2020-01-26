using SingletonScriptableObject;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UniRx;

namespace UI
{
    public class LevelDisplay: SerializedMonoBehaviour
    {
        [OdinSerialize] private string StartText = "LV:";
        private void Start()
        {
            var textMeshProUgui = GetComponent<TextMeshProUGUI>();
            GameMaster.Level.Subscribe(level => textMeshProUgui.text = StartText+level).AddTo(this);
        }
    }
}