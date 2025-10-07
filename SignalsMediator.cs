using System;
using System.Linq;
using Zenject;
namespace Application.Core
{
    public static class SignalsMediator
    {
        public static void InstallSignalBindings<T>(this DiContainer container) where T : ISignal
        {
            var signals = AppDomain.CurrentDomain.GetAssemblies().SelectMany(_ => _.GetTypes()).Where(_ =>
                typeof(T).IsAssignableFrom(_) && _.IsAbstract == false && _.IsInterface == false);

            foreach (var signal in signals)
            {
                container.DeclareSignal(signal).OptionalSubscriberWithWarning();
            }
        }
    }

    public interface ISignal
    {
        
    }
}