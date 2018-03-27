using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
  // Block:
  //    Immutable abstract data type for tile blocks
  //    Contains a block's type and visual variant


  // Block info variables:
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
}