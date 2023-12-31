using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public GameObject objectToSpawn;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float despawnDelay;


    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        float randomDespawn = Random.Range(5, 8);
        despawnDelay = randomDespawn;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        Vector3 spawnPosition = GetRandomGroundPosition();
        spawnPosition = new Vector3(Mathf.RoundToInt(spawnPosition.x) + 0.5f, Mathf.RoundToInt(spawnPosition.y) + 0.5f, spawnPosition.z);

        while (IsSpawnPositionBlocked(spawnPosition))
        {
            spawnPosition = GetRandomGroundPosition();
            spawnPosition = new Vector3(Mathf.RoundToInt(spawnPosition.x) + 0.5f, Mathf.RoundToInt(spawnPosition.y) + 0.5f, spawnPosition.z);
        }

        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(spawnedObject);

        StartCoroutine(DespawnObject(spawnedObject));
    }

    private bool IsSpawnPositionBlocked(Vector3 spawnPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(spawnPosition);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Bonus") || collider.CompareTag("Player") || collider.CompareTag("Snake") || collider.CompareTag("Portal"))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator DespawnObject(GameObject obj)
    {
        yield return new WaitForSeconds(despawnDelay);
        spawnedObjects.Remove(obj);
        Destroy(obj);
    }

    private Vector3 GetRandomGroundPosition()
    {
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        if (groundObjects.Length > 0)
        {
            int randomIndex = Random.Range(0, groundObjects.Length);
            Transform groundTransform = groundObjects[randomIndex].transform;
            Vector3 randomPosition = new Vector3(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY), 0);
            return randomPosition;
        }

        return transform.position;
    }
}