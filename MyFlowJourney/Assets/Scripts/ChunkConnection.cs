using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkConnection : MonoBehaviour
{
    // Start is called before the first frame update
    public void ConnectChunks(Transform previousChunk, Transform newChunk)
    {
    // Find the highest and lowest points
    Transform previousHighest = previousChunk.Find("HighestPoint");
    Transform newLowest = newChunk.Find("LowestPoint");
    
    // Calculate the offset needed to connect the chunks
    Vector3 offset = previousHighest.position - newLowest.position;
    
    // Apply the offset to the new chunk
    newChunk.position += offset;
    }
}
