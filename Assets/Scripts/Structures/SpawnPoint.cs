﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint {
	// SpawnPoint: Class for spawn points. Contains monster spawn position and rotation.
	

	// Spawn point variables:
	public Vector3 spawnPosition;
	public Quaternion spawnRotation;
	public Block block;


	// Constructor:
	public SpawnPoint(Block b, Vector3 pos, Quaternion rot) {
		spawnPosition = pos;
		spawnRotation = rot;
		block = b;
	}
}