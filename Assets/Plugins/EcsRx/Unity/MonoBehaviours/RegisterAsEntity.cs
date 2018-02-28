using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EcsRx.Unity.Extensions;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Json;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.MonoBehaviours
{
    public class RegisterAsEntity : MonoBehaviour
    {
        [Inject]
        public IPoolManager PoolManager { get; private set; }

        [SerializeField]
        public string PoolName;

        [SerializeField]
        public SerializableEditorEntity EditorEntity = new SerializableEditorEntity();
    
        [Inject]
        public void RegisterEntity()
        {
            if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

            IPool poolToUse;

            if (string.IsNullOrEmpty(PoolName))
            { poolToUse = PoolManager.GetPool(); }
            else if (PoolManager.Pools.All(x => x.Name != PoolName))
            { poolToUse = PoolManager.CreatePool(PoolName); }
            else
            { poolToUse = PoolManager.GetPool(PoolName); }

            var createdEntity = poolToUse.CreateEntity();
            createdEntity.AddComponent(new ViewComponent { View = gameObject });
            SetupEntityBinding(createdEntity, poolToUse);
            SetupEntityComponents(createdEntity);

            Destroy(this);
        }

        private void SetupEntityBinding(IEntity entity, IPool pool)
        {
            var entityBinding = gameObject.AddComponent<EntityView>();
            entityBinding.Entity = entity;
            entityBinding.Pool = pool;
        }

        private void SetupEntityComponents(IEntity entity)
        {
            foreach (var component in EditorEntity.Components) {
                entity.AddComponent(component);
            }
        }
        
        public IPool GetPool()
        {
            return PoolManager.GetPool(PoolName);
        }
    }
}