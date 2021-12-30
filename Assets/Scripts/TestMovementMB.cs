
using JetBrains.Annotations;

namespace NAMESPACE
{
    using UnityEngine;
    
    public class TestMovementMB : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1f;
        
        public void Walk(Vector2 walkInput)
        {
            transform.position += new Vector3(walkInput.x, 0, walkInput.y) * speed * Time.deltaTime;
        }
    }
}
