using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Roulette : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _forceOffset;
    [SerializeField] private float _decreaseRate;
    [SerializeField] private float _decreaseRateOffset;
    [SerializeField] private float _deadzone;

    private bool _rotate;
    private float _initialForce;
    private float _initDecreaseRate;
    private bool _holdingMouse;

    private void Start()
    {
        _initialForce = _force;
        _initDecreaseRate = _decreaseRate;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !_rotate)
            _holdingMouse = true;

        if (Input.GetMouseButtonUp(0))
            ResetRoulette();

        if (_holdingMouse)
        {
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            return;
        }

        if (!_rotate) 
            return;

        transform.Rotate(Vector3.forward * _force * Time.deltaTime);

        _force = Mathf.Clamp(_force - _decreaseRate, 0f, _force);

        if (_force <= _deadzone)
            _rotate = false;
    }

    private void ResetRoulette()
    {
        _rotate = true;
        _holdingMouse = false;
        _force = Random.Range(_initialForce - _forceOffset, _initialForce + _forceOffset);
        _decreaseRate = Random.Range(_initDecreaseRate - _decreaseRateOffset, _initDecreaseRate + _decreaseRateOffset);
    }
}
