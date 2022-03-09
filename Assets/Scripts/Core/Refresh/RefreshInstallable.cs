
namespace Main.Core.Updates
{
    public abstract class RefreshInstallable : Refresh
    {
        private bool m_installedOnEnable;
        
        private void OnEnable()
        {
            OnPreEnable();
            if (!m_installedOnEnable)
            {
                OnFirstEnable();
                m_installedOnEnable = true;
            }
            OnLateEnable();
        }
        
        protected virtual void OnPreEnable() { }
        protected virtual void OnLateEnable() { }
        protected abstract void OnFirstEnable();
    }
}