using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ControlIconsData", menuName = "ControlIcons")]
public class InteractionIcons : ScriptableObject
{
    // Keyboard images
    public Sprite eIcon; // Interact image
    public Sprite spaceIcon;


    // Gamepad Images
    public Sprite northButtonIcon;
    public Sprite southButtonIcon;

   
}
