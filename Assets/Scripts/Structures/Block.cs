using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
  // Mutable data type for tile blocks
  //    Contains a block's type, visual variant, and features

  private BlockType type;
  private int variant;

  // Creates block with BlockType t
  public Block(BlockType t) {
    type = t;
  }

  // Returns BlockType of block
  public BlockType GetBlockType() {
    return type;
  }

  // Change block based on action
  public void ApplyAction(ToolAction a) {

    switch (a) {

    case ToolAction.Dig:

      if (BlockTypes.InGroup(type, BlockTypes.DepthShallow)) {
        if (BlockTypes.InGroup(type, BlockTypes.TempHot))
          type = BlockType.LavaD;
        else if (BlockTypes.InGroup(type, BlockTypes.TempCold))
          type = BlockType.IceD;
        else
          type = BlockType.WaterD;

        break;
      }

      if (BlockTypes.InGroup(type, BlockTypes.DepthGround)) {
        if (BlockTypes.InGroup(type, BlockTypes.TempHot))
          type = BlockType.Lava;
        else if (BlockTypes.InGroup(type, BlockTypes.TempCold))
          type = BlockType.Ice;
        else
          type = BlockType.Water;

        break;
      }

      break;

    case ToolAction.Fill:

      if (BlockTypes.InGroup(type, BlockTypes.DepthShallow)) {
        if (BlockTypes.InGroup(type, BlockTypes.TempHot))
          type = BlockType.Scorch;
        else if (BlockTypes.InGroup(type, BlockTypes.TempCold))
          type = BlockType.Tundra;
        else
          type = BlockType.Dirt;
      }

      if (BlockTypes.InGroup(type, BlockTypes.DepthDeep)) {
        if (BlockTypes.InGroup(type, BlockTypes.TempHot))
          type = BlockType.Lava;
        else if (BlockTypes.InGroup(type, BlockTypes.TempCold))
          type = BlockType.Ice;
        else
          type = BlockType.Water;
      }

      break;

    case ToolAction.Flatten:
      if (type == BlockType.Rough)
        type = BlockType.Dirt;

      break;

    case ToolAction.Grass:
      if (type == BlockType.Dirt)
        type = BlockType.Grassland;

      if (type == BlockType.Scorch)
        type = BlockType.Ashland;

      if (type == BlockType.Tundra)
        type = BlockType.Snow;

      break;

    case ToolAction.Remove:
      if (type == BlockType.Grassland)
        type = BlockType.Dirt;

      if (type == BlockType.Ashland)
        type = BlockType.Scorch;

      if (type == BlockType.Snow)
        type = BlockType.Tundra;

      break;

    case ToolAction.Wet:
      if (type == BlockType.Grassland)
        type = BlockType.Wetland;

      if (type == BlockType.Sand)
        type = BlockType.Dirt;

      break;

    case ToolAction.Dry:
      if (type == BlockType.Dirt)
        type = BlockType.Sand;

      if (type == BlockType.Wetland)
        type = BlockType.Grassland;

      break;

    case ToolAction.Heat:

      if (BlockTypes.InGroup(type, BlockTypes.TempCold)) {
        if (type == BlockType.Snow)
          type = BlockType.Grassland;

        else if (type == BlockType.Ice)
          type = BlockType.Water;

        else if (type == BlockType.IceD)
          type = BlockType.WaterD;
        else
          type = BlockType.Dirt;
      } else if (!BlockTypes.InGroup(type, BlockTypes.TempHot)) {
        if (type == BlockType.Grassland || type == BlockType.Wetland)
          type = BlockType.Ashland;
        else if (type == BlockType.Water)
          type = BlockType.Lava;
        else if (type == BlockType.WaterD)
          type = BlockType.LavaD;
        else
          type = BlockType.Scorch;
      }

      break;

    case ToolAction.Chill:

      if (BlockTypes.InGroup(type, BlockTypes.TempHot)) {
        if (type == BlockType.Ashland)
          type = BlockType.Grassland;
        else if (type == BlockType.Lava)
          type = BlockType.Water;
        else if (type == BlockType.LavaD)
          type = BlockType.WaterD;
        else
          type = BlockType.Dirt;
      } else if (!BlockTypes.InGroup(type, BlockTypes.TempCold)) {
        if (type == BlockType.Grassland || type == BlockType.Wetland)
          type = BlockType.Snow;
        else if (type == BlockType.Water)
          type = BlockType.IceD;
        else if (type == BlockType.WaterD)
          type = BlockType.IceD;
        else
          type = BlockType.Tundra;
      }

      break;

    default:
      break;
    }
  }
}