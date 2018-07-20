using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPrefabsFiller : MonoBehaviour {

  public GameObject[] monsterPrefabs;
  public GameObject[] producePrefabs;
  public GameObject[] plantPrefabs;
  public GameObject[] seedPrefabs;

  public void Fill() {

    for (int i = 0; i < monsterPrefabs.Length; i++) {
      GameObject prefab = monsterPrefabs[i];
      UnitPrefabs.AddPrefab(prefab, i, UnitPrefabCategory.Monster);
    }

    for (int i = 0; i < producePrefabs.Length; i++) {
      GameObject prefab = producePrefabs[i];
      UnitPrefabs.AddPrefab(prefab, i, UnitPrefabCategory.Produce);
    }

    for (int i = 0; i < plantPrefabs.Length; i++) {
      GameObject prefab = plantPrefabs[i];
      UnitPrefabs.AddPrefab(prefab, i, UnitPrefabCategory.Plant);
    }

    for (int i = 0; i < seedPrefabs.Length; i++) {
      GameObject prefab = seedPrefabs[i];
      UnitPrefabs.AddPrefab(prefab, i, UnitPrefabCategory.Seed);
    }
  }
}