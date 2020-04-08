using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 finishPos = Vector3.zero; // целевое положение
    public float speed = 0.5f;

    private Vector3 _startPos;
    private float _trackPercent = 0; // Насколько далеко наше положение от старта до финиша
    private int _direction = 1; // направление движения в данный момент
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position; // положени е в сцене - это точка, от которой начинается движение
    }

    // Update is called once per frame
    void Update()
    {
        _trackPercent += _direction * speed * Time.deltaTime;
        float x = (finishPos.x - _startPos.x) * _trackPercent + _startPos.x;
        float y = (finishPos.y - _startPos.y) * _trackPercent + _startPos.y;
        transform.position = new Vector3(x, y, _startPos.z);

        if ((_direction == 1 && _trackPercent > .9f) ||
            (_direction == -1 && _trackPercent < .1f))
        {
            _direction *= -1; // меняем направление как в начале, так и в конце
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finishPos);
    }
}
