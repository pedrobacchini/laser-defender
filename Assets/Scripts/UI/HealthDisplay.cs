using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UniRx;

namespace UI
{
    public class HealthDisplay : SerializedMonoBehaviour
    {
        [OdinSerialize] private Player.Player Player { get; set; }
        
        private void Start()
        {
            var textMeshProUgui = GetComponent<TextMeshProUGUI>();
            Player.CurrentHealth.Subscribe(health => textMeshProUgui.text = health.ToString());
        }
    }
}
