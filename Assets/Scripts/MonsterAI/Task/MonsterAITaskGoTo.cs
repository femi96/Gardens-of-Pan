using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAITaskGoTo : MonsterAITask {
// Agent goes to voxel

  private SerializableVector3 dest;
  private List<SerializableVector3> pathToDest;

  private static float PointSize = 0.1f;

  public MonsterAITaskGoTo(Vector3 dest) {
    this.dest = dest;
  }

  private bool AtPoint(Monster mon, Vector3 b) {
    Vector3 a = mon.transform.position;
    Vector3 c = new Vector3(a.x - b.x, 0, a.z - b.z);
    return c.magnitude < PointSize;
  }

  public override MonsterAITaskStatus Do(Monster mon) {

    // If at destination, complete
    if (AtPoint(mon, dest))
      return MonsterAITaskStatus.Complete;

    // If no valid path, generate new path
    if (pathToDest == null) {
      List<Vector3> path = mon.garden.GetBoard().GetPath(mon, dest);
      pathToDest = new List<SerializableVector3>();

      foreach (Vector3 v in path)
        pathToDest.Add(v);
    }

    // Move to voxel along path
    mon.mover.MoveTo(pathToDest[0]);

    // If at next voxel in path, remove it
    if (AtPoint(mon, pathToDest[0])) {
      pathToDest.RemoveAt(0);
    }

    return MonsterAITaskStatus.Ongoing;
  }
}
