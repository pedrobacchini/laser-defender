using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class DamageDealer : SerializedMonoBehaviour
{
    [OdinSerialize] public int Damage { get; private set; } = 100;

    public void Hit()
    {
        gameObject.SetActive(false);
    }
}
