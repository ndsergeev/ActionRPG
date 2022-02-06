
namespace Main.Cameras
{
    using UnityEngine;
    
    using Cinemachine;
    
    public class CameraManagerMB : MonoBehaviour
    {
        public static CameraManagerMB Instance;
        
        //=== Main Camera ===//
        [SerializeField]
        private Camera m_MainCamera;
        public Camera mainCamera => m_MainCamera;
        
        private Transform m_MainCameraTransform;
        public Transform MainCameraTransform => m_MainCameraTransform;
        
        //=== Cinemachine FreeLook Camera ===//
        
        [SerializeField]
        private CinemachineFreeLook m_CinemachineFreeLook;
        public CinemachineFreeLook cinemachineFreeLook => m_CinemachineFreeLook;

        private void Awake()
        {
            Instance = this;

            m_MainCameraTransform = m_MainCamera.transform;
        }
    }
}
