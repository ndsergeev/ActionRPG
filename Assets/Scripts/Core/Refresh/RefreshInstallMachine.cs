
using System.Collections.Generic;

namespace Main.Core.Updates
{
    using Main.Core.System;
    
    public sealed class RefreshInstallMachine : MonoInstallable
    {
        private readonly List<IRefreshed> m_systems = new List<IRefreshed>(8);

        protected override void OnFirstEnable()
            => InstallNewSystems();

        private void InstallNewSystems()
        {
            GetComponents(m_systems);
            InitializeSystems();
            foreach (var system in m_systems)
                RefreshCore.AddSystem(system);
        }
        
        private void RemoveSystems()
        {
            foreach (var system in m_systems)
                RefreshCore.RemoveSystem(system);
        }
        
        private void InitializeSystems()
        {
            foreach (var system in m_systems)
                if (system is IInit initSystem)
                    initSystem.Init();
        }

        protected override void OnLateEnable()
        {
            foreach (var system in m_systems)
                system.SetNightCacheSystemActive(true);
        }

        private void OnDisable()
        {
            foreach (var system in m_systems)
                system.SetNightCacheSystemActive(false);
        }

        private void OnDestroy()
            => RemoveSystems();
    }
}