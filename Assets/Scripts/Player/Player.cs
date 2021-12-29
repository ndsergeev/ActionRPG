using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    # region VARIABLES
    
    // PLAYER COMPONENTS
    
    private PlayerMovement movement;
    public PlayerMovement Movement
    {
        get => movement;
        set => movement = value;
    }

    private PlayerAnimations animations;
    public PlayerAnimations Animations
    {
        get => animations;
        set => animations = value;
    }

    private PlayerCollision collision;
    public PlayerCollision Collision
    {
        get => collision;
        set => collision = value;
    }

    private PlayerAppearance appearance;
    public PlayerAppearance Appearance
    {
        get => appearance;
        set => appearance = value;
    }

    private PlayerTargeting targeting;
    public PlayerTargeting Targeting
    {
        get => targeting;
        set => targeting = value;
    }

    [SerializeField]
    private PlayerCameras cameras;
    public PlayerCameras Cameras
    {
        get => cameras;
        set => cameras = value;
    }

    // PHYSICS
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public CapsuleCollider capsColl;
    
    #endregion VARIABLES

    #region METHODS
    
    #region MONOBEHAVIOUR
    
    void Awake()
    {
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        // INPUTS
        InputManager.Singleton.EnableInputs();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    #endregion MONOBEHAVIOUR

    private void Initialize()
    {
        // PLAYER COMPONENTS
        movement = GetComponent<PlayerMovement>();
        animations = GetComponent<PlayerAnimations>();
        collision = GetComponent<PlayerCollision>();
        appearance = GetComponent<PlayerAppearance>();
        targeting = GetComponent<PlayerTargeting>();
        cameras = GetComponent<PlayerCameras>();

        // PHYSICS
        rb = GetComponent<Rigidbody>();
        capsColl = GetComponent<CapsuleCollider>();
    }
        
    #endregion METHODS
     
}
