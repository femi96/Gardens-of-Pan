using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Unit {
  // Game controller that handles a monster's state and behavior

  private MonsterMover mover;
  private bool owned = false;

  // Monster AI
  public MonsterBehavior currentBehavior;
  public MonsterBehavior[] behaviors;
  private bool currentBehaviorDone = true;

  // Monster models
  private GameObject modelOwned;
  private GameObject modelWild;

  void Awake() {

    // Awake with components
    mover = gameObject.GetComponent<MonsterMover>();
    modelOwned = transform.Find("ModelOwned").gameObject;
    modelWild = transform.Find("ModelWild").gameObject;
  }

  void Start() {

    UpdateBehaviors();
  }

  void Update() {

    if (currentBehaviorDone)
      StartNewBehavior();
    else
      currentBehavior.BehaviorUpdate();
  }

  // Returns if garden meets visit conditions
  public abstract bool CanVisit(Garden garden);

  // Returns if monster can be owned
  public abstract bool CanOwn(Garden garden);

  // Returns if garden board has a valid spawn
  public abstract bool CanSpawn(GardenBoard board);

  // Returns a valid spawn from garden board, given it exists
  public abstract SpawnPoint GetSpawn(GardenBoard board);

  // Returns if monster is owned
  public bool IsOwned() {
    return owned;
  }

  // Set if monster is owned
  public void SetOwned(bool own) {

    if (owned != own)
      UpdateBehaviors();

    owned = own;
    UpdateModel();
  }

  // Returns monster rmover
  public MonsterMover GetMover() {
    return mover;
  }

  // Sets current state to done
  public void BehaviorDone() {
    currentBehaviorDone = true;
  }

  // Chooses a new state, and starts it
  private void StartNewBehavior() {

    bool newState = false;
    float max = float.MinValue;

    foreach (MonsterBehavior b in behaviors) {

      float f = b.GetFactorTotal();

      if (f > max) {
        max = f;
        currentBehavior = b;
        newState = true;
      }
    }

    if (newState) {
      currentBehavior.StartBehavior();
      currentBehaviorDone = false;
    }
  }

  // Create and set monster's behavior states
  public abstract void UpdateBehaviors();

  // Set monster's behavior states to input b
  public void SetBehaviors(MonsterBehavior[] b) {
    behaviors = b;
  }

  // Update monster's model based on monster's owned
  public void UpdateModel() {
    modelOwned.SetActive(owned);
    modelWild.SetActive(!owned);
  }

  // Get set of wild only monster behaviors
  public static MonsterBehavior[] WildBehaviors(Monster m) {

    Garden g = GameObject.Find("Garden").GetComponent<Garden>();
    MonsterBehavior[] wildBehaviors = new MonsterBehavior[] {

      new MonsterBehavior("Leave", g, m,
      new MonsterAction[] {
        new ActionLeave()
      },
      new MonsterFactor[] {
        new FactorTimeout(10f, 30f)
      }),

      new MonsterBehavior("Join", g, m,
      new MonsterAction[] {
        new ActionJoin()
      },
      new MonsterFactor[] {
        new FactorRepeat(10f)
      }),
    };

    return wildBehaviors;
  }
}
