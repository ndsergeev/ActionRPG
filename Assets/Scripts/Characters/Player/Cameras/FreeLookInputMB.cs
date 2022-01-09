    
namespace Main.Input
{
    using UnityEngine;
    
    using Cinemachine;
    
    using Main.Inputs;
    
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class FreeLookInputMB : InputMB
    {
        [SerializeField]
        [Range(0f, 10f)]
        private float m_LookSpeed = 1f;

        [SerializeField]
        private bool m_InvertY = true;

        private CinemachineFreeLook m_FreeLookComponent;
    
        private bool m_IsLocked;
    
        public void Awake()
        {
            m_FreeLookComponent = GetComponent<CinemachineFreeLook>();
        }
    
        protected override void OnEnable()
        {
            base.OnEnable();
            
            InputReader.onFocusEvent += FocusPressed;
            InputReader.onLookAroundEvent += LookAround;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            InputReader.onFocusEvent -= FocusPressed;
            InputReader.onLookAroundEvent -= LookAround;
        }

        protected void FocusPressed()
            => m_IsLocked = !m_IsLocked;

        public void Lock(bool value)
            => m_IsLocked = value;

        public void LookAround(Vector2 lookAround)
        {
            const float degPI = 180f; 
            
            if (m_IsLocked)
                return;
    
            if (m_InvertY) // TODO: invert should be controlled not in this class
            {
                lookAround.y = -lookAround.y;
            }
    
            // Do this because X axis only contains between -180 and 180 instead of 0 and 1 like the Y axis
            lookAround.x *= degPI;
    
            //Adjust axis values using look speed and Time.deltaTime so the look doesn't go faster if there is more FPS
            m_FreeLookComponent.m_XAxis.Value += lookAround.x * m_LookSpeed * Time.deltaTime;
            m_FreeLookComponent.m_YAxis.Value += lookAround.y * m_LookSpeed * Time.deltaTime;
        }
    }
}

