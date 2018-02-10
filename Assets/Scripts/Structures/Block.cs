using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
	// Block: Class for tile blocks. Contains BlockType and variant number
	

	// Block info variables:
	public BlockType type;
	public int variant = 0;


	// Constructor:
	public Block(BlockType t) {
		type = t;
	}
}
