using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class Player : SerializedMonoBehaviour
{
    [OdinSerialize] private float Speed { get; set; } = 10;

    private float _xMin, _xMax, _yMin, _yMax;

    public IObservable<Vector2> Movement { get; private set; }

    private void Start()
    {
        SetUpMoveBoundaries();
        // Movement inputs tick on FixedUpdate
//        Movement = this.FixedUpdateAsObservable()
//            .Select(_ => {
//                var x = Input.GetAxis("Horizontal");
//                var y = Input.GetAxis("Vertical");
//                return new Vector2(x, y).normalized;
//            });
    }

    private void SetUpMoveBoundaries()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
            throw new ArgumentException("Missing Sprite Renderer "+ spriteRenderer.name);

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

        Debug.Log(_xMin + " " + _xMax);
        Debug.Log(_yMin + " " + _yMax);
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var position = transform.position;

        var deltaX = Input.GetAxis("Horizontal");
        var deltaY = Input.GetAxis("Vertical");

        var newXPos = Mathf.Clamp(position.x + deltaX * Time.deltaTime * Speed, _xMin, _xMax);
        var newYPos = Mathf.Clamp(position.y + deltaY * Time.deltaTime * Speed, _yMin, _yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }
}