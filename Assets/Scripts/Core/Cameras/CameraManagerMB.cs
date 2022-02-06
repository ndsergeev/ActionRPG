
namespace Main.Cameras
{
    using UnityEngine;
    
    using Cinemachine;
    
    public class CameraManagerMB : MonoBehaviour
    {
        public static CameraManagerMB instance;
        
        //=== Main Camera ===//
        [SerializeField]
        private Camera m_mainCamera;
        public Camera MainCamera => m_mainCamera;
        
        private Transform m_mainCameraTransform;
        public Transform MainCameraTransform => m_mainCameraTransform;
        
        //=== Cinemachine FreeLook Camera ===//
        [SerializeField]
        private CinemachineFreeLook m_cinemachineFreeLook;
        public CinemachineFreeLook CinemachineFreeLook => m_cinemachineFreeLook;

        private void Awake()
        {
            instance = this;
            m_mainCameraTransform = m_mainCamera.transform;
        }
    }
}
