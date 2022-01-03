
namespace Main.Core.Updates
{
    using UnityEngine;
    
    public sealed class RefreshEntry : MonoBehaviour
    {
        private void Update()
        {
            RefreshCore.Run();
        }

        private void FixedUpdate()
        {
            RefreshCore.FixedRun();
        }

        private void LateUpdate()
        {
            RefreshCore.LateRun();
        }
        
        private void OnDestroy()
        {
            RefreshCore.Reset();
        }
    }
}