using System.Collections;
using System.Collections.Generic;
    
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
    
using Core;
using Main.Input;

namespace Main.Camera
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class FreeLookInputHandlerMB : SingletonMB<FreeLookInputHandlerMB>
    {
        [SerializeField, Range(0f, 10f)]
        private float m_lookSpeed = 1f;

        [SerializeField]
        private bool m_invertY = true;
        
        private CinemachineFreeLook m_freeLookComponent;
    
        private bool m_isLocked;
    
        public void Start()
        {
            m_freeLookComponent = GetComponent<CinemachineFreeLook>();
        }
    
        public void Update()
        {
            HandleCamera();
        }
    
        public void Lock()
        {
            m_isLocked = true;
        }
    
        public void Unlock()
        {
            m_isLocked = false;
        }

        private void HandleCamera()
        {
            if (m_isLocked) return;
    
            // Get look input (mouse/joystick)
            Vector2 lookMovement = Vector2.zero;
    
            if (m_invertY)
            {
                lookMovement.y = -lookMovement.y;
            }
    
            // Do this because X axis only contains between -180 and 180 instead of 0 and 1 like the Y axis
            lookMovement.x = lookMovement.x * 180f;
    
            //Adjust axis values using look speed and Time.deltaTime so the look doesn't go faster if there is more FPS
            m_freeLookComponent.m_XAxis.Value += lookMovement.x * m_lookSpeed * Time.deltaTime;
            m_freeLookComponent.m_YAxis.Value += lookMovement.y * m_lookSpeed * Time.deltaTime;
        }
    }
}

