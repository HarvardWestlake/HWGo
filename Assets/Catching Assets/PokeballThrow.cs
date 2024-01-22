using System.Collections;
using UnityEngine;

public class PokeballThrow : MonoBehaviour
{
    [SerializeField]
    private float _throwSpeed = 35f;

    private float _speed;
    private float _lastMouseX, _lastMouseY;

    [SerializeField]
    private bool _thrown, _holding, _curve;

    private Rigidbody _rigidbody;
    private Vector3 _newPosition;

    [SerializeField]
    private float _curveAmount = 0f, _curveSpeed = 2f, _minCurveAmountToCurveBall = 1f, _maxCurveAmount = 2.5f;

    private Rect _circlingBox;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.maxAngularVelocity = _curveAmount * 8f;
        _circlingBox = new Rect(Screen.width / 2, Screen.height / 2, 0f, 0f);

        Reset();
    }

    private void Update()
    {
        if (_holding)
            OnTouch();

        _curve = (Mathf.Abs(_curveAmount) > _minCurveAmountToCurveBall);

        if (_curve && _thrown)
        {
            Vector3 direction = Vector3.right;
            direction = Camera.main.transform.InverseTransformDirection(direction);

            _rigidbody.AddForce(direction * _curveAmount * Time.deltaTime, ForceMode.Impulse);
        }

        _rigidbody.maxAngularVelocity = _curveAmount * 8f;
        _rigidbody.angularVelocity = transform.forward * _curveAmount * 8f + _rigidbody.angularVelocity;

        if (_thrown)
            return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Start of a new touch.
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f))
                    {
                        if (hit.transform == transform)
                        {
                            _holding = true;
                            transform.SetParent(null);
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    // Update the last touch position for curve calculation.
                    _lastMouseX = touch.position.x;
                    _lastMouseY = touch.position.y;
                    break;

                case TouchPhase.Ended:
                    // Swipe ended, now throw the ball.
                    Vector2 swipeVelocity = (touch.position - touch.deltaPosition) / touch.deltaTime;
                    ThrowBall(swipeVelocity);
                    break;
            }
        }
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(5.0f);
        Reset();
    }


    public void Reset()
    {
        _curveAmount = 0f;
        CancelInvoke();
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.35f, Camera.main.nearClipPlane * 30f));
        _newPosition = transform.position;
        _thrown = _holding = false;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.Sleep();

        transform.localRotation = Quaternion.Euler(0f, 200f, 0f);
        transform.SetParent(Camera.main.transform);
    }

    private void OnTouch()
    {
        if (Input.touchCount > 0)
        {
            CalcCurveAmount();

            Vector3 mousePos = Input.GetTouch(0).position;
            mousePos.z = Camera.main.nearClipPlane * 30f;

            _newPosition = Camera.main.ScreenToWorldPoint(mousePos);

            transform.localPosition = Vector3.Lerp(transform.localPosition, _newPosition, 50f * Time.deltaTime);
        }
    }

    private void CalcCurveAmount()
    {
        if (Input.touchCount > 0)
        {
            Vector2 b = new Vector2(_lastMouseX, _lastMouseY);
            Vector2 c = Input.GetTouch(0).position;
            Vector2 a = _circlingBox.center;

            if (b == c)
            {
                return;
            }

            bool isLeft = ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0; // a = mid, b = last, c = now

            if (isLeft)
            {
                _curveAmount -= Time.deltaTime * _curveSpeed;
            }
            else
            {
                _curveAmount += Time.deltaTime * _curveSpeed;
            }

            _curveAmount = Mathf.Clamp(_curveAmount, -_maxCurveAmount, _maxCurveAmount);
        }
    }

    private float minThrowSpeed = 5f;
    private float maxThrowSpeed = 40f;
    private float expectedMaxSwipeSpeed = 4000f;

    private void ThrowBall(Vector2 swipeVelocity)
    {
        Debug.Log(swipeVelocity.magnitude);
        _rigidbody.useGravity = true;

        // Calculate the normalized swipe speed as a value between 0 and 1.
        float normalizedSwipeSpeed = Mathf.Clamp01(swipeVelocity.magnitude / Screen.dpi / expectedMaxSwipeSpeed);

        // Scale the speed between min and max throw speeds.
        _speed = Mathf.Lerp(minThrowSpeed, maxThrowSpeed, normalizedSwipeSpeed);

        // Calculate throw direction (45 degrees upward).
        Vector3 throwDirection = (Camera.main.transform.forward + Vector3.up).normalized;

        // Apply the force to the Pokeball.
        _rigidbody.AddForce(throwDirection * _speed, ForceMode.Impulse);

        _holding = false;
        _thrown = true;

        StartCoroutine(DelayedReset());
    }


}