using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

public class GardenWild : MonoBehaviour {
  // Game controller that handles garden's wilderness data
  //    stores wild monsters, all possible visiting monsters
  //    stores visit timer, for when monsters visit
  //    pushes new monsters and their spawn point to the garden

  // Assigned in Editor:
  public GameObject[] wildMonsterPrefabs; // Monsters that can visit garden

  private Garden garden;
  private GardenBoard gardenBoard;

  private float visitTime = 0f;
  private int monsterInd = 0;

  void Awake() {

    // Awake with components
    garden = GetComponent<Garden>();
    gardenBoard = GetComponent<GardenBoard>();
  }

  void Start() {}

  void Update() {

    // Visit
    visitTime += Time.deltaTime;

    if (visitTime >= 1f) {
      visitTime -= 1f;
      TryAddWildMonster();
    }
  }

  void FixedUpdate() {
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
    }

    monsterInd += 1;
  }
}
