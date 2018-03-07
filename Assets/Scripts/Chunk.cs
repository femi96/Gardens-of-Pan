using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
	// Chunk:
	//		Controller hold and changes blocks for garden
	//		Chunks are the gameObject representations of block data
	

	// Assigned in Editor:
	public GameObject[] blocksPrefabs;

	// Block variables:
	private Block[,] blockMap;		// Grid location to block data @ location
	private GameObject[,] chunkMap;	// Grid location to chunks

	private Transform blockContainer; // Parent transform of chunks
	private Garden garden;
	private int gardenSize;

	// Spawns variables:
	private SpawnPoint[,] xSpawnPoints;
	private SpawnPoint[,] zSpawnPoints;


	// Unity MonoBehavior Functions:
	void Awake() {

		// Awake with components
		blockContainer = transform.Find("BlockContainer");
		garden = GetComponent<Garden>();
		gardenSize = garden.gardenSize;
	}

	void Start() {

		// Create blockMap as Rough Blocks
		blockMap = new Block[gardenSize, gardenSize];
		for(int z = 0; z < gardenSize; z++) {
			for(int x = 0; x < gardenSize; x++) {
				blockMap[x, z] = new Block(BlockType.Rough);
			}
		}

		// Update chunkMap from blockMap
		chunkMap = new GameObject[gardenSize, gardenSize];
		for(int z = 0; z < gardenSize; z++) {
			for(int x = 0; x < gardenSize; x++) {
				UpdateChunk(x, z);
			}
		}
	}

	// Return BlockType of block at Vector3 v
	public BlockType GetType(Vector3 v) {
		float g = gardenSize/2f;
		int x = Mathf.RoundToInt(Mathf.Floor(v.x + g)); // Get x from v
		int z = Mathf.RoundToInt(Mathf.Floor(v.z + g)); // Get z from v

		return blockMap[x, z].GetBlockType();
	}

	// Sets BlockType of block at Vector3 v to BlockType t
	public void SetType(Vector3 v, BlockType t) {
		float g = gardenSize/2f;
		int x = Mathf.RoundToInt(Mathf.Floor(v.x + g)); // Get x from v
		int z = Mathf.RoundToInt(Mathf.Floor(v.z + g)); // Get z from v

		blockMap[x, z].SetBlockType(t);

		UpdateChunk(x, z);
	}

	// Update chunk at x, z from block data
	private void UpdateChunk(int x, int z) {

		// Clear the old chunk gameObject
		if(chunkMap[x, z] != null) {
			Destroy(chunkMap[x, z]);
		}

		// Base prefab on BlockType
		GameObject newBlock = blocksPrefabs[0];
		switch(blockMap[x, z].GetBlockType()) {
			case BlockType.Dirt:
				newBlock = blocksPrefabs[1]; break;
			case BlockType.Grass:
				newBlock = blocksPrefabs[2]; break;
			case BlockType.Water:
				newBlock = blocksPrefabs[3]; break;
			case BlockType.Sand:
				newBlock = blocksPrefabs[4]; break;
		}

		// Create new chunk gameObject
		float g = gardenSize/2f;
		float px = x - g + 0.5f;
		float pz = z - g + 0.5f;

		GameObject go = Instantiate(newBlock, new Vector3(px, -0.5f, pz), Quaternion.identity, blockContainer);
		go.name = "Chunk ("+x+", "+z+")";
		chunkMap[x, z] = go;
	}

	// Update spawn point based on block change
	public void UpdateSpawnPoint(int x, int z) {
		if(true) {
			UpdateAllSpawnPoints();
		}
	}

	// Update spawn points from block data
	public void UpdateAllSpawnPoints() {
		xSpawnPoints = new SpawnPoint[gardenSize, 2];
		zSpawnPoints = new SpawnPoint[2, gardenSize];

		for(int x = 0; x < gardenSize; x++) {
			// xSpawnPoints[x, 0] = new SpawnPoint();
		}
	}
}
