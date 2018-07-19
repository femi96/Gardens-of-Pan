using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionJoin : MonsterAction {
  // AI controller scripting monster behaviour
  //  Monster tries to join garden

  private Monster monster;
  private MonsterBehaviour behaviour;

  private float timeout;

  public ActionJoin(float joinTime) {
    timeout = joinTime;
  }

  public void SetupAction(MonsterBehaviour behaviour) {

    this.behaviour = behaviour;
    monster = behaviour.monster;
  }

  public void Act() {

    if (!monster.owned && monster.CanOwn()) {

      if (behaviour.behaviourTime >= timeout) {
        monster.SetOwned(true);
        behaviour.EndBehaviour();
      }

    } else {

      behaviour.EndBehaviour();
    }
  }
}