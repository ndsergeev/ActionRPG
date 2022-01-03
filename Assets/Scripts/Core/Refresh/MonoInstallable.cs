    
namespace Main.Core.System
{
    using UnityEngine;
    
    public abstract class MonoInstallable : MonoBehaviour
    {
        private bool m_InstalledOnEnable;
        
        private void OnEnable()
        {
            OnPreEnable();
            if (!m_InstalledOnEnable)
            {
                OnFirstEnable();
                m_InstalledOnEnable = true;
            }
            OnLateEnable();
        }
        
        protected virtual void OnPreEnable() { }
        protected virtual void OnLateEnable() { }
        protected abstract void OnFirstEnable();
    }
}