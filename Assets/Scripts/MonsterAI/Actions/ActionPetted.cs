using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionPetted : MonsterAction {
  // AI controller scripting monster behaviour
  //  Monsters releases hearts and increases happiness

  private float petHappiness;

  public ActionPetted(float petHappiness) {
    this.petHappiness = petHappiness;
  }

  public override void SetupAction(MonsterBehaviour behaviour, Monster monster) {}

  public override void Act(MonsterBehaviour behaviour, Monster monster) {

    // Setup necessary variables
    Garden garden = monster.garden;

    // Increase happiness
    monster.happyInternal = petHappiness;

    // Release hearts
    garden = garden;

    // End behavior
    monster.EndBehaviour();
  }
}
