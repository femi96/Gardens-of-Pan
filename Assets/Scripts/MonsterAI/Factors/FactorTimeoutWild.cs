using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorTimeoutWild : MonsterFactor {
  // AI controller evaluating state priority
  //  Priority based on time (in seconds), after a timeout, only if WILD

  private float m;
  private float timeout;

  public FactorTimeoutWild(float multiplier, float timeout) {

    m = multiplier;
    this.timeout = timeout;
  }

  public FactorTimeoutWild(float multiplier, float timeoutMin, float timeoutMax) {

    m = multiplier;
    timeout = UnityEngine.Random.Range(timeoutMin, timeoutMax);
  }

  public float GetScore(MonsterBehavior behavior) {

    if (behavior.GetMonster().IsOwned()) {
      return -100f;
    } else {
      int totalSeconds = (int)DateTime.Now.Subtract(behavior.GetLastEndedTime()).TotalSeconds;
      return Mathf.Max(0, totalSeconds - timeout) * m;
    }
  }
}
