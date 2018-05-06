using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Global BlockType Enum
public enum BlockType {
  Rough,

  Sand, Dirt,
  Grassland, Wetland,
  Tundra, Snow,
  Scorch, Ashland,

  Water, WaterD,
  Ice, IceD,
  Lava, LavaD,
}

public static class BlockTypes {

  public static BlockType[] DepthGround = new BlockType[] {
    BlockType.Sand, BlockType.Dirt,
    BlockType.Grassland, BlockType.Wetland,
    BlockType.Scorch, BlockType.Ashland,
    BlockType.Tundra, BlockType.Snow,
  };

  public static BlockType[] DepthShallow = new BlockType[] {
    BlockType.Water,
    BlockType.Ice,
    BlockType.Lava,
  };

  public static BlockType[] DepthDeep = new BlockType[] {
    BlockType.WaterD,
    BlockType.IceD,
    BlockType.LavaD,
  };

  public static BlockType[] TempNeutral = new BlockType[] {
    BlockType.Sand, BlockType.Dirt,
    BlockType.Grassland, BlockType.Wetland,
    BlockType.Water, BlockType.WaterD,
  };

  public static BlockType[] TempHot = new BlockType[] {
    BlockType.Scorch, BlockType.Ashland,
    BlockType.Lava, BlockType.LavaD,
  };

  public static BlockType[] TempCold = new BlockType[] {
    BlockType.Tundra, BlockType.Snow,
    BlockType.Ice, BlockType.IceD,
  };

  public static bool InGroup(BlockType type, BlockType[] typeGroup) {

    foreach (BlockType b in typeGroup) {
      if (type == b)
        return true;
    }

    return false;
  }
}
