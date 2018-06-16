using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBoard : MonoBehaviour {
  // Game controller that handles garden's board data, board includes:
  //    blocks - board tile data
  //    chunks - gameObject representations of blocks
  //    spawnPoints (blocks and chunks for spawns)

  // Assigned in Editor:
  public BlockType[] blockPrefabsIndex; // Maps BlockType to index
  public GameObject[] blockPrefabs;     // Maps index to block chunk
  public GameObject[] spawnPrefabs;     // Maps index to spawn chunk

  private Garden garden;
  private int gardenSize;

  // Blocks:
  private Block[,] blockMap;      // Grid location to block @ location
  private GameObject[,] chunkMap; // Grid location to chunk

  private Transform blockContainer; // Parent transform of block chunks

  // Spawns:
  private SpawnPoint[,] spawnPointMap; // Grid location to spawns
  private GameObject[,] spawnChunkMap; // Grid location to spawn chunks

  private Transform spawnContainer; // Parent transform of spawn chunks

  // BlockMap & ChunkMap are full
  // SpawnMaps only have edges

  void Awake() {

    // Awake with components
    blockContainer = transform.Find("BlockContainer");
    spawnContainer = transform.Find("SpawnContainer");
    garden = GetComponent<Garden>();
  }

  // Create empty board collections
  private void SetupCollections() {

    // Clear containers
    foreach (Transform child in blockContainer)
      Destroy(child.gameObject);

    foreach (Transform child in spawnContainer)
      Destroy(child.gameObject);

    // Create collections
    gardenSize = garden.gardenSize;
    blockMap = new Block[gardenSize, gardenSize];
    chunkMap = new GameObject[gardenSize, gardenSize];
    spawnPointMap = new SpawnPoint[gardenSize + 2, gardenSize + 2];
    spawnChunkMap = new GameObject[gardenSize + 2, gardenSize + 2];
  }

  // Setup a new garden board
  public void NewBoard() {

    SetupCollections();

    // Start all blocks as Rough
    for (int z = 0; z < gardenSize; z++) {
      for (int x = 0; x < gardenSize; x++) {
        blockMap[x, z] = new Block(BlockType.Rough);
      }
    }

    // Update chunks from blocks
    for (int z = 0; z < gardenSize; z++) {
      for (int x = 0; x < gardenSize; x++) {
        UpdateChunk(x, z);
      }
    }
  }

  // Set blockMap to a blockMap
  public void SetBlockMap(Block[,] blocks) {

    SetupCollections();

    blockMap = blocks;

    for (int z = 0; z < gardenSize; z++) {
      for (int x = 0; x < gardenSize; x++) {
        // blockMap[x, z] = blocks[x, z];
        UpdateChunk(x, z);
      }
    }
  }

  // Get blockMap of board
  public Block[,] GetBlockMap() {
    return blockMap;
  }

  // Get block at Vector3 v
  public Block GetBlock(Vector3 v) {
    float g = gardenSize / 2f;
    int x = Mathf.RoundToInt(Mathf.Floor(v.x + g)); // Get x from v
    int z = Mathf.RoundToInt(Mathf.Floor(v.z + g)); // Get z from v
    return blockMap[x, z];
  }

  // Returns BlockType of block at Vector3 v
  public BlockType GetType(Vector3 v) {
    return GetBlock(v).GetBlockType();
  }

  // Apply action to block at Vector3 v
  public void ApplyAction(Vector3 v, ToolAction a) {
    GetBlock(v).ApplyAction(a);
    UpdateChunk(v);
  }

  // Returns BlockType of block at Vector3 v
  public int GetBlockTypeCount(BlockType t) {
    int count = 0;

    for (int z = 0; z < gardenSize; z++) {
      for (int x = 0; x < gardenSize; x++) {
        if (blockMap[x, z].GetBlockType() == t)
          count += 1;
      }
    }

    return count;
  }

  // Returns list of spawn points on board
  public List<SpawnPoint> GetSpawnPoints() {
    List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    int iMax = spawnPointMap.GetLength(0) - 1;
    int jMax = spawnPointMap.GetLength(1) - 1;

    // Add all from x edges
    for (int i = 0; i <= iMax; i++) {
      if (spawnPointMap[i, 0] != null) spawnPoints.Add(spawnPointMap[i, 0]);

      if (spawnPointMap[i, jMax] != null) spawnPoints.Add(spawnPointMap[i, jMax]);
    }

    // Add all from z edges
    for (int j = 1; j <= jMax - 1; j++) {
      if (spawnPointMap[0, j] != null) spawnPoints.Add(spawnPointMap[0, j]);

      if (spawnPointMap[iMax, j] != null) spawnPoints.Add(spawnPointMap[iMax, j]);
    }

    return spawnPoints;
  }


  // Returns block chunk prefab of BlockType t
  private GameObject GetBlockPrefab(BlockType t) {
    GameObject newChunk = blockPrefabs[0];

    for (int i = 0; i < blockPrefabsIndex.Length; i++) {
      if (blockPrefabsIndex[i] == t && i < blockPrefabs.Length) newChunk = blockPrefabs[i];
    }

    return newChunk;
  }

  // Returns spawn chunk prefab of BlockType t
  private GameObject GetSpawnPrefab(BlockType t) {
    GameObject newChunk = spawnPrefabs[0];

    for (int i = 0; i < blockPrefabsIndex.Length; i++) {
      if (blockPrefabsIndex[i] == t && i < spawnPrefabs.Length) newChunk = spawnPrefabs[i];
    }

    return newChunk;
  }


  // Update chunk at x, z based on block data
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

  private void UpdateChunk(Vector3 v) {
    float g = gardenSize / 2f;
    int x = Mathf.RoundToInt(Mathf.Floor(v.x + g)); // Get x from v
    int z = Mathf.RoundToInt(Mathf.Floor(v.z + g)); // Get z from v
    UpdateChunk(x, z);
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

  // Update spawn point at x, y near Block b
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
