using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint {
	// SpawnPoint: Class for spawn points. Contains monster spawn position and rotation.
	

	// Spawn point variables:
	private Vector3 spawnPosition;
	private Quaternion spawnRotation;
	private Block block;


	// Constructor:
	public SpawnPoint(Block b, Vector3 pos, Quaternion rot) {
		spawnPosition = pos;
		spawnRotation = rot;
		block = b;
	}
}