using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBoard : MonoBehaviour {
  // GardenBoard:
  //    Controller hold and changes blocks for garden
  //    Chunks are the gameObject representations of block data


  // Assigned in Editor:
  public BlockType[] blockPrefabsIndex;
  public GameObject[] blockPrefabs;
  public GameObject[] spawnPrefabs;

  // Block variables:
  private Block[,] blockMap;      // Grid location to block data @ location
  private GameObject[,] chunkMap; // Grid location to chunk gameobjects

  private Transform blockContainer; // Parent transform of chunks
  private Transform spawnContainer; // Parent transform of spawn chunks
  private Garden garden;
  private int gardenSize;

  // Spawn variables:
  private SpawnPoint[,] spawnPointMap;
  private GameObject[,] spawnChunkMap;


  // Unity MonoBehavior Functions:
  void Awake() {

    // Awake with components
    blockContainer = transform.Find("BlockContainer");
    spawnContainer = transform.Find("SpawnContainer");
    garden = GetComponent<Garden>();
    gardenSize = garden.gardenSize;
  }

  void Start() {

    // Create blockMap as Rough Blocks
    blockMap = new Block[gardenSize, gardenSize];

    for (int z = 0; z < gardenSize; z++) {
      for (int x = 0; x < gardenSize; x++) {
        blockMap[x, z] = new Block(BlockType.Rough);
      }
    }

    // Create edgeMaps
    spawnPointMap = new SpawnPoint[gardenSize + 2, gardenSize + 2];
    spawnChunkMap = new GameObject[gardenSize + 2, gardenSize + 2];

    // Update chunkMap from blockMap
    chunkMap = new GameObject[gardenSize, gardenSize];

    for (int z = 0; z < gardenSize; z++) {
      for (int x = 0; x < gardenSize; x++) {
        UpdateChunk(x, z);
      }
    }
  }

  // Returns BlockType of block at Vector3 v
  public BlockType GetType(Vector3 v) {
    float g = gardenSize / 2f;
    int x = Mathf.RoundToInt(Mathf.Floor(v.x + g)); // Get x from v
    int z = Mathf.RoundToInt(Mathf.Floor(v.z + g)); // Get z from v

    return blockMap[x, z].GetBlockType();
  }

  // Sets BlockType of block at Vector3 v to BlockType t
  public void SetType(Vector3 v, BlockType t) {
    float g = gardenSize / 2f;
    int x = Mathf.RoundToInt(Mathf.Floor(v.x + g)); // Get x from v
    int z = Mathf.RoundToInt(Mathf.Floor(v.z + g)); // Get z from v

    blockMap[x, z] = new Block(t);

    UpdateChunk(x, z);
  }


  // Returns block chunk prefab of BlockType t
  private GameObject GetBlockPrefab(BlockType t) {
    GameObject newChunk = blockPrefabs[0];

    for (int i = 0; i < blockPrefabsIndex.Length; i++) {
      if (blockPrefabsIndex[i] == t && i < blockPrefabs.Length) newChunk = blockPrefabs[i];
    }

    return newChunk;
  }

  // Returns spawn point chunk prefab of BlockType t
  private GameObject GetSpawnPrefab(BlockType t) {
    GameObject newChunk = spawnPrefabs[0];

    for (int i = 0; i < blockPrefabsIndex.Length; i++) {
      if (blockPrefabsIndex[i] == t && i < spawnPrefabs.Length) newChunk = spawnPrefabs[i];
    }

    return newChunk;
  }


  // Update chunk at x, z from block data
  private void UpdateChunk(int x, int z) {

    // Clear the old chunk gameObject
    if (chunkMap[x, z] != null) {
      Destroy(chunkMap[x, z]);
    }

    // Base prefab on BlockType
    GameObject newChunk = GetBlockPrefab(blockMap[x, z].GetBlockType());

    // Create new chunk gameObject
    float g = gardenSize / 2f;
    float px = x - g + 0.5f;
    float pz = z - g + 0.5f;

    GameObject go = Instantiate(newChunk, new Vector3(px, -0.5f, pz), Quaternion.identity, blockContainer);
    go.name = "Chunk (" + x + ", " + z + ")";
    chunkMap[x, z] = go;

    UpdateAdjacentSpawnPoint(x, z);
  }

  // Update spawn points around a block
  private void UpdateAdjacentSpawnPoint(int x, int z) {
    Block b = blockMap[x, z];

    int px = x + 1;
    int pz = z + 1;

    if (x == 0) {
      UpdateSpawnPoint(x, pz, b);

      if (z == 0)
        UpdateSpawnPoint(x, z, b);

      if (z == gardenSize - 1)
        UpdateSpawnPoint(x, z + 2, b);
    }

    if (x == gardenSize - 1) {
      UpdateSpawnPoint(x + 2, pz, b);

      if (z == 0)
        UpdateSpawnPoint(x + 2, z, b);

      if (z == gardenSize - 1)
        UpdateSpawnPoint(x + 2, z + 2, b);
    }

    if (z == 0)
      UpdateSpawnPoint(px, z, b);

    if (z == gardenSize - 1)
      UpdateSpawnPoint(px, z + 2, b);
  }

  // Update spawn point around Block b at x, y
  private void UpdateSpawnPoint(int x, int z, Block b) {

    // Clear the old chunk gameObject
    if (spawnChunkMap[x, z] != null) {
      Destroy(spawnChunkMap[x, z]);
    }

    // Base prefab on BlockType
    GameObject newChunk = GetSpawnPrefab(b.GetBlockType());

    // Create new chunk GameObject
    float g = (gardenSize + 2) / 2f;
    float px = x - g + 0.5f;
    float pz = z - g + 0.5f;

    GameObject go = Instantiate(newChunk, new Vector3(px, -0.5f, pz), Quaternion.identity, spawnContainer);
    go.name = "Spawn (" + x + ", " + z + ")";
    spawnChunkMap[x, z] = go;

    // Create new SpawnPoint
    spawnPointMap[x, z] = new SpawnPoint(b, new Vector3(px, 0, pz));
  }
}
