using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class DamageDealer : SerializedMonoBehaviour
{
    [OdinSerialize] public int Damage { get; private set; } = 100;
    [OdinSerialize] private bool isDestructible { get; set; } = true;

    public void Hit()
    {
        if(isDestructible) gameObject.SetActive(false);
    }
}
