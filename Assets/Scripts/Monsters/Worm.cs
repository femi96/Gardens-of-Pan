using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Monster {
	// Worm: Class of Worm

	// Returns if garden meets visit conditions
	public override bool CanVisit(Garden garden) {
		return true;
	}

	// Updates unit size
	public override void UpdateSize() {
		size = 1;
	}
}
