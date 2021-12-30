
namespace Main
{
    // Game Engine
    using UnityEngine;
    // Plugins
    using DG.Tweening;
    // In-project scripts assemblies
    using Core;
    
    public class ExampleMB : SingletonMB<ExampleMB>
    {
        protected Transform CachedTransform;
        
        protected override void Awake()
        {
            base.Awake();

            CachedTransform = transform;
            
            CachedTransform.position = Vector3.zero;
            CachedTransform.DOMove(new Vector3(0, 6, 0), 10F)
                .SetEase(ease: Ease.InBack);
        }
    }
}
