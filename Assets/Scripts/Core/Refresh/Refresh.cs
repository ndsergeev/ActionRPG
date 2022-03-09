
namespace Main.Core.Updates
{
    using UnityEngine;
    
    [RequireComponent(typeof(RefreshInstallMachine))]
    public abstract class Refresh : MonoBehaviour, IRefreshed
    {
        private GameObject m_cachedGameObject;
        private Transform m_cachedTransform;
        
        private bool m_systemIsActiveInScene;

        public GameObject CachedGameObject => m_cachedGameObject ??= gameObject;
        public Transform CachedTransform => m_cachedTransform ??= transform;

        public bool IsActive()
            => m_systemIsActiveInScene;

        public void SetNightCacheSystemActive(bool status)
            => m_systemIsActiveInScene = status;
    }
}