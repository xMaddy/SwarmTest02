using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaycastRing : MonoBehaviour
{
    public float StartSpeed = 50f;
    public float RayLength = 50f;

    private Vector3 _rayDir;
    private int _curAngle, _rotationAngle;
    private int _multiplicator = 1;
    private float _currentSpeed;
    private bool _hitObstacle;

    private List<GameObject> boidsFound = new List<GameObject>();

    private LayerMask _layerToIgnore;
    private const int AngleSteps = 15;
    private const int MaxSight = 100;
    private const int ObstacleLayer = 8;
    private const int BoidLayer = 10;

    private void Start()
    {
        _layerToIgnore = LayerMask.GetMask(LayerMask.LayerToName(9));
        _currentSpeed = StartSpeed;
    }

    private void Update()
    {
        Movement();
        ShootRaycasts();
    }

    private void ShootRaycasts()
    {
        boidsFound.Clear();

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
                if (hit.transform.gameObject.layer == BoidLayer)
                {
                    if (gameObject.layer == BoidLayer)
                        return;

                    Debug.DrawLine(transform.position, hit.transform.position, Color.cyan);

                    boidsFound.Add(hit.transform.gameObject);

                    RotationLerp(hit.transform.rotation);
                }

                else if (hit.transform.gameObject.layer == ObstacleLayer)
                {
                    //Debug.DrawRay(transform.position, _rayDir.normalized * RayLength, Color.red);
                    if(Vector3.Distance(transform.position, hit.transform.position) < RayLength)
                    Debug.DrawLine(transform.position, hit.transform.position, Color.white);

                    _rotationAngle += (int)Mathf.Sign(_curAngle) * -1;
                    _hitObstacle = true;
                    //_currentSpeed = Mathf.Clamp(1 / StartSpeed * 100, StartSpeed * 0.5f, StartSpeed);
                }  
            }
            else
            {
                if (_hitObstacle)
                {
                    _hitObstacle = false;
                    transform.rotation = transform.rotation * Quaternion.Euler(0, 0, _rotationAngle);
                    _rotationAngle = 0;
                    _currentSpeed = StartSpeed;
                }
            }
        }

        if (boidsFound.Count > 0)
            CheckForAverages(boidsFound);
    }

    private void CheckForAverages(List<GameObject> currentHits)
    {
        Vector3 averagePosition = new Vector3(
                                    currentHits.Average(i => i.transform.position.x),
                                    currentHits.Average(i => i.transform.position.y),
                                    currentHits.Average(i => i.transform.position.z));

        //transform.position = averagePosition;
    }

    private void Movement()
    {
        transform.position += transform.up * Time.deltaTime * _currentSpeed;
    }

    private IEnumerator RotationLerp(Quaternion hitRot)
    {
        float totalTime = 2;
        float elapsedTime = 0;

        Quaternion curRot = transform.rotation;

        while (elapsedTime < totalTime)
        {
            transform.rotation = Quaternion.Lerp(curRot, hitRot, elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}