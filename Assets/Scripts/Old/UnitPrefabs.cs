using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitPrefabCategory {
  Monster, Produce, Plant, Seed
}

public static class UnitPrefabs {

  private const int CategorySize = 1000;
  private static Dictionary<int, GameObject> prefabIDMap = new Dictionary<int, GameObject>();

  public static GameObject PrefabFromID(int prefabId) {
    return prefabIDMap[prefabId];
  }

  public static GameObject PrefabFromID(int localId, UnitPrefabCategory category) {
    int prefabId = localId;
    prefabId += (int)category * CategorySize;
    return prefabIDMap[prefabId];
  }

  public static void AddPrefab(GameObject prefab, int localId, UnitPrefabCategory category) {
    int prefabId = localId;
    prefabId += (int)category * CategorySize;
    prefabIDMap[prefabId] = prefab;

    // prefab.GetComponent<Unit>().SetUnitPrefabID(prefabId);
  }
}