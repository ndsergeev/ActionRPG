
using System;
using System.Collections.Generic;

namespace Main.Core.Updates
{
    using Main.Core.System;
    
    public static class RefreshCore
    {
        private static readonly List<IRefresh> RunSystems = new List<IRefresh>(256);
        private static readonly List<IFixedRefresh> FixedRunSystems = new List<IFixedRefresh>(64);
        private static readonly List<ILateRefresh> LateRunSystems = new List<ILateRefresh>(64);

        private static int _runCount;
        private static int _fixedRunCount;
        private static int _lateRunCount;
        
        public static Action OnRun;
        public static Action OnFixedRun;
        public static Action OnLateRun;

        public static void AddSystem(IRefreshed refreshed)
        {
            switch (refreshed)
            {
                case IRefresh runSystem:
                    RunSystems.Add(runSystem);
                    break;
                case IFixedRefresh fixedRunSystem:
                    FixedRunSystems.Add(fixedRunSystem);
                    break;
                case ILateRefresh lateRunSystem:
                    LateRunSystems.Add(lateRunSystem);
                    break;
            }

            UpdateCounts();
        }

        public static void RemoveSystem(IRefreshed refreshed)
        {
            switch (refreshed)
            {
                case IRefresh runSystem:
                    RunSystems.Remove(runSystem);
                    break;
                case IFixedRefresh fixedRunSystem:
                    FixedRunSystems.Remove(fixedRunSystem);
                    break;
                case ILateRefresh lateRunSystem:
                    LateRunSystems.Remove(lateRunSystem);
                    break;
            }

            UpdateCounts();
        }
        
        public static void Run()
        {
            for (var i = 0; i < _runCount; i++)
                if (RunSystems[i].IsActive())
                    RunSystems[i].Run();
            
            OnRun?.Invoke();
        }

        public static void FixedRun()
        {
            for (var i = 0; i < _fixedRunCount; i++)
                if (FixedRunSystems[i].IsActive())
                    FixedRunSystems[i].FixedRun();
            
            OnFixedRun?.Invoke();
        }

        public static void LateRun()
        {
            for (var i = 0; i < _lateRunCount; i++)
                if (LateRunSystems[i].IsActive())
                    LateRunSystems[i].LateRun();
            
            OnLateRun?.Invoke();
        }

        public static void Reset()
        {
            ResetLists();
            ResetActions(); 
            UpdateCounts();
        }

        private static void ResetLists()
        {
            RunSystems?.Clear();
            FixedRunSystems?.Clear();
            LateRunSystems?.Clear();
        }

        private static void ResetActions()
        {
            OnRun?.SetNull();
            OnFixedRun?.SetNull();
            OnLateRun?.SetNull();
        }
        
        private static void UpdateCounts()
        {
            _runCount = RunSystems.Count;
            _fixedRunCount = FixedRunSystems.Count;
            _lateRunCount = LateRunSystems.Count;
        }
    }
}