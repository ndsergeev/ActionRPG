namespace Main.Cameras
{
    using System;
    
    using UnityEngine;
    
    using Cinemachine;
    
    using Core;
    
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
        private CinemachineFreeLook m_CinemachineFreeLook;
        public CinemachineFreeLook CinemachineFreeLook => m_CinemachineFreeLook;

        private void Awake()
        {
            instance = this;

            m_mainCameraTransform = m_mainCamera.transform;
        }
    }
}
