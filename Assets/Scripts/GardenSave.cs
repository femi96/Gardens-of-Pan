using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GardenSave {
  // Mutable data type for a garden's save data
  //    Need to store all garden fields as public.
  //    This is not data safe so instances of garden save should not be passed around.

  public string gardenName = "Null Garden";
  public int gardenID = 0;
  public Block[,] blockMap = new Block[0, 0];
  public UnitSave[] unitSaves = new UnitSave[0];
}