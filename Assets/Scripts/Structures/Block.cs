using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
	// Block:
	//		Abstract data type for tile blocks
	//		Contains a block's type and visual variant
	

	// Block info variables:
	private BlockType type;
	private int variant = 0;


	// Constructor:
	public Block(BlockType t) {
		type = t;
	}

	public BlockType GetBlockType() {
		return type;
	}

	public void SetBlockType(BlockType t) {
		type = t;
	}

}
