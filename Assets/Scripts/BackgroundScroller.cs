using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BackgroundScroller : SerializedMonoBehaviour
{
    [OdinSerialize] private float BackgroundSpeed { get; set; } = 0.4f;

    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        
        var offset = new Vector2(0, BackgroundSpeed);

        this.UpdateAsObservable()
            .Subscribe(_ => _material.mainTextureOffset += offset * Time.deltaTime)
            .AddTo(this);
    }
}