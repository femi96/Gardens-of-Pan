using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorTimeout : MonsterFactor {
  // AI controller evaluating state priority
  //  Priority based on time (in seconds), after a timeout

  private float m;
  private float timeout;

  public FactorTimeout(float multiplier, float timeout) {

    m = multiplier;
    this.timeout = timeout;
  }

  public FactorTimeout(float multiplier, float timeoutMin, float timeoutMax) {

    m = multiplier;
    timeout = UnityEngine.Random.Range(timeoutMin, timeoutMax);
  }

  public float GetScore(MonsterBehavior behavior) {

    float totalSeconds = behavior.GetTimeSinceLastEnd();
    return Mathf.Max(0, totalSeconds - timeout) * m;
  }
}
