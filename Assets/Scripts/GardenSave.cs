using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GardenSave {
  /*
  Mutable data type for garden's save data
  This is not data safe so instances should not be passed around.
  */

  public string gardenName = "Null";
  public int gardenId = 0;
  public Voxel[,] voxelMap = new Voxel[0, 0];
}