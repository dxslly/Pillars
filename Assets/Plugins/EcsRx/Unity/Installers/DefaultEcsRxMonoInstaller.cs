using Zenject;

namespace EcsRx.Unity.Installers
{
    public class DefaultEcsRxMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            DefaultEcsRxInstaller.Install(Container);
        }
    }
}