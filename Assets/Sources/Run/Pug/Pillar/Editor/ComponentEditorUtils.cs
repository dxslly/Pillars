using EcsRx.Components;
using EcsRx.Unity.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace Sources.Run.Pug.Pillar.Editor {
  public static class ComponentEditorUtils {
    public static C GetComponentFromTarget<C>(object target) where C : class, IComponent {
      if (!(target is GameObject)) {
        return null;
      }

      RegisterAsEntity registerAsEntity = (target as GameObject).GetComponent<RegisterAsEntity>();
      if (null == registerAsEntity) {
        return null;
      }

      SerializableEditorEntity editorEntity = registerAsEntity.EditorEntity;
      if (!editorEntity.HasComponent<C>()) {
        return null;
      }

      return editorEntity.GetComponent<C>();
    }
  }
}