
namespace Main.Core.Updates
{
    public sealed class RefreshEntry : SingletonMB<RefreshEntry>
    {
        private void FixedUpdate() => RefreshCore.FixedRun();
        private void Update() => RefreshCore.Run();
        private void LateUpdate() => RefreshCore.LateRun();
        private void OnDestroy() => RefreshCore.Reset();
    }
}
