using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform _followTarget;
    [SerializeField] float _smoothTime = 0.3f;
    [SerializeField] bool _physicsUpdate;
    private Vector3 _startPosition;
    private Vector3 _velocity;
    private void Start()
    {
        _startPosition = transform.position;
    }
    private void Update()
    {
        if (_physicsUpdate) { return; }
        var targetPosition = Vector3.SmoothDamp(transform.position, _followTarget.position, ref _velocity, _smoothTime);
        targetPosition.z = _startPosition.z;
        transform.position = targetPosition;
    }
    private void FixedUpdate()
    {
        if (!_physicsUpdate) { return; }
        var targetPosition = Vector3.SmoothDamp(transform.position, _followTarget.position, ref _velocity, _smoothTime);
        targetPosition.z = _startPosition.z;
        transform.position = targetPosition;
    }
}
