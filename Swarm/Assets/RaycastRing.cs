using System.Collections;
using UnityEngine;

public class RaycastRing : MonoBehaviour
{
    public float StartSpeed = 50f;
    public float RayLength = 50f;

    private Coroutine _coroutine;
    private Vector3 _rayDir;
    private int _curAngle, _rotationAngle;
    private int _multiplicator = 1;
    private float _currentSpeed;
    private bool _hitObstacle;
    private bool _isSwarming;
    private float timeCounter;

    public bool IsSwarming { get; private set; }

    private LayerMask _layerToIgnore;

    private const int AngleSteps = 15;
    private const int MaxSight = 100;
    private const int ObstacleLayer = 8;

    private void Start()
    {
        _layerToIgnore = LayerMask.GetMask(LayerMask.LayerToName(9));
        _currentSpeed = StartSpeed;
    }

    private void Update()
    {
        Swarming();
        Movement();
        ShootRaycasts();
    }

    private void ShootRaycasts()
    {
        for (int i = 0; i <= MaxSight; i += AngleSteps)
        {
            _curAngle = i * _multiplicator;
            _multiplicator *= -1;

            if (_multiplicator == -1)
                i -= AngleSteps;

            _rayDir = transform.up;
            _rayDir = Quaternion.AngleAxis(_curAngle, transform.forward) * _rayDir;

            if (Physics.Raycast(transform.position, _rayDir.normalized, out RaycastHit hit, RayLength, ~_layerToIgnore))
            {
                if (hit.transform.gameObject.layer == ObstacleLayer)
                {
                    Debug.DrawRay(transform.position, _rayDir.normalized * RayLength, Color.red);
                    _rotationAngle += (int)Mathf.Sign(_curAngle) * -1;
                    _hitObstacle = true;
                    _currentSpeed = Mathf.Clamp(2 / StartSpeed * 100, StartSpeed * 0.5f, StartSpeed);
                }

                if (hit.transform.gameObject.layer == 12)
                {
                    Debug.DrawRay(transform.position, _rayDir.normalized * RayLength, Color.blue);

                    //transform.position = Quaternion.AngleAxis(2, Vector3.up) * new Vector3(0.1f, 0f);

                    transform.rotation = transform.rotation * Quaternion.Euler(0, 0, Random.Range(-1, 0));
                }

                if (hit.transform.gameObject.layer == 11) //RedFish
                {
                    Debug.DrawRay(transform.position, _rayDir.normalized * RayLength, Color.red);
                    IsSwarming = true;
                    if (_coroutine == null)
                        StartCoroutine(TestLerp(hit.transform.rotation));
                }

                if (IsSwarming)
                    return;

                else if (hit.transform.gameObject.layer == 10) //OtherPlayer
                {
                    if (hit.transform.GetComponent<RaycastRing>().IsSwarming)
                    {
                        Debug.DrawRay(transform.position, _rayDir.normalized * RayLength, Color.yellow);
                        IsSwarming = true;

                        if (_coroutine != null)
                            StopCoroutine(_coroutine);

                        StartCoroutine(TestLerp(hit.transform.rotation));
                    }
                }
            }
            else
            {
                //Debug.DrawRay(transform.position, _rayDir.normalized * RayLength, Color.white);

                if (_hitObstacle)
                {
                    _hitObstacle = false;
                    transform.rotation = transform.rotation * Quaternion.Euler(0, 0, _rotationAngle);
                    _rotationAngle = 0;
                    _currentSpeed = StartSpeed;
                }
            }
        }
    }

    private void Movement()
    {
        transform.position += transform.up * Time.deltaTime * _currentSpeed;
    }

    private void Swarming()
    {
        foreach (GameObject obj in PlayerSpawn.AllObjects)
        {
            if (_hitObstacle || _isSwarming)
                return;

            if (obj == gameObject)
                return;

            if (Vector3.Distance(transform.position, obj.transform.position) < 35)
            {
                _isSwarming = true;
                transform.rotation = obj.transform.rotation;
            }
        }
    }

    private IEnumerator TestLerp(Quaternion hitRot)
    {
        float totalTime = 5;
        float elapsedTime = 0;

        Quaternion curRot = transform.rotation;

        while (elapsedTime < totalTime)
        {
            transform.rotation = Quaternion.Lerp(curRot, hitRot, elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _coroutine = null;
    }
}