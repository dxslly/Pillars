using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EcsRx.Unity.Extensions;
using EcsRx.Components;
using EcsRx.Unity.Components;
using EcsRx.Unity.Helpers.Extensions;
using EcsRx.Unity.Helpers.UIAspects;
using EcsRx.Unity.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace EcsRx.Unity.Helpers {
  [Serializable]
  [CustomEditor(typeof(RegisterAsEntity))]
  public class RegisterAsEntityInspector : Editor {
    private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass && !typeof(ViewComponent).IsAssignableFrom(p));

    private SerializableEditorEntity _editorEntity;
    private RegisterAsEntity _registerAsEntity;

    private bool showComponents;

    private void PoolSection() {
      this.UseVerticalBoxLayout(() => {
        _registerAsEntity.PoolName = this.WithTextField("Pool: ", _registerAsEntity.PoolName);
      });
    }

    private void ComponentListings() {
      EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
      this.WithHorizontalLayout(() => {
        this.WithLabel("Components (" + _editorEntity.Components.Count + ")");
        if (this.WithIconButton("▸")) showComponents = false;
        if (this.WithIconButton("▾")) showComponents = true;
      });

      var componentsToRemove = new HashSet<IComponent>();
      var componentCount = _editorEntity.Components.Count;
      if (showComponents)
        foreach (var component in _editorEntity.Components) {
          this.UseVerticalBoxLayout(() => {
            var type = component.GetType();

            this.WithVerticalLayout(() => {
              this.WithHorizontalLayout(() => {
                if (this.WithIconButton("-")) componentsToRemove.Add(component);

                this.WithLabel(type.Name);
              });

              EditorGUILayout.LabelField(type.Namespace);
              EditorGUILayout.Space();
            });

            ComponentUIAspect.ShowComponentProperties(component);
          });
        }

      EditorGUILayout.EndVertical();
      
      foreach (var component in componentsToRemove) {
        _editorEntity.RemoveComponent(component);
      }
    }


    private void ComponentSelectionSection() {
      this.UseVerticalBoxLayout(() => {
        var availableTypes = allComponentTypes
            .Where(type => !_editorEntity.ComponentTypes.Contains(type))
            .ToArray();

        var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();
        var index = -1;
        index = EditorGUILayout.Popup("Add Component", index, types);
        if (index >= 0) {
          var selectedComponentType= availableTypes.ElementAt(index);
          IComponent componentInstance = (IComponent) Activator.CreateInstance(selectedComponentType);
          _editorEntity.AddComponent(componentInstance);
        }
      });
    }

    private void PersistChanges() {
      if (GUI.changed) this.SaveActiveSceneChanges();
    }

    public override void OnInspectorGUI() {
      _registerAsEntity = (RegisterAsEntity) target;
      _editorEntity = _registerAsEntity.EditorEntity;

      PoolSection();
      EditorGUILayout.Space();
      ComponentSelectionSection();
      ComponentListings();
      PersistChanges();
    }
  }
}