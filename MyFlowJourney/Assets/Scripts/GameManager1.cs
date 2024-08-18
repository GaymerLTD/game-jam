using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Transform[] chunkPrefabs;
    public Transform initialChunk;
    private Transform lastChunk;
    public GameObject playerPrefab;
    private GameObject player;
    public float spawnDistance = 10f;
    public float removeDistance = 20f;

    private List<Transform> activeChunks = new List<Transform>();

    void Start()
    {
        if (initialChunk == null)
        {
            return;
        }

        lastChunk = initialChunk;
        activeChunks.Add(lastChunk);
        Vector3 startPosition = lastChunk.Find("HighestPoint").position;
        startPosition.x += 1;
        startPosition.y += 1;
        player = Instantiate(playerPrefab, startPosition, Quaternion.identity);
    }

    void FixedUpdate()
    {

        if (lastChunk != null)
        {
            float playerX = player.transform.position.x;
            float endOfCurrentChunkX = lastChunk.Find("LowestPoint").position.x - spawnDistance;

            if (playerX >= endOfCurrentChunkX)
            {
                SpawnNextChunk();
                RemoveOldChunks();
            }
        }
        else
        {
            Debug.LogWarning("LastChunk is null.");
        }
    }

    public void SpawnNextChunk()
    {
        Transform newChunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)]);
        ConnectChunks(lastChunk, newChunk);

        lastChunk = newChunk;
        activeChunks.Add(newChunk);
    }

    private void ConnectChunks(Transform previousChunk, Transform newChunk)
    {
        Transform previousLowest = previousChunk.Find("LowestPoint");
        Transform newHighest = newChunk.Find("HighestPoint");

        if (previousLowest != null && newHighest != null)
        {
            Vector3 offset = previousLowest.position - newHighest.position;
            newChunk.position += offset;
        }
        else
        {
            Debug.LogWarning("Missing HighestPoint or LowestPoint in ConnectChunks.");
        }
    }

    private void RemoveOldChunks()
    {
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            Transform chunk = activeChunks[i];

            if (chunk != null)
            {
                Transform lowestPoint = chunk.Find("LowestPoint");
                if (lowestPoint != null)
                {
                    float distance = player.transform.position.x - lowestPoint.position.x;

                    if (distance > removeDistance)
                    {
                        Debug.Log("Removing chunk: " + chunk.name);
                        Destroy(chunk.gameObject);
                        activeChunks.RemoveAt(i);
                    }
                }
                else
                {
                    Debug.LogWarning("LowestPoint not found in chunk: " + chunk.name);
                }
            }
            else
            {
                Debug.LogWarning("Active chunk reference is null.");
                activeChunks.RemoveAt(i);
            }
        }
    }
}
