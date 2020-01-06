using UnityEngine;
using System.Collections.Generic;

public class TutorialFlocking : MonoBehaviour
{
    public static List<GameObject> BoidsList = new List<GameObject>();

    private Vector3 _averagePos;

    private void Start()
    {
        AddToList();
    }

    public void AddToList()
    {
        BoidsList.Add(gameObject);
    }

    private void Update()
    {
        _averagePos = Vector3.zero;

        foreach(GameObject boid in BoidsList)
        {
            _averagePos += boid.transform.position;
        }

        _averagePos /= BoidsList.Count - 1;

        transform.position += _averagePos;
    }
}
