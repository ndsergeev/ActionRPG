
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

        private static int s_runCount;
        private static int s_fixedRunCount;
        private static int s_lateRunCount;
        
        public static Action onRunCallback;
        public static Action onFixedRunCallback;
        public static Action onLateRunCallback;

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
            for (var i = 0; i < s_runCount; i++)
                if (RunSystems[i].IsActive())
                    RunSystems[i].Run();
            
            onRunCallback?.Invoke();
        }

        public static void FixedRun()
        {
            for (var i = 0; i < s_fixedRunCount; i++)
                if (FixedRunSystems[i].IsActive())
                    FixedRunSystems[i].FixedRun();
            
            onFixedRunCallback?.Invoke();
        }

        public static void LateRun()
        {
            for (var i = 0; i < s_lateRunCount; i++)
                if (LateRunSystems[i].IsActive())
                    LateRunSystems[i].LateRun();
            
            onLateRunCallback?.Invoke();
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
            onRunCallback?.SetNull();
            onFixedRunCallback?.SetNull();
            onLateRunCallback?.SetNull();
        }
        
        private static void UpdateCounts()
        {
            s_runCount = RunSystems.Count;
            s_fixedRunCount = FixedRunSystems.Count;
            s_lateRunCount = LateRunSystems.Count;
        }
    }
}