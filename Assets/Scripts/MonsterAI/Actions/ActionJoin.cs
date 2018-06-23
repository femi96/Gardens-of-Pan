using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionJoin : MonsterAction {
  // AI controller scripting monster behavior
  //  Monster tries to join garden

  private Monster monster;
  private MonsterBehavior behavior;

  private float timeout;

  public ActionJoin(float joinTime) {
    timeout = joinTime;
  }

  public void SetupAction(MonsterBehavior behavior) {

    this.behavior = behavior;
    monster = behavior.monster;
  }

  public void Act() {

    if (!monster.owned && monster.CanOwn()) {

      if (behavior.behaviorTime >= timeout) {
        monster.SetOwned(true);
        behavior.EndBehavior();
      }

    } else {

      behavior.EndBehavior();
    }
  }
}