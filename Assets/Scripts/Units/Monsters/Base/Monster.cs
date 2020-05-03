using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Unit {
  // Handles a monster's state and behaviour

  [Header("Monster Fields")]
  public EntityMover mover;
  public bool joined = false;

  // Monster models
  public GameObject monsterHead;
  private GameObject modelJoined;
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
  public bool interrupt = false;
  private MonsterAIMotivation[] motivations;

  public bool hasGoal = false;
  public MonsterAIGoal goal;

  public bool hasTask = false;
  public MonsterAITask task;

  public override void Awake() {
    base.Awake();
    mover = gameObject.GetComponent<EntityMover>();
    modelJoined = transform.Find("ModelJoined").gameObject;
    modelWild = transform.Find("ModelWild").gameObject;
  }

  void Start() {
    motivations = new MonsterAIMotivation[] {
      new MonsterAIMotivationIdle(),
      new MonsterAIMotivationJoin(),
      new MonsterAIMotivationLeave()
    };
  }

  void Update() {
    // Update AI
    UpdateAI();

    // Update Happy
    UpdateHappy();
  }

  // Returns if garden meets visit conditions
  public abstract bool CanVisit();

  // Returns if monster can be joined
  public abstract bool CanJoin();

  // Returns if garden board has a valid spawn
  public abstract bool CanSpawn();

  // Returns a valid spawn from garden board, given it exists
  public abstract SpawnPoint GetSpawn();

  // Set if monster is joined
  public void SetJoined(bool join) {
    joined = join;
    modelJoined.SetActive(joined);
    modelWild.SetActive(!joined);
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

  // Monster AI
  // ====================================

  // Update function for monster AI
  private void UpdateAI() {
    if (interrupt) {
      Interrupt();
    }

    if (hasGoal) {
      if (hasTask) {
        MonsterAITaskStatus taskStatus = task.Do(this);

        switch (taskStatus) {
        case MonsterAITaskStatus.Complete:
          hasTask = false;
          break;

        case MonsterAITaskStatus.Failed:
          hasTask = false;
          hasGoal = false;
          break;

        default:
          break;
        }
      } else {
        GetNewTask();
      }
    } else {
      GetNewGoal();
    }
  }

  // Interrupts current goal/task
  private void Interrupt() {
    hasGoal = false;
    hasTask = false;
    interrupt = false;
  }

  private void GetNewTask() {

    if (goal.IsDone()) {
      hasGoal = false;
    } else {
      task = goal.Driver(this);
      hasTask = true;
    }
  }

  private void GetNewGoal() {
    float bestPriority = 0;
    MonsterAIMotivation bestMotivation = null;

    foreach (MonsterAIMotivation m in motivations) {
      float p = m.GetPriority(this);

      if (p > bestPriority) {
        bestMotivation = m;
        bestPriority = p;
      }
    }

    if (bestMotivation != null) {
      goal = bestMotivation.GenerateGoal(this);
      hasGoal = true;
    }
  }

  // Saving/Loading monster
  // ====================================

  public override UnitSave GetUnitSave() {
    UnitSave save = new MonsterSave(this);
    return save;
  }

  public override void SetFromSave(UnitSave save) {
    MonsterSave m = (MonsterSave)save;
    SetJoined(m.joined);

    hasGoal = m.hasGoal;
    goal = m.goal;

    hasTask = m.hasTask;
    task = m.task;
  }
}