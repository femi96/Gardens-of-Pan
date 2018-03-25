using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

public class Wild : MonoBehaviour {
  // Wild:
  //		Controller that handles wilderness data.
  //		Contains wild monsters and visit times.


  // Assigned in Editor:
  public GameObject[] wildMonsterPrefabs;

  // Wild monster variables
  private int monsterInd = 0;
  private Garden garden;

  // Visit variables:
  private int visitTime = 0;

  // Unity MonoBehavior Functions:
  void Awake() {

    // Awake with components
    garden = GetComponent<Garden>();
  }

  void Start() {}

  void Update() {}

  void FixedUpdate() {

    // Visit
    if (visitTime == 50) {
      visitTime -= 10;
      GetWildMonster();
    } else {
      visitTime += 1;
    }
  }


  // Tries to add a wild monster to the garden
  public void GetWildMonster() {

    // Wrap wild monster index
    if (monsterInd >= wildMonsterPrefabs.Length) {
      monsterInd = 0;
      return;
    }

    // Add monster if there is space and requirement is met
    GameObject monster = wildMonsterPrefabs[monsterInd];

    if (monster.GetComponent<Monster>().RoomInGarden(garden) && monster.GetComponent<Monster>().CanVisit(garden)) {
      garden.AddMonster(monster);
      visitTime = 0;
    }

    monsterInd += 1;
  }
}
