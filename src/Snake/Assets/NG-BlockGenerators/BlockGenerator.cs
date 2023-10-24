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
        SpawnObject();
        SpawnObject();

        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        Vector3 spawnPosition = GetRandomGroundPosition();

        while (IsSpawnPositionBlocked(spawnPosition))
        {
            spawnPosition = GetRandomGroundPosition();
        }

        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(spawnedObject);

        StartCoroutine(DespawnObject(spawnedObject));
    }

    private bool IsSpawnPositionBlocked(Vector3 spawnPosition)
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null && Vector3.Distance(obj.transform.position, spawnPosition) < 1f)
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
