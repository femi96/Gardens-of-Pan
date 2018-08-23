using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionLookAtCamera : MonsterAction {
  // AI controller scripting monster behaviour
  //  Monsters looks towards the camera

  private float frontAngle;
  private float cameraDistance;

  public ActionLookAtCamera(float frontAngle, float cameraDistance) {
    this.frontAngle = frontAngle;
    this.cameraDistance = cameraDistance;
  }

  public override void SetupAction(MonsterBehaviour behaviour, Monster monster) {}

  public override void Act(MonsterBehaviour behaviour, Monster monster) {

    // Setup necessary variables
    GameObject camera = monster.garden.wandCamera;
    GameObject head = monster.monsterHead;

    if (head == null)
      return;

    // If camera in front of and near monster
    //  move camera to look at camera at rate
    // else
    //  move camera to front at rate
    bool inRange = (camera.transform.position - head.transform.position).magnitude <= cameraDistance;
    Quaternion camAngle = Quaternion.LookRotation(camera.transform.position - head.transform.position);
    bool inAngle = Quaternion.Angle(camAngle, monster.transform.rotation) <= frontAngle;
    Quaternion headRotation;

    if (inRange && inAngle) {
      headRotation = camAngle;
    } else {
      headRotation = monster.transform.rotation;
    }

    head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, headRotation, Time.deltaTime * 180f);
  }
}
