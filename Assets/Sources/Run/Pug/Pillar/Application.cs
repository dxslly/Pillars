using EcsRx.Unity;
using EcsRx.Unity.Components;

namespace Run.Pug.Pillar {
  public class Application : EcsRxApplication {
    protected override void ApplicationStarting() {
      RegisterAllBoundSystems();
    }

    protected override void ApplicationStarted() {
      var defaultPool = PoolManager.GetPool();
      var segmentEntity =  defaultPool.CreateEntity();
      segmentEntity.AddComponent(new ViewComponent());
      segmentEntity.AddComponent(new ViewSegment());
    }
  }
}