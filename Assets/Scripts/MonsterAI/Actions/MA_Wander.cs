using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "MonsterAI/MonsterActions/Wander")]
public class MA_Wander : MonsterAction {
	// MonsterAction: MonsterAI component scripting monster actions.


	// Sets a random destination in the garden and enable movement towards it
	public override void Act(Monster mon) {
		
		// If no destination, pick one randomly
		if(!mon.moving) {
			mon.moving = true;
			float limit = (mon.garden.gardenSize / 2f) - mon.radius;
			mon.moveDestination = new Vector3(Random.Range(-limit, limit), 0, Random.Range(-limit, limit));
		}

		// If monster path isnt valid, update path

		// Get next step on path
		Vector3 step = mon.moveDestination - mon.transform.position;

		// Set next movement to next step
		mon.moveDirection = step;

		// Check if reached
		mon.moveDestination.y = mon.transform.position.y;
		float dist = (mon.moveDestination - mon.transform.position).magnitude;
		if(dist < mon.radius) { mon.currentState.done = true; }

		// State exit procedure
		if(mon.currentState.done) { mon.moving = false; }
	}
}