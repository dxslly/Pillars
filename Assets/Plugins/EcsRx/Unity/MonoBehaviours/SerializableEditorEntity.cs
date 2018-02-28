using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.EcsRx.Unity.Extensions;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Json;
using UniRx.Operators;
using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours {
  [Serializable]
  public class SerializableEditorEntity : ISerializationCallbackReceiver {

    [Serializable]
    private class SerializedEditorEntity {
      [SerializeField]
      public List<String> ComponentTypes = new List<String>();
      [SerializeField]
      public List<String> ComponentData = new List<String>();
    }

    [SerializeField]
    private SerializedEditorEntity _serizalizedEntity = new SerializedEditorEntity();
    private Dictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();

    public ICollection<Type> ComponentTypes {
      get { return _components.Keys; }
    }
    
    public ICollection<IComponent> Components {
      get { return _components.Values; }
    }

    public bool HasComponent<T>() where T : IComponent {
      return _components.ContainsKey(typeof(T));
    }
    
    public T GetComponent<T>() where T : class, IComponent {
      return _components[typeof(T)] as T;
    }
    
    public void AddComponent(IComponent component) {
      _components.Add(component.GetType(), component);
    }
    
    public void RemoveComponent<T>() where T : IComponent {
      _components.Remove(typeof(T));
    }
    
    public void RemoveComponent(IComponent component) {
      _components.Remove(component.GetType());
    }
    
    public void OnBeforeSerialize() {
      _serizalizedEntity = SerializeEntity(_components);
    }

    public void OnAfterDeserialize() {
      _components = DeserializeComponents(_serizalizedEntity);
    }
    

    private static SerializedEditorEntity SerializeEntity(Dictionary<Type, IComponent> components) {
      SerializedEditorEntity serializedEntity = new SerializedEditorEntity();
      foreach (var component in components.Values) {
        serializedEntity.ComponentData.Add(component.SerializeComponent().ToString());
        serializedEntity.ComponentTypes.Add(component.GetType().AssemblyQualifiedName);
      }
      return serializedEntity;
    }
    
    private static Dictionary<Type, IComponent> DeserializeComponents(SerializedEditorEntity serializedEditorEntity) {
      var components = new Dictionary<Type, IComponent>();
      for (var i = 0; i < serializedEditorEntity.ComponentTypes.Count; i++) {
        var typeName = serializedEditorEntity.ComponentTypes[i];
        var type = Type.GetType(typeName);
        if (type == null) {
          throw new Exception("Cannot resolve type for [" + typeName + "]");
        }
        
        var component = (IComponent)Activator.CreateInstance(type);
        var componentProperties = JSON.Parse(serializedEditorEntity.ComponentData[i]);
        component.DeserializeComponent(componentProperties);

        components.Add(type, component);
      }

      return components;
    }
  }
}