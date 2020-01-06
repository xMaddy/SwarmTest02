using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Color[] BoidColoration;
    public GameObject Prefab;
    public GameObject Area;
    public Transform SortingParent;
    public Vector3 SpawnSize;

    public bool SpawnWithTime = true;
    [ShowIf(nameof(SpawnWithTime))]
    public float SpawnRate = 0.1f;
    public int SpawnCount = 50;

    private RectTransform _areaRectTransform;
    private Vector2 _canvasSize;
    private float _borderSpawnOffset = 20;

    public static List<GameObject> AllBoids = new List<GameObject>();

    private void Start()
    {
        _areaRectTransform = Area.GetComponent<RectTransform>();
        _canvasSize.x = _areaRectTransform.rect.width * 0.5f - _borderSpawnOffset;
        _canvasSize.y = _areaRectTransform.rect.height * 0.5f - _borderSpawnOffset;

        StartCoroutine(StartSpawns());
    }

    private IEnumerator StartSpawns()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            Spawn();

            if (SpawnWithTime)
                yield return new WaitForSeconds(SpawnRate);
        }
    }

    [Button]
    private void Spawn()
    {
        Vector3 randomHeight = new Vector3(Random.Range(-_canvasSize.x, _canvasSize.x), Random.Range(-_canvasSize.y, _canvasSize.y), 0);

        GameObject obj = Instantiate(Prefab);

        AllBoids.Add(obj);

        obj.transform.SetParent(SortingParent);
        obj.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        obj.GetComponent<RectTransform>().anchoredPosition = randomHeight;
        obj.GetComponent<RectTransform>().localScale = SpawnSize;
        obj.GetComponent<Image>().color = BoidColoration[Random.Range(0, BoidColoration.Length)];
    }
}
