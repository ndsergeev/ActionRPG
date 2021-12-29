using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDashTrail : MonoBehaviour
{
    ParticleSystem ps;
    PlayerMovement movement;

    bool isEmitting;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleEmitting();
    }

    void HandleEmitting()
    {
        if (movement.isDashing)
        {
            ps.Play();
            isEmitting = true;
        }
        else
        {
            ps.Stop();
        }
    }
}
