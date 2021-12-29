using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashTrail : MonoBehaviour
{
    [SerializeField] float maxTime = 0.15f;
    [SerializeField] float minTime = 0.05f;

    TrailRenderer trailRenderer;
    PlayerMovement movement;

    bool emitting;

    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
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
            trailRenderer.emitting = true;

            if (!emitting)
            {
                emitting = true;
            }

        }
        else
        {
            trailRenderer.emitting = false;
            emitting = false;
        }
    }
}
