using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour
{
    public GameObject[] groundChunks;  // Array of different ground chunk prefabs
    public Transform player;           // Reference to the player
    public float spawnDistance = 20f;  
    // Distance from player to start spawning new chunks
    [SerializeField]
    private float despawnDistance = 30f; // Distance to despawn old chunks
    public float getDespawnDistance() =>  despawnDistance;

    private Transform lastChunkTransform;
    private Vector3 lastChunkEndPosition;

    void Start()
    {

        lastChunkTransform = Instantiate(groundChunks[0]).transform;
        Transform endPoint = lastChunkTransform.Find("EndPoint");
        if (endPoint != null)
        {
            lastChunkEndPosition = endPoint.position;
        }
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastChunkEndPosition) < spawnDistance)
        {
            SpawnChunk();
        }

        if (Vector3.Distance(player.position, lastChunkTransform.position) > despawnDistance)
        {
            Destroy(lastChunkTransform.gameObject);
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        GameObject chunk = groundChunks[Random.Range(0, groundChunks.Length)];

        Transform newChunkTransform = Instantiate(chunk).transform;

        Transform startPoint = newChunkTransform.Find("StartPoint");
        Transform endPoint = newChunkTransform.Find("EndPoint");

        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("Missing StartPoint or EndPoint in chunk prefab");
            Destroy(newChunkTransform.gameObject);
            return;
        }
        Vector3 chunkOffset = startPoint.position - newChunkTransform.position;
        Vector3 newChunkPosition = lastChunkEndPosition - chunkOffset;

        newChunkTransform.position = newChunkPosition;

        lastChunkEndPosition = endPoint.position;

        lastChunkTransform = newChunkTransform;
    }
}
