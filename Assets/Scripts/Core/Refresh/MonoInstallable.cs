    
namespace Main.Core.System
{
    using UnityEngine;
    
    public abstract class MonoInstallable : MonoBehaviour
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