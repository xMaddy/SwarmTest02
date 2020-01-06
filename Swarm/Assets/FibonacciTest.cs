using System.Collections.Generic;
using UnityEngine;

public class FibonacciTest : MonoBehaviour
{
    public GameObject Point;
    public  int NumPoints = 300;
    public float TurnFraction;
    public float pow = 5;
    public bool Animate;
    private readonly int _index;

    private readonly List<GameObject> _points = new List<GameObject>();

    private void Start()
    {
        Spawning(0);
    }

    private void Update()
    {
        for (int i = 0; i < NumPoints; i++)
        {
            //float dist = Mathf.Pow(i / (NumPoints - 1f), pow);
            //float angle = 2 * Mathf.PI * TurnFraction * i;

            float t = i / (NumPoints - 1f);
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = 2 * Mathf.PI * TurnFraction * i;

            //float x = dist * Mathf.Cos(angle);
            //float y = dist * Mathf.Sin(angle);

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);

            if (i % 2 == 0)
                PointPosition(x, y, z, i, Color.white);
            else
                PointPosition(x, y, z, i, Color.black);
        }

        if(Animate)
            TurnFraction += Time.deltaTime * 0.0001f;
    }

    private void Spawning(float x)
    {
        for (int i = 0; i < NumPoints; i++)
        {
            x += 0.1f;
            GameObject gO = Instantiate(Point, new Vector3(x, 0, Point.transform.position.z), Point.transform.rotation);
            _points.Add(gO);
        }
    }

    private void PointPosition(float x, float y, float z, int index, Color color)
    {
        _points[index].transform.position = new Vector3(x, y, z);
        //_points[index].GetComponent<SpriteRenderer>().color = color;
    }
}