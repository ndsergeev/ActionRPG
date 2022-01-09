
namespace Main.Core.Updates
{
    using UnityEngine;
    
    [RequireComponent(typeof(RefreshInstallMachine))]
    public abstract class Refresh : MonoBehaviour, IRefreshed
    {
        private GameObject m_CachedGameObject;
        private Transform m_CachedTransform;
        
        private bool m_SystemIsActiveInScene;

        public GameObject cachedGameObject => m_CachedGameObject ??= gameObject;
        public Transform cachedTransform => m_CachedTransform ??= transform;

        public bool IsActive()
            => m_SystemIsActiveInScene;

        public void SetNightCacheSystemActive(bool status)
            => m_SystemIsActiveInScene = status;
    }
}