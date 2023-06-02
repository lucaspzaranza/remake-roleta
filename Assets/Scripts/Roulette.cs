using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;
using static UnityEngine.InputSystem.InputAction;

public class Roulette : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [Tooltip("The higher the value, the less the velocity will be.")]
    [SerializeField] private float _velocityRatio;
    [SerializeField] private float _decreaseRate;
    [SerializeField] private float _deadzone;
    [SerializeField] private bool _rotate;

    private bool _holdingMouse;
    private int _rotationDirection;
    private float _touchTimeCounter;
    private Vector2 _lastMousePos;
    private Vector3 _lastFrameRotation;
    private Vector3 _initalMousePos;
    private Vector3 _finalTouch;

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !_rotate)
        {
            _initalMousePos = Input.mousePosition;
            _holdingMouse = true;
        }
        else if(Input.GetMouseButton(0) && !_rotate)
        {
            SetTouchInitialPosition();
            _lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
            ResetRoulette();

        if (_holdingMouse)
        {
            _touchTimeCounter += Time.deltaTime;
            var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _lastFrameRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            var distance = transform.rotation.eulerAngles - _lastFrameRotation;
            if (distance.z > 0)
            {
                // left rotation
                _rotationDirection = 1;
            }
            else if(distance.z != 0)
            {
                // right rotation
                _rotationDirection = -1;
            }
            return;
        }       

        if (!_rotate) 
            return;

        transform.Rotate(Vector3.forward * _velocity * _rotationDirection * Time.deltaTime);

        _velocity = Mathf.Clamp(_velocity - _decreaseRate, 0f, _velocity);

        if (_velocity <= _deadzone)
            _rotate = false;
    }

    private void ResetRoulette()
    {
        _finalTouch = Input.mousePosition;
        _rotate = true;
        _holdingMouse = false;
        _velocity = GetVelocity();
        _touchTimeCounter = 0f;
    }

    private float GetVelocity()
    {
        float distance = Vector3.Distance(_finalTouch, _initalMousePos);
        // v = ΔS / Δt
        float velocity = distance / _touchTimeCounter;

        if (float.IsInfinity(velocity) || 
        velocity == float.PositiveInfinity ||
        velocity == float.NegativeInfinity)
        {
            velocity = 0f;
        }

        //print($"The velocity is: {velocity}.");
        return velocity / _velocityRatio;
    }

    private void SetTouchInitialPosition()
    {
        float distance = Vector2.Distance(Input.mousePosition, _lastMousePos);

        if(distance == 0f)
        {
            print($"Reseting mouse Initial Pos");
            _initalMousePos = Input.mousePosition;
            _touchTimeCounter = 0f;
        }
    }
}
