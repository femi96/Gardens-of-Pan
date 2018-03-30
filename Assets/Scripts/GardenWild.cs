using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

public class GardenWild : MonoBehaviour {
  // GardenWild:
  //    Controller that handles wilderness data.
  //    Manages visiting monsters, occasionally
  //      pushing a monster and its spawn point to the garden


  // Assigned in Editor:
  public GameObject[] wildMonsterPrefabs;

  // Monster variables
  private int monsterInd = 0;
  private Garden garden;
  private GardenBoard gardenBoard;

  // Visit variables:
  private int visitTime = 0;

  // Unity MonoBehavior Functions:
  void Awake() {

    // Awake with components
    garden = GetComponent<Garden>();
    gardenBoard = GetComponent<GardenBoard>();
  }

  void Start() {}

  void Update() {}

  void FixedUpdate() {

    // Visit
    if (visitTime == 50) {
      visitTime -= 10;
      TryAddWildMonster();
    } else {
      visitTime += 1;
    }
  }


  // Tries to add a wild monster to the garden
  private void TryAddWildMonster() {

    // Wrap wild monster index
    if (monsterInd >= wildMonsterPrefabs.Length) {
      monsterInd = 0;
      return;
    }

    // Add monster if there is space and requirement is met
    GameObject monsterGo = wildMonsterPrefabs[monsterInd];
    Monster monster = monsterGo.GetComponent<Monster>();

    bool roomInGarden = monster.RoomInGarden(garden);
    bool canVisit = monster.CanVisit(garden);
    bool canSpawn = monster.CanSpawn(gardenBoard);

    if (roomInGarden && canVisit && canSpawn) {
      SpawnPoint spawn = monster.GetSpawn(gardenBoard);
      garden.AddMonster(monsterGo, spawn);
      visitTime = 0;
    }

    monsterInd += 1;
  }
}
