using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTimeout : MonsterAction {
  // AI controller scripting monster behaviour
  //  Waits a duration and ends state

  private MonsterBehaviour behaviour;

  private float timeout;

  public ActionTimeout(float timeout) {
    this.timeout = timeout;
  }

  public ActionTimeout(float timeoutMin, float timeoutMax) {
    timeout = Random.Range(timeoutMin, timeoutMax);
  }

  public override void SetupAction(MonsterBehaviour behaviour) {

    this.behaviour = behaviour;
  }

  public override void Act() {

    if (behaviour.behaviourTime >= timeout) {
      behaviour.EndBehaviour();
    }
  }
}