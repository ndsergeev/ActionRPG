using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDynamicBones : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private DynamicBone tailBone;
    [SerializeField]
    private DynamicBone earBoneLeft;
    [SerializeField]
    private DynamicBone earBoneRight;

    private bool inert; // If true, bones won't be dynamic

    private void Update()
    {
        HandleInertness();
    }

    void HandleInertness()
    {
        if (!inert && player.Movement.onPlatform)
        {
            tailBone.m_Inert = 1f;
            tailBone.UpdateParameters();
            earBoneLeft.m_Inert = 1f;
            earBoneLeft.UpdateParameters();
            earBoneRight.m_Inert = 1f;
            earBoneRight.UpdateParameters();
            
            inert = true;
        }
        else if (inert && !player.Movement.onPlatform)
        {
            tailBone.m_Inert = 0f;
            tailBone.UpdateParameters();
            earBoneLeft.m_Inert = 0f;
            earBoneLeft.UpdateParameters();
            earBoneRight.m_Inert = 0f;
            earBoneRight.UpdateParameters();

            inert = false;
        }
    }
}
