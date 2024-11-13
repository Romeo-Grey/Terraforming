using UnityEngine;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour
{
    public GenTest genTest;
    private Dictionary<Vector3Int, Chunk> activeChunks = new Dictionary<Vector3Int, Chunk>();
    private Queue<Vector3Int> chunkLoadQueue = new Queue<Vector3Int>();
    
    public void QueueChunkLoad(Vector3Int chunkCoord) {
        // Only queue if the chunk isn't already loaded or queued
        if (!activeChunks.ContainsKey(chunkCoord) && !chunkLoadQueue.Contains(chunkCoord)) {
            chunkLoadQueue.Enqueue(chunkCoord);
        }
    }

    void Update() {
        // Process a few chunks per frame
        int chunksToLoadPerFrame = 2;
        for(int i = 0; i < chunksToLoadPerFrame; i++) {
            if(chunkLoadQueue.Count > 0) {
                Vector3Int coord = chunkLoadQueue.Dequeue();
                LoadChunkAt(coord);
            }
        }
    }

    void LoadChunkAt(Vector3Int coord) {
        // Double-check the chunk isn't already loaded before creating it
        if (activeChunks.ContainsKey(coord)) {
            return;
        }

        float chunkSize = genTest.boundsSize / genTest.numChunks;
        float posX = (-(genTest.numChunks - 1f) / 2 + coord.x) * chunkSize;
        float posY = (-(genTest.numChunks - 1f) / 2 + coord.y) * chunkSize;
        float posZ = (-(genTest.numChunks - 1f) / 2 + coord.z) * chunkSize;
        Vector3 centre = new Vector3(posX, posY, posZ);

        GameObject meshHolder = new GameObject($"Chunk ({coord.x}, {coord.y}, {coord.z})");
        meshHolder.transform.parent = genTest.transform;
        meshHolder.layer = genTest.gameObject.layer;

        Chunk chunk = new Chunk(coord, centre, chunkSize, genTest.numPointsPerAxis, meshHolder);
        chunk.SetMaterial(genTest.material);
        activeChunks.Add(coord, chunk);
        
        genTest.GenerateChunk(chunk);
    }
}
