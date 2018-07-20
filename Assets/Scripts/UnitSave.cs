using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSave {
  // Mutable data type for a unit's save data
  //    Need to store all unit fields as public.
  //    This is not data safe so instances of unit save should not be passed around.

  public int prefabID = 0;

  public SerializableVector3 position = Vector3.zero;
  public SerializableQuaternion rotation = Quaternion.identity;

  public UnitSave(Unit u) {
    prefabID = u.prefabID;
    position = u.transform.position;
    rotation = u.transform.rotation;
  }
}