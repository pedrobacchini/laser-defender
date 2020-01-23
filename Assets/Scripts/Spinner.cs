using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Spinner : SerializedMonoBehaviour
{
    [OdinSerialize] private float SpinnerSpeed { get; set; } = 360f;

    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => transform.Rotate(new Vector3(0, 0 ,SpinnerSpeed * Time.deltaTime )));
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer) Destroy(gameObject); 
    }
}
