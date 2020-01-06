using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform PlayerContainer;

    public bool SpawnWithTime = true;
    public float SpawnRate = 0.1f;
    public int SpawnOnStart = 50;

    private RectTransform _rectTransform;
    private Vector2 _canvasSize;
    private float _spawnOffset = 20;

    public static List<GameObject> AllObjects = new List<GameObject>();

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasSize.x = _rectTransform.rect.width * 0.5f - _spawnOffset;
        _canvasSize.y = _rectTransform.rect.height * 0.5f - _spawnOffset;

        StartCoroutine(SpawnsOnStart());
    }

    private IEnumerator SpawnsOnStart()
    {
        for(int i = 0; i < SpawnOnStart; i++)
        {
            Spawn();

            if(SpawnWithTime)
                yield return new WaitForSeconds(SpawnRate);
        }
    }
    
    [Button]
    private void Spawn()
    {
        Vector3 randomHeight = new Vector3(Random.Range(-_canvasSize.x, _canvasSize.x), Random.Range(-_canvasSize.y, _canvasSize.y), 0);

        GameObject newSpawn = Instantiate(PlayerPrefab, transform.position, Quaternion.Euler(0f,0f, Random.Range(0f, 360f)));

        AllObjects.Add(newSpawn);

        newSpawn.transform.SetParent(PlayerContainer);
        newSpawn.GetComponent<RectTransform>().anchoredPosition= randomHeight;
        newSpawn.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
