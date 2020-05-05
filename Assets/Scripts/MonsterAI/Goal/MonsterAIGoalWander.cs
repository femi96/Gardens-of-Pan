using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAIGoalWander : MonsterAIGoal {

  private int taskCount = 0;
  private int times;

  // Goal to go to x locations in sequence
  public MonsterAIGoalWander(int times) {
    this.times = times;
  }

  private Vector3 GetNearbyDest(Monster mon) {
    Vector3 start = mon.transform.position;
    Vector3 dest = start + 3 * Random.onUnitSphere;
    float limit = (mon.garden.gardenSize / 2f);

    dest = new Vector3(dest.x, 0, dest.z);

    if (dest.x > limit)
      dest = new Vector3(2 * limit - dest.x, dest.y, dest.z);

    if (dest.x < -limit)
      dest = new Vector3((-2 * limit) - dest.x, dest.y, dest.z);

    if (dest.z > limit)
      dest = new Vector3(dest.x, dest.y, 2 * limit - dest.z);

    if (dest.z < -limit)
      dest = new Vector3(dest.x, dest.y, (-2 * limit) - dest.z);

    return dest;
  }

  private Vector3 GetRandomDest(Monster mon) {
    float limit = (mon.garden.gardenSize / 2f);
    Vector3 dest = new Vector3(Random.Range(-limit, limit), 0, Random.Range(-limit, limit));
    return dest;
  }

  public override MonsterAITask Driver(Monster mon) {
    if (taskCount < times * 2) {
      if ((taskCount % 2) == 0) {
        taskCount += 1;
        Vector3 dest = GetNearbyDest(mon);
        return new MonsterAITaskGoTo(dest);
      } else {
        taskCount += 1;
        return new MonsterAITaskWait(5f);
      }
    }

    return new MonsterAITaskNone();
  }

  public override bool IsDone() {
    return taskCount == times * 2;
  }
}
