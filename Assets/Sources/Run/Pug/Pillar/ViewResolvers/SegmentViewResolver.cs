using System;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Unity.Components;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Run.Pug.Pillar.ViewResolvers {
  public class SegmentViewResolver : ViewResolverSystem {

    private readonly GameObject _segmentPrefab;
    
    public SegmentViewResolver(IViewHandler viewHandler) : base(viewHandler) {
      _segmentPrefab = Resources.Load<GameObject>("CubeSegment");
    }

    public override IGroup TargetGroup {
      get { return new GroupBuilder().WithComponent<ViewComponent>().WithComponent<ViewSegment>().Build(); }
    }

    public override GameObject ResolveView(IEntity entity) {
      return ViewHandler.InstantiateAndInject(_segmentPrefab);
    }
  }
}