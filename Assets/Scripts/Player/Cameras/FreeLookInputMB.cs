    
namespace Main.Input
{
    using UnityEngine;
    
    using Cinemachine;
    
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class FreeLookInputMB : InputMB
    {
        [SerializeField]
        [Range(0f, 10f)]
        private float m_lookSpeed = 1f;

        [SerializeField]
        private bool m_invertY = true;

        private CinemachineFreeLook m_freeLookComponent;
    
        private bool m_isLocked;
    
        public void Awake()
        {
            m_freeLookComponent = GetComponent<CinemachineFreeLook>();
        }
    
        protected override void OnEnable()
        {
            base.OnEnable();
            
            inputReaderSO.onFocusEvent += FocusPressed;
            inputReaderSO.onLookAroundEvent += LookAround;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            inputReaderSO.onFocusEvent -= FocusPressed;
            inputReaderSO.onLookAroundEvent -= LookAround;
        }

        protected void FocusPressed()
            => m_isLocked = !m_isLocked;

        public void Lock(bool value)
            => m_isLocked = value;

        public void LookAround(Vector2 lookAround)
        {
            if (m_isLocked)
                return;
    
            if (m_invertY) // TODO: change
            {
                lookAround.y = -lookAround.y;
            }
    
            // Do this because X axis only contains between -180 and 180 instead of 0 and 1 like the Y axis
            lookAround.x *= 180f;
    
            //Adjust axis values using look speed and Time.deltaTime so the look doesn't go faster if there is more FPS
            m_freeLookComponent.m_XAxis.Value += lookAround.x * m_lookSpeed * Time.deltaTime;
            m_freeLookComponent.m_YAxis.Value += lookAround.y * m_lookSpeed * Time.deltaTime;
        }
    }
}

