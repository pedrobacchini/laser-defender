using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Player : SerializedMonoBehaviour
{
    [OdinSerialize] private float Speed { get; set; } = 10;

    private float _xMin, _xMax, _yMin, _yMax;

    private void Start()
    {
        SetUpMoveBoundaries();
        // Movement inputs tick on FixedUpdate
        this.FixedUpdateAsObservable()
            .Select(_ => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")))
            .Select(axis =>
            {
                var currentPosition = transform.position;
                var newXPos = Mathf.Clamp(currentPosition.x + axis.x * Time.deltaTime * Speed, _xMin, _xMax);
                var newYPos = Mathf.Clamp(currentPosition.y + axis.y * Time.deltaTime * Speed, _yMin, _yMax);
                return new Vector2(newXPos, newYPos);
            })
            .Subscribe(newPosition => transform.position = newPosition);
    }

    private void SetUpMoveBoundaries()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            throw new ArgumentException("Missing Sprite Renderer " + spriteRenderer.name);

        var gameCamera = Camera.main;
        if (gameCamera == null)
            throw new ArgumentException("Put main camera in Scene");

        var zeroPoint = gameCamera.ViewportToWorldPoint(new Vector2(0, 0));
        var onePoint = gameCamera.ViewportToWorldPoint(new Vector2(1, 1));

        var size = spriteRenderer.size / 2;

        _xMin = zeroPoint.x + size.x;
        _xMax = onePoint.x - size.x;
        _yMin = zeroPoint.y + size.y;
        _yMax = onePoint.y - size.y;
    }
}