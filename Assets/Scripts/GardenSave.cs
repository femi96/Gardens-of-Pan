using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GardenSave {
  // Immutable data type for a garden's save data
  //    Need to store all garden fields as public.
  //    This is not data safe so instances of garden save should not be passed around.

  public string gardenName = "Null Garden";
  public Block[,] blockMap = new Block[0, 0];
}