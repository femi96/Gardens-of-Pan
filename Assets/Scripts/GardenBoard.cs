using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BlockPrefabEntry {
  public BlockType blockType;
  public GameObject blockFullPrefab;
  public GameObject blockHalfPrefab;
  public GameObject blockSpawnPrefab;
}

public class GardenBoard : MonoBehaviour {
  // Handles garden's board data, board includes:
  //  blockInfos - block's data
  //  blockObjs - block's gameObject representation
  //  spawns - blockInfo and blockObj for invisible out of garden blocks spawn points

  // Assigned in Editor
  public BlockPrefabEntry[] blockPrefabMap;  // Maps BlockType to prefabs

  private Garden garden;
  private int gardenGridSize;

  private float gardenSize;
  private float blockSize = 0.5f;

  // Blocks
  private Block[,] blockInfoMap;      // Grid location to block info
  private GameObject[,] blockObjMap; // Grid location to block objs

  private Transform blockContainer; // Parent transform of block chunks

  // Spawn points
  private SpawnPoint[,] spawnPointMap; // Grid location to spawn point
  private GameObject[,] spawnObjMap; // Grid location to spawn chunks

  private Transform spawnContainer; // Parent transform of spawn chunks

  // Invariants
  // blockInfoMap & blockObjMap are full
  // spawnPointMap & spawnObjMap only have edges

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
    gardenGridSize = Mathf.CeilToInt(gardenSize / blockSize);

    blockInfoMap = new Block[gardenGridSize, gardenGridSize];
    blockObjMap = new GameObject[gardenGridSize, gardenGridSize];

    spawnPointMap = new SpawnPoint[gardenGridSize + 2, gardenGridSize + 2];
    spawnObjMap = new GameObject[gardenGridSize + 2, gardenGridSize + 2];
  }

  // Setup a new garden board
  public void NewBoard() {

    SetupCollections();

    // Start all blocks as Rough
    for (int z = 0; z < gardenGridSize; z++) {
      for (int x = 0; x < gardenGridSize; x++) {
        blockInfoMap[x, z] = new Block();
      }
    }

    // Update chunks from blocks
    for (int z = 0; z < gardenGridSize; z++) {
      for (int x = 0; x < gardenGridSize; x++) {
        UpdateBlockObj(x, z);
      }
    }
  }

  // Set blockInfoMap to a blockInfoMap
  public void SetBlockMap(Block[,] blocks) {

    SetupCollections();

    blockInfoMap = blocks;

    for (int z = 0; z < gardenGridSize; z++) {
      for (int x = 0; x < gardenGridSize; x++) {
        UpdateBlockObj(x, z);
      }
    }
  }

  // Get blockInfoMap of board
  public Block[,] GetBlockMap() {
    return blockInfoMap;
  }

  // Get grid location Vector2Int from Vector3 v
  private Vector2Int GetCoords(Vector3 v) {
    float g = gardenSize / 2f;
    float xPos = v.x + g;
    float zPos = v.z + g;
    int x = Mathf.FloorToInt(xPos / blockSize); // Get x from v
    int z = Mathf.FloorToInt(zPos / blockSize); // Get z from v
    return new Vector2Int(x, z);
  }

  // Get Vector3 v from grid location Vector2Int
  private Vector3 GetPosition(Vector2Int v) {
    float g = gardenSize / 2f;
    float x = ((v.x + 0.5f) * blockSize) - g;
    float z = ((v.y + 0.5f) * blockSize) - g;
    return new Vector3(x, 0, z);
  }

  // Get block at Vector3 v
  public Block GetBlock(Vector3 v) {
    Vector2Int c = GetCoords(v);

    if ((c.x < gardenGridSize && c.x >= 0) && (c.y < gardenGridSize && c.y >= 0))
      return blockInfoMap[c.x, c.y];

    return null;
  }

  // Get block that is neighbor of block at Vector3 v in direction dir
  public Block GetBlockNeighbor(Vector3 v, Vector3 dir) {
    return GetBlock(v + (blockSize * dir));
  }

  // Returns BlockType of block at Vector3 v
  public BlockType GetType(Vector3 v) {
    return GetBlock(v).GetBlockType();
  }

  // Get block at Vector3 v
  public bool InGarden(Vector3 v) {
    Vector2Int c = GetCoords(v);
    return (c.x < gardenGridSize && c.x >= 0) && (c.y < gardenGridSize && c.y >= 0);
  }

  // Apply action to block at Vector3 v
  public void ApplyAction(Vector3 v, ToolAction a) {
    GetBlock(v).ApplyAction(a);
    UpdateBlockObj(v);
  }
  public void ApplyActionNeighbor(Vector3 v, Vector3 dir, ToolAction a) {
    GetBlock(v + (blockSize * dir)).ApplyAction(a);
    UpdateBlockObj(v + (blockSize * dir));
  }

  public void ApplyHalfDig(Vector3 v, BlockDir dir) {
    GetBlock(v).halfDir = dir;
    UpdateBlockObj(v);
  }

  // Returns BlockType of block at Vector3 v
  public float GetBlockTypeCount(BlockType t) {
    int count = 0;

    for (int z = 0; z < gardenGridSize; z++) {
      for (int x = 0; x < gardenGridSize; x++) {
        if (blockInfoMap[x, z].GetBlockType() == t)
          count += 1;
      }
    }

    return count * blockSize * blockSize;
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


  // Returns block obj prefab of BlockType t
  private GameObject GetBlockPrefab(Block b) {
    BlockType t = b.GetBlockType();
    GameObject newBlockPrefab = blockPrefabMap[0].blockFullPrefab;

    for (int i = 0; i < blockPrefabMap.Length; i++) {
      if (blockPrefabMap[i].blockType == t) {
        if (b.halfDir == BlockDir.None)
          newBlockPrefab = blockPrefabMap[i].blockFullPrefab;
        else
          newBlockPrefab = blockPrefabMap[i].blockHalfPrefab;
      }
    }

    return newBlockPrefab;
  }

  // Returns spawn chunk prefab of BlockType t
  private GameObject GetSpawnPrefab(BlockType t) {
    GameObject newBlockPrefab = blockPrefabMap[0].blockSpawnPrefab;

    for (int i = 0; i < blockPrefabMap.Length; i++) {
      if (blockPrefabMap[i].blockType == t) newBlockPrefab = blockPrefabMap[i].blockSpawnPrefab;
    }

    return newBlockPrefab;
  }


  // Update block obj at x, z based on block data
  private void UpdateBlockObj(int x, int z) {

    // Clear the old chunk gameObject
    if (blockObjMap[x, z] != null) {
      Destroy(blockObjMap[x, z]);
    }

    // Base prefab on BlockType
    GameObject newBlockPrefab = GetBlockPrefab(blockInfoMap[x, z]);

    // Create new chunk gameObject
    Vector3 pos = GetPosition(new Vector2Int(x, z)) - 0.5f * Vector3.up;

    GameObject go = Instantiate(newBlockPrefab, pos, Quaternion.identity, blockContainer);
    go.transform.localScale = new Vector3(blockSize, 1, blockSize);
    go.name = "BlockObj (" + x + ", " + z + ")";

    switch (blockInfoMap[x, z].halfDir) {
    case BlockDir.Forward:
      go.transform.rotation = Quaternion.Euler(0, 180, 0);
      break;

    case BlockDir.Right:
      go.transform.rotation = Quaternion.Euler(0, 270, 0);
      break;

    case BlockDir.Back:
      go.transform.rotation = Quaternion.Euler(0, 0, 0);
      break;

    case BlockDir.Left:
      go.transform.rotation = Quaternion.Euler(0, 90, 0);
      break;

    default:
      break;
    }

    blockObjMap[x, z] = go;

    UpdateAdjacentSpawnPoint(x, z);
  }

  private void UpdateBlockObj(Vector3 v) {
    Vector2Int c = GetCoords(v);
    UpdateBlockObj(c.x, c.y);
  }

  // Update spawn points around a block
  private void UpdateAdjacentSpawnPoint(int x, int z) {
    Block b = blockInfoMap[x, z];

    int px = x + 1;
    int pz = z + 1;

    if (x == 0) {
      UpdateSpawnPoint(x, pz, b);

      if (z == 0)
        UpdateSpawnPoint(x, z, b);

      if (z == gardenGridSize - 1)
        UpdateSpawnPoint(x, z + 2, b);
    }

    if (x == gardenGridSize - 1) {
      UpdateSpawnPoint(x + 2, pz, b);

      if (z == 0)
        UpdateSpawnPoint(x + 2, z, b);

      if (z == gardenGridSize - 1)
        UpdateSpawnPoint(x + 2, z + 2, b);
    }

    if (z == 0)
      UpdateSpawnPoint(px, z, b);

    if (z == gardenGridSize - 1)
      UpdateSpawnPoint(px, z + 2, b);
  }

  // Update spawn point at x, y near Block b
  private void UpdateSpawnPoint(int x, int z, Block b) {

    // Clear the old chunk gameObject
    if (spawnObjMap[x, z] != null) {
      Destroy(spawnObjMap[x, z]);
    }

    // Base prefab on BlockType
    GameObject newBlockPrefab = GetSpawnPrefab(b.GetBlockType());

    // Create new chunk GameObject
    Vector3 pos = GetPosition(new Vector2Int(x - 1, z - 1)) - 0.5f * Vector3.up;
    Vector3 posPoint = pos + 0.5f * Vector3.up;

    GameObject go = Instantiate(newBlockPrefab, pos, Quaternion.identity, spawnContainer);
    go.transform.localScale = new Vector3(blockSize, 1, blockSize);
    go.name = "SpawnObj (" + x + ", " + z + ")";
    spawnObjMap[x, z] = go;

    // Create new SpawnPoint
    spawnPointMap[x, z] = new SpawnPoint(b, posPoint);
  }
}
