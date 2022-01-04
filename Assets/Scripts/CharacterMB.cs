
using Cinemachine;
using UnityEngine.Serialization;

namespace Main.SomethingToBeRenamed
{
    using System;
    using UnityEngine;
    
    using Main.Core.Console;
    
    public class CharacterMB : MonoBehaviour
    {
        // TODO: e.g. speed
        // [SerializeField]
        // protected ScriptableObject someSharedSettings;

        [SerializeField]
        private CinemachineFreeLook cinemachineFreeLook;
        
        private Transform m_CachedTransform;

        private Quaternion m_Heading;
        private Vector3 m_InputDirection;

        private void Awake()
            => m_CachedTransform = transform;

        public void Move(Vector2 value)
        {
            m_InputDirection = new Vector3(value.x, 0, value.y);
        }

        public void Jump()
            => throw new NotImplementedException();

        private void Update()
        {
            // TODO: move forward based on the camera heading
            m_CachedTransform.Translate(m_InputDirection * Time.deltaTime);
        }
    }
}