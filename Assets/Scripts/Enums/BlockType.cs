using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Global BlockType Enum
public enum BlockType {
  Rough,
  Sand, DirtDry, Dirt, DirtWet,
  Aridland, Grassland, Wetland,
  Tundra, Snow,
  Scorch, Ashland,
  Overgrowth,
  Water, WaterDeep,
  Ice, IceDeep,
  Lava, LavaDeep,
}

public static class BlockTypeGroups {

  public static BlockType[] Ground = new BlockType[] {
    BlockType.Sand, BlockType.DirtDry, BlockType.Dirt, BlockType.DirtWet,
    BlockType.Aridland, BlockType.Grassland, BlockType.Wetland,
    BlockType.Scorch, BlockType.Ashland,
    BlockType.Tundra, BlockType.Snow,
    BlockType.Overgrowth,
  };

  public static BlockType[] GroundBasic = new BlockType[] {
    BlockType.DirtDry, BlockType.Dirt, BlockType.DirtWet,
  };

  public static BlockType[] Shallow = new BlockType[] {
    BlockType.Water,
    BlockType.Ice,
    BlockType.Lava,
  };

  public static BlockType[] Deep = new BlockType[] {
    BlockType.WaterDeep,
    BlockType.IceDeep,
    BlockType.LavaDeep,
  };

  public static BlockType[] Life = new BlockType[] {
    BlockType.Aridland, BlockType.Grassland, BlockType.Wetland,
  };

  public static BlockType[] Dry = new BlockType[] {
    BlockType.DirtDry,
    BlockType.Aridland,
  };

  public static BlockType[] Wet = new BlockType[] {
    BlockType.DirtWet,
    BlockType.Wetland,
  };

  public static BlockType[] Temperate = new BlockType[] {
    BlockType.Sand, BlockType.DirtDry, BlockType.Dirt, BlockType.DirtWet,
    BlockType.Aridland, BlockType.Grassland, BlockType.Wetland,
    BlockType.Overgrowth,
    BlockType.Water, BlockType.WaterDeep,
  };

  public static BlockType[] Hot = new BlockType[] {
    BlockType.Scorch, BlockType.Ashland,
    BlockType.Lava, BlockType.LavaDeep,
  };

  public static BlockType[] Cold = new BlockType[] {
    BlockType.Tundra, BlockType.Snow,
    BlockType.Ice, BlockType.IceDeep,
  };

  public static bool InGroup(BlockType type, BlockType[] typeGroup) {

    foreach (BlockType b in typeGroup) {
      if (type == b)
        return true;
    }

    return false;
  }
}

public static class BlockTypeFeatures {
// Maps block type to a set of features. Features are as follows:
//  int[] features = [height, life, humidity, temp]

  public static int[] GetFeatures(BlockType type) {

    int[] features = new int[] { 0, 0, 0, 0 };

    if (type == BlockType.Rough)
      features[0] = 1;

    if (BlockTypeGroups.InGroup(type, BlockTypeGroups.Shallow))
      features[0] = -1;

    if (BlockTypeGroups.InGroup(type, BlockTypeGroups.Deep))
      features[0] = -2;

    if (BlockTypeGroups.InGroup(type, BlockTypeGroups.Life))
      features[1] = 1;

    if (type == BlockType.Overgrowth)
      features[1] = 2;

    if (BlockTypeGroups.InGroup(type, BlockTypeGroups.Wet))
      features[2] = 1;

    if (BlockTypeGroups.InGroup(type, BlockTypeGroups.Dry))
      features[2] = -1;

    if (type == BlockType.Sand)
      features[2] = -2;

    if (BlockTypeGroups.InGroup(type, BlockTypeGroups.Hot))
      features[3] = 1;

    if (BlockTypeGroups.InGroup(type, BlockTypeGroups.Cold))
      features[3] = -1;

    return features;
  }

  public static BlockType GetType(int[] features) {

    if (features[0] == 0) {

      if (features[3] == 1) {

        if (features[1] >= 1)
          return BlockType.Ashland;

        return BlockType.Scorch;
      }

      if (features[3] == -1) {

        if (features[1] >= 1)
          return BlockType.Snow;

        return BlockType.Tundra;
      }

      if (features[1] == 0) {

        if (features[2] == 1)
          return BlockType.DirtWet;

        if (features[2] == -1)
          return BlockType.DirtDry;

        if (features[2] == -2)
          return BlockType.Sand;

        return BlockType.Dirt;
      }

      if (features[1] == 1) {

        if (features[2] == 1)
          return BlockType.Wetland;

        if (features[2] == -1)
          return BlockType.Aridland;

        return BlockType.Grassland;
      }

      if (features[1] == 2) {

        if (features[2] == 1)
          return BlockType.Wetland;

        if (features[2] == -1)
          return BlockType.Aridland;

        return BlockType.Overgrowth;
      }

      return BlockType.Dirt;
    }

    if (features[0] == -1) {

      if (features[3] == 1)
        return BlockType.Lava;

      if (features[3] == -1)
        return BlockType.Ice;

      return BlockType.Water;
    }

    if (features[0] == -2) {

      if (features[3] == 1)
        return BlockType.LavaDeep;

      if (features[3] == -1)
        return BlockType.IceDeep;

      return BlockType.WaterDeep;
    }

    return BlockType.Rough;
  }
}
