using UnityEngine;

public class Pokeball : MonoBehaviour
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

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //for pc = if(Input.GetButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); //for pc = Input.mousePosition
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform == transform)
                {
                    _holding = true;
                    transform.SetParent(null);
                }
            }
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            //for pc = if(Input.GetButtonUp(0)){
            if (_lastMouseY < Input.GetTouch(0).position.y)
            {
                ThrowBall(Input.GetTouch(0).position);
            }
        }

        if (Input.touchCount == 1)
        {
            //for pc = if(Input.GetButton(0)){
            _lastMouseX = Input.GetTouch(0).position.x;
            _lastMouseY = Input.GetTouch(0).position.y;

            if (_lastMouseX < _circlingBox.x)
            {
                _circlingBox.x = _lastMouseX;
            }
            if (_lastMouseX < _circlingBox.xMax)
            {
                _circlingBox.xMax = _lastMouseX;
            }

            if (_lastMouseY < _circlingBox.y)
            {
                _circlingBox.y = _lastMouseY;
            }
            if (_lastMouseY < _circlingBox.yMax)
            {
                _circlingBox.yMax = _lastMouseY;
            }
        }
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

    private void ThrowBall(Vector2 mousePos)
    {
        _rigidbody.useGravity = true;

        float differenceY = (mousePos.y - _lastMouseY) / Screen.height * 100;
        _speed = _throwSpeed * differenceY;

        float x = (mousePos.x - _lastMouseX) / Screen.width;

        Vector3 direction = Quaternion.AngleAxis(x * 180f, Vector3.up) * new Vector3(0f, 1f, 1f);
        direction = Camera.main.transform.TransformDirection(direction);

        _rigidbody.AddForce(direction * _speed);

        _holding = false;
        _thrown = true;

        Invoke("Reset", 5.0f);
    }
}