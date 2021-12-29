using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameras : MonoBehaviour
{
    [HideInInspector]
    public Camera mainCam;
    [SerializeField] Transform camLookAtPlayerTarget;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetLookAtPlayerTarget()
    {
        return camLookAtPlayerTarget;
    }
}
