using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint {
  // SpawnPoint:
  //    Immutable abstract data type for spawn points
  //    Contains monster spawn position and rotation


  // Spawn point variables:
  private Vector3 spawnPosition;
  private Quaternion spawnRotation;
  private Block block;


  // Creates a spawn point by block b at position pos
  public SpawnPoint(Block b, Vector3 pos) {
    spawnPosition = pos;
    block = b;

    spawnRotation = Quaternion.identity;
  }

  // Returns block of spawn point
  public Block GetBlock() {
    return block;
  }

  // Returns spawn position of spawn point
  public Vector3 GetPosition() {
    return spawnPosition;
  }

  public Quaternion GetRotation() {
    return spawnRotation;
  }
}