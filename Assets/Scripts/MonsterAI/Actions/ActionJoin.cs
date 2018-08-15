using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionJoin : MonsterAction {
  // AI controller scripting monster behaviour
  //  Monster tries to join garden

  private float timeout;

  public ActionJoin(float joinTime) {
    timeout = joinTime;
  }

  public override void SetupAction(MonsterBehaviour behaviour, Monster monster) {}

  public override void Act(MonsterBehaviour behaviour, Monster monster) {

    if (!monster.owned && monster.CanOwn()) {

      if (behaviour.behaviourTime >= timeout) {
        monster.SetOwned(true);
        monster.EndBehaviour();
      }

    } else {

      monster.EndBehaviour();
    }
  }
}