using EcsRx.Entities;
using EcsRx.Systems;
using EcsRx.Unity.Installers;
using Run.Pug.Pillar.ViewResolvers;
using Zenject;

namespace Run.Pug.Pillar {
  public class GameMonoInstaller : MonoInstaller<GameMonoInstaller> {
    public override void InstallBindings() {
      DefaultEcsRxInstaller.Install(Container);
      Container.Bind<ISystem>().To<SegmentViewResolver>().AsSingle();
    }
  }
}