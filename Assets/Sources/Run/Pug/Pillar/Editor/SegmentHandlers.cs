using System.Collections;
using System.Collections.Generic;
using EcsRx.Components;
using EcsRx.Unity.MonoBehaviours;
using Run.Pug.Pillar;
using Sources.Run.Pug.Pillar.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameObject))]
public class SegmentHandlers : Editor {

  private RegisterAsEntity _registerAsEntity;

  private void OnSceneGUI() {
    Segment segment= ComponentEditorUtils.GetComponentFromTarget<Segment>(target);
    if (null == segment) {
      return;
    }
    
    EditorGUI.BeginChangeCheck();
    Vector3 entrancePosition = Handles.PositionHandle(segment.Entrance, Quaternion.identity);
    Handles.Label(entrancePosition, "Entrance");
    if (EditorGUI.EndChangeCheck()) {
      segment.Entrance = entrancePosition;
    }
    
    EditorGUI.BeginChangeCheck();
    Vector3 exitPosition = Handles.PositionHandle(segment.Exit, Quaternion.identity);
    Handles.Label(exitPosition, "Exit");
    if (EditorGUI.EndChangeCheck()) {
      segment.Exit = exitPosition;
    }
  }
}
