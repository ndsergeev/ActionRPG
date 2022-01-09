
using System.Collections.Generic;

namespace Main.Core.Updates
{
    using Main.Core.System;
    
    public sealed class RefreshInstallMachine : MonoInstallable
    {
        private readonly List<IRefreshed> m_Systems = new List<IRefreshed>(8);

        protected override void OnFirstEnable()
            => InstallNewSystems();

        private void InstallNewSystems()
        {
            GetComponents(m_Systems);
            InitializeSystems();
            foreach (var system in m_Systems)
                RefreshCore.AddSystem(system);
        }
        
        private void RemoveSystems()
        {
            foreach (var system in m_Systems)
                RefreshCore.RemoveSystem(system);
        }
        
        private void InitializeSystems()
        {
            foreach (var system in m_Systems)
                if (system is IInit initSystem)
                    initSystem.Init();
        }

        protected override void OnLateEnable()
        {
            foreach (var system in m_Systems)
                system.SetNightCacheSystemActive(true);
        }

        private void OnDisable()
        {
            foreach (var system in m_Systems)
                system.SetNightCacheSystemActive(false);
        }

        private void OnDestroy()
            => RemoveSystems();
    }
}