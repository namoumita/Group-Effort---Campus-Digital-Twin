
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float transitionDuration = 2.5f; // Adjust for desired speed

    private float _time;
    private bool _moving = false;

    private void Start()
    {
        _time = 0;
        _moving = true;
    }

    private void Update()
    {
        if (_moving)
        {
            _time += Time.deltaTime / transitionDuration;
            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, _time);

            if (_time >= 1.0f)
            {
                _moving = false;
            }
        }
    }
}