
namespace Main
{
    using UnityEngine;
    
    using Main.Core;

    public class PlayerMB : CharacterMB
    {
        public void ChangeColorToRandom()
        {
            var rend = GetComponent<Renderer>();
            rend.sharedMaterial.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        
        public void ChangeColorToGrey()
        {
            var rend = GetComponent<Renderer>();
            rend.sharedMaterial.color = new Color(0.5f, 0.5f, 0.5f);
        }

        public bool IsGrounded() => true;
    }
}
