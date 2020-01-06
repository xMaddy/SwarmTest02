using UnityEngine;

public class Raycasts : MonoBehaviour
{
    private Vector3 _startDirection = new Vector3(1, 1, 10);
    private Vector3 _dirV;

    private float _angleStep = 15;
    private float _curAngle;

    protected void Update()
    {
        AnotherTry();
    }

    private void AnotherTry()
    {
        _curAngle = 0;
        _dirV = _startDirection;

        for (int i = 0; i < 10000; i++)
        {
            Vector3 v = _dirV;

            v = Quaternion.AngleAxis(_curAngle, transform.forward) * v;

            if (Physics.Raycast(transform.position, v.normalized, out RaycastHit hit, 2f))
            {
                Debug.DrawRay(transform.position, v.normalized * 2f, Color.red, Mathf.Infinity);
            }
            else
            {
                Debug.DrawRay(transform.position, v.normalized * 2f, Color.cyan, Mathf.Infinity);
            }

            _curAngle += _angleStep;

            if (_curAngle > 360)
            {
                _curAngle = 0;
                _dirV = new Vector3(_dirV.x, _dirV.y, Mathf.Clamp(_dirV.z - 0.2f, -10, 10));
                _angleStep = 15 + Mathf.Abs(_dirV.z)*2;
            }
        }
    }
}