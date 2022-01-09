
namespace Main.Core.Updates
{
    public interface IRefreshed
    {
        public bool IsActive();
        public void SetNightCacheSystemActive(bool status);
    }

    public interface IInit : IRefreshed
    {
        public void Init();
    }
    
    public interface IRefresh : IRefreshed
    {
        public void Run();
    }

    public interface IFixedRefresh : IRefreshed
    {
        public void FixedRun();
    }

    public interface ILateRefresh : IRefreshed
    {
        public void LateRun();
    }
}