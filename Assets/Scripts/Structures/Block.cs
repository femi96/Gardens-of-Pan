using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockDir {
  None, Forward, Back, Left, Right,
}

[System.Serializable]
public class Block {
  // Mutable data type for tile blocks
  //    Contains a block's type and features

  public BlockType type;

  public int height;
  public int humidity;
  public bool veg;
  public bool soft;

  public BlockDir halfDir;

  // Creates block with BlockType t
  public Block() {
    type = BlockType.Rough;
    height = 0;
    humidity = 0;
    veg = false;
    soft = false;
    halfDir = BlockDir.None;
  }

  // Returns BlockType of block
  public BlockType GetBlockType() {
    return type;
  }

  // Updates block type based on features
  private void UpdateBlockType() {
    if (soft) {
      if (height == -2) {
        type = BlockType.Deep;
      }

      if (height == -1) {
        type = BlockType.Water;
      }

      if (height == 0) {
        if (humidity == -1) {
          type = BlockType.Sand;
        }

        if (humidity == 0) {
          if (veg) {
            type = BlockType.Grass;
          } else {
            type = BlockType.Dirt;
          }
        }

        if (humidity == 1) {
          if (veg) {
            type = BlockType.Wet;
          } else {
            type = BlockType.Mud;
          }
        }
      }
    } else {
      type = BlockType.Rough;
    }
  }

  // Change block based on action
  public void ApplyAction(ToolAction a) {

    switch (a) {

    case ToolAction.Dig:
      if (height > -2) {
        height -= 1;
        humidity = 0;
        veg = false;
      }

      halfDir = BlockDir.None;

      break;

    case ToolAction.Fill:
      if (halfDir != BlockDir.None)
        halfDir = BlockDir.None;
      else if (height < 0) {
        height += 1;
        humidity = 0;
        veg = false;
      }

      break;

    case ToolAction.Soften:
      if (!soft) {
        soft = true;
      }

      break;

    case ToolAction.Grass:
      if (!veg) {
        if (humidity >= 0)
          veg = true;
      }

      break;

    case ToolAction.Remove:
      if (veg) {
        veg = false;
      }

      break;

    case ToolAction.Wet:
      if (humidity <= 0) {
        humidity += 1;
      }

      break;

    case ToolAction.Dry:
      if (humidity >= 0) {
        humidity -= 1;
      }

      if (humidity < 0) {
        veg = false;
      }

      break;

    case ToolAction.Heat:
      break;

    case ToolAction.Chill:
      break;

    default:
      break;
    }

    UpdateBlockType();
  }
}