using Zenject;

namespace Project.SinalSystem.Runtime
{
    public class SignalSystemInstaller : MonoInstaller<SignalSystemInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}