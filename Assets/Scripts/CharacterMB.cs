
namespace Main.SomethingToBeRenamed
{
    using System;
    using UnityEngine;
    
    public class CharacterMB : MonoBehaviour
    {
        // TODO: e.g. speed
        // [SerializeField]
        // protected ScriptableObject someSharedSettings;
        
        private Transform m_CachedTransform;

        private void Awake()
            => m_CachedTransform = transform;

        public void Move(Vector2 value)
        {
            var moveValue = new Vector3(value.x, 0, value.y) * Time.deltaTime;
            m_CachedTransform.Translate(moveValue);
            
            Debug.Log("<color=green>Move Changed!</color>");
        }

        public void Jump()
            => throw new NotImplementedException();
    }
}