using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Voxel {
  public int height;
  public VoxelType voxelType;

  private List<VoxelType> GroundTypes = new List<VoxelType> { VoxelType.Dirt, VoxelType.Grass, VoxelType.Sand };

  public Voxel() {
    voxelType = VoxelType.Dirt;
    height = 0;
  }

  public void ApplyAction(ToolAction a) {

    switch (a) {

    case ToolAction.GroundRaise:
      if (height < 2)
        height += 1;

      break;

    case ToolAction.GroundLower:
      if (height > 0)
        height -= 1;

      break;

    case ToolAction.WaterDig:
      if (GroundTypes.Contains(voxelType)) {
        voxelType = VoxelType.Water;
        return;
      }

      if (voxelType == VoxelType.Water) {
        voxelType = VoxelType.WaterDeep;
        return;
      }

      break;

    case ToolAction.WaterFill:
      if (voxelType == VoxelType.WaterDeep) {
        voxelType = VoxelType.Water;
        return;
      }

      if (voxelType == VoxelType.Water) {
        voxelType = VoxelType.Dirt;
        return;
      }

      break;

    case ToolAction.CoverClear:
      if (GroundTypes.Contains(voxelType))
        voxelType = VoxelType.Dirt;

      break;

    case ToolAction.CoverGrass:
      if (GroundTypes.Contains(voxelType))
        voxelType = VoxelType.Grass;

      break;

    case ToolAction.CoverSand:
      if (GroundTypes.Contains(voxelType))
        voxelType = VoxelType.Sand;

      break;
    }
  }
}
