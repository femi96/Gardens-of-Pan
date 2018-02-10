using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "MonsterAI/MonsterActions/Timeout")]
public class MA_Timeout : MonsterAction {
	// MonsterAction: MonsterAI component scripting monster actions.


	// Variables:
	public float timeout;


	// Ends current state after set time
	public override void Act(Monster mon) {
		if(mon.stateTime > timeout) {
			mon.currentState.done = true;
		}
	}
}