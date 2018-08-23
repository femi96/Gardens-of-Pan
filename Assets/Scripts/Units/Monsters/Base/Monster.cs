using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Unit {
  // Game controller that handles a monster's state and behaviour

  [Header("Monster Fields")]
  public EntityMover mover;
  public bool owned = false;

  // Monster models
  public GameObject monsterHead;
  private GameObject modelOwned;
  private GameObject modelWild;

  // Monster data
  public const float HappyMax = 10f;
  public const float HappyMin = -10f;
  public const float HappyPerSec = 0.1f;
  public const float HappyInternalPerSecDecay = 0.1f / 3f;
  public float happy = 0f;
  public float happyWell = 0f;
  public float happyInternal = 0f;
  public float happyTime = 0f;

  // Monster AI
  [Header("MonsterAI Fields")]
  public MonsterBehaviour currentBehaviour;
  public MonsterBehaviour[] behaviours;
  public bool currentBehaviourDone = true;

  public override void Awake() {
    base.Awake();
    mover = gameObject.GetComponent<EntityMover>();
    modelOwned = transform.Find("ModelOwned").gameObject;
    modelWild = transform.Find("ModelWild").gameObject;
    SetBehaviours();
  }

  void Update() {
    // Update Behaviour
    if (currentBehaviourDone)
      StartNewBehaviour();

    foreach (MonsterBehaviour behaviour in behaviours)
      behaviour.timeSinceLastEnd += Time.deltaTime;

    currentBehaviour.BehaviourUpdate(this);

    // Update Happy
    UpdateHappy();
  }

  // Returns if garden meets visit conditions
  public abstract bool CanVisit();

  // Returns if monster can be owned
  public abstract bool CanOwn();

  // Returns if garden board has a valid spawn
  public abstract bool CanSpawn();

  // Returns a valid spawn from garden board, given it exists
  public abstract SpawnPoint GetSpawn();

  // Set if monster is owned
  public void SetOwned(bool own) {

    owned = own;
    modelOwned.SetActive(owned);
    modelWild.SetActive(!owned);
  }

  // Monster Happiness
  // ====================================

  // Update the monster's happiness
  public void UpdateHappy() {
    float happyInteralDelta = Mathf.Sign(-happyInternal) * HappyInternalPerSecDecay * Time.deltaTime;
    happyInteralDelta = Mathf.Clamp(happyInteralDelta, -Mathf.Abs(happyInternal), Mathf.Abs(happyInternal));
    happyInternal += happyInteralDelta;
    happyInternal = Mathf.Clamp(happyInternal, HappyMin, HappyMax);

    happyTime += Time.deltaTime;

    if (happyTime >= 5f) {
      happyTime -= 5f;
      UpdateHappyWell();
    }

    float happyWellDelta = happyWell - happy;
    float happyDelta = Mathf.Sign(happyWellDelta) * HappyPerSec * Time.deltaTime;
    happyDelta = Mathf.Clamp(happyDelta, -Mathf.Abs(happyWellDelta), Mathf.Abs(happyWellDelta));
    happy += happyDelta;
    happy = Mathf.Clamp(happy, HappyMin, HappyMax);
  }

  // Update the monster's happiness well
  public void UpdateHappyWell() {
    float happyTemp = 0f;
    happyTemp += GetHappyExternal();
    happyTemp += happyInternal;
    happyWell = happyTemp;
  }

  public abstract float GetHappyExternal();

  // Monster Behaviours
  // ====================================

  // Chooses a new state, and starts it
  private void StartNewBehaviour() {

    bool newState = false;
    float maxPriority = float.MinValue;

    foreach (MonsterBehaviour behaviour in behaviours) {

      behaviour.behavioursSince += 1;

      // Is behaviour valid
      if (behaviour.IsResticted(this))
        continue;

      // Find highest factor behaviour
      float priority = behaviour.GetPriority(this);

      if (priority > maxPriority) {
        maxPriority = priority;
        currentBehaviour = behaviour;
        newState = true;
      }
    }

    if (newState) {
      currentBehaviour.StartBehaviour(this);
      currentBehaviourDone = false;
    }
  }

  // End this monsters current behavior
  public void EndBehaviour() {
    currentBehaviour.timeSinceLastEnd = 0;
    currentBehaviourDone = true;
  }

  // Set monster's behaviour states
  public void SetBehaviours() {
    behaviours = Behaviours();
  }

  // Interrupt a monster's behavior
  public void InterruptBehaviour(float interruptPriority) {
    if (currentBehaviour.interruptPriority <= interruptPriority)
      EndBehaviour();
  }

  // Get set of monster behaviour states based on monster type
  public abstract MonsterBehaviour[] Behaviours();

  // SAVING/LOADING monster
  // ====================================

  public override UnitSave GetUnitSave() {
    UnitSave save = new MonsterSave(this);
    return save;
  }

  public override void SetFromSave(UnitSave save) {
    MonsterSave m = (MonsterSave)save;
    SetOwned(m.owned);

    behaviours = m.behaviours;
    currentBehaviour = behaviours[m.currentBehaviourIndex];
    currentBehaviourDone = m.currentBehaviourDone;
  }
}