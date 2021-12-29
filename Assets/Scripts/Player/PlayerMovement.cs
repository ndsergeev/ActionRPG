using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;

    [Header("STATES")]
    public bool isMoving;
    public bool isGrounded;
    public bool isJumping;
    public bool isFlapping;
    public bool isFalling;
    public bool isDashing;
    public bool isSliding;

    // Inputs
    Vector2 movementInput;
    bool jumpInput;

    // Stops all movement if true
    public bool isFrozen = false;

    // GROUNDING
    float footSnapDist = 0.01f;
    RaycastHit groundingHit;
    
    [Header("MOVEMENT")]
    // Horizontal movement
    [SerializeField]
    float moveSpeed = 3f;
    Vector3 moveDirection;

    // Rotation
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    float obstacleRaycastHeight = 0.05f;
    float obstacleRaycastDistance = 0.4f;

    bool isRotatingTowardDirection;
    float rotateTowardOriginalY;
    float rotateTowardTargetY;
    float rotateTowardCurrTime;
    float rotateTowardTotalTime;

    bool isRotatingToTarget;

    [SerializeField] float rotateToTargetSpeed;

    [Header("SLOPES")]
    // Slopes
    [SerializeField] float maxSlopeAngle = 35f;
    Vector3 slopeDir;
    float slopeAngle;
    float slopeCheckLength = 0.3f;
    Vector3 slopeNormal;

    [Header("SLIDING")]
    // Sliding
    [SerializeField] float slideSpeed = 5f;
    [SerializeField] float slideTurnSpeed = 5f;
    float currSlideTurnAngle;
    [SerializeField] float slideRotateSpeed;

    [Header("JUMPING")]
    // Jumping / Falling
    [SerializeField]
    float jumpHeight = 3f;
    Vector3 velocityBeforeJump;
    [SerializeField] float loseVelocityInAirSpeed = 1f;
    float fallMultiplier = 2.5f;
    float lowJumpModifier = 2f;
    public bool doubleJumpEnabled;
    bool canDoubleJump;
    bool unlimitedJumpsEnabled;
    [SerializeField] float wingBoost = 5f;

    [Header("DASHING")]
    // Dashing
    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float dashLength = 3f;
    Vector3 dashDir;
    Vector3 dashStartPos;
    Vector3 velocityBeforeDash;
    bool canDash = true;
    public bool isDashUnlocked;
    Collider dashStartGround;

    [Header("MOVING PLATFORMS")]
    // Platform movement
    public bool onPlatform;

    private void OnEnable()
    {
        PlayerEvents.JumpEvent += Jump;
        PlayerEvents.DashEvent += Dash;

        PlayerEvents.StartDialogueEvent += Freeze;
        PlayerEvents.EndDialogueEvent += Unfreeze;
    }

    private void OnDisable()
    {
        PlayerEvents.JumpEvent -= Jump;
        PlayerEvents.DashEvent -= Dash;

        PlayerEvents.StartDialogueEvent -= Freeze;
        PlayerEvents.EndDialogueEvent -= Unfreeze;
    }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Don't do anything if frozen
        if (isFrozen) { return; }

        GetMovementInputs();

        HandleGrounding();
        HandleMovement();
        CheckForObstacles();
        HandleRotation();
        HandleSlopes();
        HandleSliding();
        HandleJumping();
        HandleDashing();
    }

    private void Update()
    {
        HandleRotatingTowardDirection();
    }

    void GetMovementInputs()
    {
            // Get movement input
            movementInput = InputManager.Singleton.GetMoveInput();

            // Get jump input
            jumpInput = InputManager.Singleton.GetJumpInput();
    }

    void HandleGrounding()
    {
        // Don't do anything if starting jump
        // Prevents player from being stuck on ground when trying to jump
        if (isJumping) return;

        // I set layer 8 as the 'player' layer in unity
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask; // Everything but player layer

        // Calculate position of bottom of collider to use as origin of raycast
        Vector3 botOfColl = transform.position + player.capsColl.center + Vector3.down * player.capsColl.height / 2;

        // Calculate how far to shoot ray
        float rayDistance = botOfColl.y - transform.position.y + footSnapDist;

        // RaycastHit to store hit info
        RaycastHit hit;
        RaycastHit sinkPreventHit;
        Vector3 groundHitPos = new Vector3(0,0,0);
        // Does the ray intersect any objects excluding the player layer
        // Checks if the player is on the ground

        if (Physics.SphereCast(botOfColl, player.capsColl.radius,
            Vector3.down, out hit, rayDistance, layerMask))
        {
            //Debug.DrawRay(botOfColl, Vector3.down * rayDistance, Color.red);

            if (!isGrounded && dashStartGround != null && hit.collider != dashStartGround)
            {
                // Stop dashing if dashing
                print("ground stop dashing");
                if (isDashing) isDashing = false;
            } else if (!isGrounded)
            {
                //if (isDashing) isDashing = false;
            }
            
            // Player is on the ground
            isGrounded = true;
            isFalling = false;

            // Enable dash if dashed while in air
            if (!canDash) canDash = true;

            // Store RaycastHit for ground
            groundingHit = hit;
            groundHitPos = hit.point;

        }
        else // In air
        {
            //Debug.DrawRay(botOfColl, Vector3.down * rayDistance, Color.blue);
            //Debug.DrawRay(transform.position + player.capsColl.center + Vector3.down * player.capsColl.height / 2, Vector3.down * rayDistance, Color.green);

            // If stepped off platform or ledge and is now falling
            if (isGrounded)
            {
                // Keep some velocity of the movement from before falling
                velocityBeforeJump = player.rb.velocity / 2;
            }

            // Update states
            isGrounded = false;
            isFalling = true;

            // Activate gravity for player
            player.rb.useGravity = true;

            // (FOR BETTER JUMPING FEEL)
            // If falling
            if (player.rb.velocity.y < 0)
            {
                // Make player fall faster
                player.rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }

            /*
            // If wings have flapped
            if (player.animations.GetFlapStatus())
            {
                // Return wings to original position
                player.animations.Flapped();
            }
            */
            // Release Plaftorm
            if (transform.parent != null)
            {
                transform.parent = null;

            }

            if (onPlatform)
            {
                onPlatform = false;

                transform.parent = null;

                transform.localScale = Vector3.one;

            }

            // Stop from sinking into ground
            Vector3 sinkCheckpos = botOfColl + new Vector3(0, 0.2f, 0);
            Debug.DrawRay(sinkCheckpos, Vector3.down * rayDistance * 0.9f, Color.white);
            if (Physics.Raycast(sinkCheckpos, Vector3.down, out sinkPreventHit, rayDistance, layerMask))
            {
                Debug.DrawRay(sinkCheckpos, Vector3.down * rayDistance, Color.blue);

                print("Prevented sink!");
                groundingHit = sinkPreventHit;
                groundHitPos = sinkPreventHit.point;

                if (!isGrounded)
                {
                    // Stop dashing if dashing
                    print("sink prevent stop dashing");
                    if (isDashing && sinkPreventHit.collider != dashStartGround) isDashing = false;
                }

                // Player is on the ground
                isGrounded = true;
                isFalling = false;

                // Enable dash if dashed while in air
                if (!canDash) canDash = true;
            }

            // Stop sliding if sliding
            if (isSliding) isSliding = false;
        }

        // If player is now on ground
        if (isGrounded)
        {
            // Stop from sinking into ground
            Vector3 sinkCheckpos = botOfColl + new Vector3(0, 0.2f, 0);
            Debug.DrawRay(sinkCheckpos, Vector3.down * rayDistance * 0.9f, Color.white);
            if (Physics.Raycast(sinkCheckpos, Vector3.down, out sinkPreventHit, rayDistance, layerMask))
            {
                Debug.DrawRay(sinkCheckpos, Vector3.down * rayDistance, Color.blue);

                print("Prevented sink!");
                groundingHit = sinkPreventHit;
                groundHitPos = sinkPreventHit.point;
            }

            // Turn off gravity
            player.rb.useGravity = false;

            // Set target y position to position the raycast hit
            float yPosTarget = groundHitPos.y;
            
            // Create a target position based off the players current position and the 'y target position'
            Vector3 targetPosition = transform.position;
            targetPosition.y = yPosTarget;

            // Set player's position to target position
            transform.position = targetPosition;

            // Reset velocity before jump, since player has landed
            velocityBeforeJump = Vector3.zero;

            // Remove any 'y' velocity from player's velocity
            player.rb.velocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);

            // Is the hit object a moving platform?


            if (hit.collider != null)
            {
                bool hitMovingPlatform = (hit.collider.gameObject.layer == LayerMask.NameToLayer("Moving Platform"));


                // If landed on platform
                if (hitMovingPlatform && !onPlatform)
                {
                    // Set player's parent object to the platform that they landed on
                    transform.parent = hit.collider.transform.parent;


                    onPlatform = true;
                }
                else if (!hitMovingPlatform && onPlatform)
                {
                    onPlatform = false;
                    transform.parent = null;
                }
            }
            
        }
    }

    void HandleMovement()
    {
        //if (isDashing) return;

        // Create normalized direction vector out of movement inputs
        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        // Move Character if getting any move inputs
        if (direction.magnitude > 0f)
        {
            // Get the angle that the input direction needs to be rotated so that direction is based off camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                + player.Cameras.mainCam.transform.eulerAngles.y;

            // Base rotation off player when targeting something
            if (player.Targeting.isTargeting)
            {
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                + player.transform.eulerAngles.y;
            }

            //Set the move direction based on the camera
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Normalize move direction
            Vector3 movDir = moveDirection.normalized;

            // Get movement speed
            float speed = moveSpeed * Time.deltaTime * 100;

            // If in the air
            if (!isGrounded)
            {
                // Reduce speed
                //speed *= 0.5f;

                if (movementInput != Vector2.zero)
                {
                    if (Vector3.Angle(movDir, velocityBeforeJump) < 45)
                    {
                        // Reduce air velocity
                        loseVelocityInAirSpeed = 1f;

                        // Reduce speed
                        speed *= 0.4f;
                    }
                    else if (Vector3.Angle(movDir, velocityBeforeJump) >= 45
                        && Vector3.Angle(movDir, velocityBeforeJump) < 90)
                    {
                        // Reduce air velocity
                        loseVelocityInAirSpeed = 2f;

                        // Reduce speed
                        speed *= 0.5f;
                    }
                    else if (Vector3.Angle(movDir, velocityBeforeJump) >= 90
                        && Vector3.Angle(movDir, velocityBeforeJump) < 135)
                    {
                        // Reduce air velocity
                        loseVelocityInAirSpeed = 3f;

                        // Reduce speed
                        speed *= 0.6f;
                    }
                    else if (Vector3.Angle(movDir, velocityBeforeJump) >= 135)
                    {
                        // Reduce air velocity
                        loseVelocityInAirSpeed = 4f;

                        // Reduce speed
                        speed *= 0.7f;
                    }
                }
            }

            // Calculate players new velocity
            Vector3 newVelocity = new Vector3(moveDirection.normalized.x * speed + velocityBeforeJump.x,
                player.rb.velocity.y, moveDirection.normalized.z * speed + velocityBeforeJump.z);

            // Set players new velocity
            player.rb.velocity = newVelocity;

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask; // Everything but player layer

            // RaycastHit to store hit info
            RaycastHit hit;
            // Stops player from getting too close to walls (Player will climb them if too close)

            // If there is something blocking player from moving in move direction
            if (Physics.Raycast(transform.position + Vector3.up / 2, moveDirection, player.capsColl.radius + 0.2f, layerMask))
            {
                player.rb.velocity -= Vector3.Project(player.rb.velocity, moveDirection);
                HitObstacle();
            }
            
            // If targeting
            if (player.Targeting.isTargeting)
            {
                Vector3 playerPos = transform.position;
                Vector3 targetPos = player.Targeting.target.transform.position;
                targetPos.y = 0;
                Vector3 dirFromPlayerToTarget = (targetPos - playerPos).normalized;
                float angleBetweenMoveDirAndDirToTarget = Vector3.Angle(moveDirection, dirFromPlayerToTarget);
                float distToTarget = Vector3.Distance(playerPos, targetPos);


                // If player is close to target
                if (distToTarget <= 0.5f)
                {
                    // Stop player from moving toward target

                    
                    if (angleBetweenMoveDirAndDirToTarget < 89)
                    {
                        player.rb.velocity -= Vector3.Project(player.rb.velocity, moveDirection);
                    }

                }
            }

            // Update 'isMoving' state based off current velocity
            if (player.rb.velocity != Vector3.zero)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
        // If there is no movement input and player is on the ground
        else if (isGrounded)
        {
            // Remove all velocity from player
            player.rb.velocity = Vector3.zero;

            // Reset moveDirection
            moveDirection = Vector3.zero;

            // Update isMoving state
            isMoving = false;
        }
        // If there is no movement input and player is in the air
        else if (!isGrounded)
        {
            // Reset moveDirection
            moveDirection = Vector3.zero;

            // Calculate player's velocity based on the velocity before jumping and the current 'y' velocity (from gravity or jumping)
            Vector3 newVelocity = new Vector3(velocityBeforeJump.x,
            player.rb.velocity.y, velocityBeforeJump.z);

            // Set the player's velocity
            player.rb.velocity = newVelocity;
        }
    }

    void CheckForObstacles()
    {
        // I set layer 8 as the 'player' layer in unity
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask; // Everything but player layer

        Vector3 rayOrigin = transform.position + new Vector3(0, obstacleRaycastHeight, 0);
        Vector3 rayDirection = transform.forward;
        rayDirection = rayDirection.normalized;
        // RaycastHit to store hit info
        RaycastHit hit;

        Debug.DrawRay(rayOrigin, rayDirection * obstacleRaycastDistance, Color.white);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, obstacleRaycastDistance, layerMask))
        {
            

            Debug.DrawRay(rayOrigin, rayDirection * obstacleRaycastDistance, Color.yellow);

            if (velocityBeforeJump != Vector3.zero)
            {
                velocityBeforeJump = Vector3.zero;
            }

            float obstacleSlopeAngle = Vector3.Angle(hit.normal, Vector3.up);


            if (obstacleSlopeAngle > maxSlopeAngle)
            {
                //player.rb.velocity -= Vector3.Project(player.rb.velocity, moveDirection);

                if (isDashing) isDashing = false;
            }
            else if (isDashing && !isJumping && isGrounded)
            {
                Vector3 left = Vector3.Cross(hit.normal, Vector3.up);
                Vector3 obstacleSlopeUpDir = Vector3.Cross(hit.normal, left);
                obstacleSlopeUpDir *= -1;
                Debug.DrawRay(rayOrigin, obstacleSlopeUpDir, Color.red);

                Vector3 temp = dashDir.normalized + obstacleSlopeUpDir;
                dashDir = new Vector3(dashDir.x, obstacleSlopeUpDir.y + 0.1f, dashDir.z).normalized;


                Debug.DrawRay(rayOrigin, dashDir, Color.green);
            }
            else if (isDashing)
            {
                dashDir = new Vector3(dashDir.x, 0, dashDir.z);
            }

        }
    }

    void HandleSlopes()
    {
        if (!isGrounded) return;

        slopeAngle = Vector3.Angle(Vector3.up, groundingHit.normal);

        //print(slopeAngle);

        if (slopeAngle > maxSlopeAngle)

        {
            if (CheckIfSlopeIsLongEnoughForSliding(groundingHit))
            {
                print("slope: " + slopeAngle);

                isSliding = true;

                player.rb.useGravity = false;
                isGrounded = true;

                slopeAngle = Vector3.Angle(groundingHit.normal, Vector3.up);

                slopeNormal = groundingHit.normal;
            }


            if (isDashing && slopeAngle != 90) isDashing = false;
        }
        else
        {
            isSliding = false;
        }
    }

    void HandleSliding()
    {
        if (isSliding)
        {
            // Get slope direction vector
            Vector3 left = Vector3.Cross(slopeNormal, Vector3.up);
            Vector3 slopeDir = Vector3.Cross(slopeNormal, left);

            // Get direction to face while sliding
            Vector3 directionToFaceWhileSliding = new Vector3(slopeNormal.x, 0, slopeNormal.z);
            // Get rotation needed to rotate from current direction to slide facing direction
            Quaternion newRot = Quaternion.FromToRotation(transform.forward, directionToFaceWhileSliding);
            // Rotate player to face in slide direction
            transform.rotation *= newRot;

            // Get velocity to slide down slope
            Vector3 slideVelocity = slopeDir.normalized * slideSpeed;


            // Get move direction
            Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

            // Get the angle that the input direction needs to be rotated so that direction is based off camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                + player.Cameras.mainCam.transform.eulerAngles.y;

            // Get move direction based on camera
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Turning axis, initially points to the left of player
            Vector3 moveAxis = Vector3.Cross(directionToFaceWhileSliding, Vector3.up);

            //Debug.DrawRay(transform.position, moveAxis.normalized * 2, Color.blue);

            // Do turning if getting movement inputs
            if (direction != Vector3.zero)
            {
                // Figure out which way to turn
                float angleFromSlideDirToLeftDir = Vector3.Angle(moveAxis, moveDirection);
                float angleFromSlideDirToRightDir = Vector3.Angle(-moveAxis, moveDirection);


                // If moving to the right
                if (angleFromSlideDirToLeftDir > angleFromSlideDirToRightDir)
                {
                    if (angleFromSlideDirToRightDir <= 75)
                    {
                        // Add velocity to the right
                        slideVelocity += -moveAxis * slideTurnSpeed;

                        if (currSlideTurnAngle <= 30)
                        {
                            // Rotate to the right
                            player.Animations.rootBone.RotateAround(player.Animations.rootBone.position,
                                slopeNormal, slideRotateSpeed * Time.deltaTime);

                            currSlideTurnAngle += slideRotateSpeed * Time.deltaTime;
                        }

                    }

                }
                // Else moving to the left
                else
                {
                    if (angleFromSlideDirToLeftDir <= 75)
                    {
                        // Add velocity to the left
                        slideVelocity += moveAxis * slideTurnSpeed;

                        if (currSlideTurnAngle >= -30)
                        {
                            // Rotate to the right
                            player.Animations.rootBone.RotateAround(player.Animations.rootBone.position,
                                slopeNormal, -slideRotateSpeed * Time.deltaTime);

                            currSlideTurnAngle -= slideRotateSpeed * Time.deltaTime;
                        }

                    }


                }
            }
            // If no turning input and already turned
            else if (currSlideTurnAngle != 0)
            {
                // If turned toward right side
                if (currSlideTurnAngle > 0)
                {
                    // Turn back toward centre
                    player.Animations.rootBone.RotateAround(player.Animations.rootBone.position,
                                slopeNormal, -slideRotateSpeed / 2 * Time.deltaTime);
                    currSlideTurnAngle -= slideRotateSpeed / 2 * Time.deltaTime;

                    slideVelocity += -moveAxis * slideTurnSpeed * currSlideTurnAngle / 35;
                }
                // If turned toward left side
                else if (currSlideTurnAngle < 0)
                {
                    // Turn back toward centre
                    player.Animations.rootBone.RotateAround(player.Animations.rootBone.position,
                                slopeNormal, slideRotateSpeed / 2 * Time.deltaTime);
                    currSlideTurnAngle += slideRotateSpeed / 2 * Time.deltaTime;


                    slideVelocity += moveAxis * slideTurnSpeed * currSlideTurnAngle / -35;
                }

                // If pretty much centred again
                if (currSlideTurnAngle > -0.00001 && currSlideTurnAngle < 0.00001)
                {
                    // Centre slide rotation
                    player.Animations.rootBone.localEulerAngles = Vector3.zero;

                    currSlideTurnAngle = 0;
                }
            }



            // Set player velocity to sliding velocity
            player.rb.velocity = slideVelocity;



        }
        else if (currSlideTurnAngle != 0)
        {
            player.Animations.rootBone.localEulerAngles = Vector3.zero;

            currSlideTurnAngle = 0;
        }
    }

    private bool CheckIfSlopeIsLongEnoughForSliding(RaycastHit hit)
    {
        bool isLongEnough = false;

        Vector3 left = Vector3.Cross(hit.normal, Vector3.up);
        slopeDir = Vector3.Cross(hit.normal, left);

        if (slopeDir.y >= 0)
        {
            slopeDir = new Vector3(slopeDir.x, -slopeDir.y, slopeDir.z);
        }

        Vector3 slopeCheckStartPoint = hit.point + hit.normal.normalized * 0.1f;

        Vector3 slopeCheckingDir = slopeDir.normalized * slopeCheckLength;

        Vector3 slopeCheckEndPoint = slopeCheckStartPoint + slopeCheckingDir;

        int layerMask = 1 << 8;

        layerMask = ~layerMask;

        RaycastHit slopeCheckHit;

        if (Physics.Raycast(slopeCheckEndPoint, -hit.normal, out slopeCheckHit, 0.15f, layerMask))
        {

            if (slopeCheckHit.normal == hit.normal)
            {
                isLongEnough = true;
                print("Slope long enough");
            }

        }

        Debug.DrawRay(slopeCheckStartPoint, slopeCheckEndPoint - slopeCheckStartPoint, Color.red);
        Debug.DrawRay(slopeCheckEndPoint, -hit.normal, Color.blue);

        return isLongEnough;
    }

    void HandleRotation()
    {
        if (player.Targeting.isTargeting)
        {
            if (isRotatingToTarget)
            {
                HandleRotatingToTarget();
                return;
            }
            else
            {
                transform.LookAt(player.Targeting.target.transform);
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                return;
            }
        }

        // Create normalized direction vector out of movement inputs
        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if (direction.magnitude > 0f)
        {
            // Get the angle that the input direction needs to be rotated so that direction is based off camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
            + player.Cameras.mainCam.transform.eulerAngles.y;

            // Get smoothed out angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref turnSmoothVelocity, turnSmoothTime);

            // Rotate the player by the smoothed out angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void HandleJumping()
    {
        // If jumping
        if (isJumping)
        {
            // (FOR BETTER JUMPING FEEL)
            // If player is rising upwards AND player isn't holding jump button
            if (player.rb.velocity.y > 0 && !InputManager.Singleton.GetJumpInput())
            {
                // Make player not rise as much / fall faster
                player.rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpModifier - 1) * Time.deltaTime;
            }

            // If player is falling
            if (player.rb.velocity.y < 0)
            {
                // Update movement states
                isJumping = false;
                isFalling = true;
                isFlapping = false;
            }
        }

        // If player is in the air and still has horizontal velocity from before being in the air
        if (!isGrounded && velocityBeforeJump != Vector3.zero)
        {
            // Lessen the amount of horizontal velocity
            velocityBeforeJump.x = Mathf.Lerp(velocityBeforeJump.x, 0, loseVelocityInAirSpeed * Time.deltaTime);
            velocityBeforeJump.z = Mathf.Lerp(velocityBeforeJump.z, 0, loseVelocityInAirSpeed * Time.deltaTime);

            // If horizontal velocity is close to 0, set it to 0
            if (velocityBeforeJump.x < 0.1 && velocityBeforeJump.x > -0.1)
                velocityBeforeJump.x = 0;
            if (velocityBeforeJump.z < 0.1 && velocityBeforeJump.z > -0.1)
                velocityBeforeJump.z = 0;
        }
    }

    public void Jump()
    {
        // Don't do anything if frozen
        if (isFrozen) return;

        // If player is on the ground OR in the air and can double jump
        if (isGrounded || canDoubleJump)
        {
            // If player is in the air AND can double jump
            if (!isGrounded && canDoubleJump)
            {
                // Stop player from falling
                player.rb.velocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
                isFalling = false;

                // If player can't jump unlimited times
                if (!unlimitedJumpsEnabled)
                {
                    // Prevent from jumping again after double jump
                    canDoubleJump = false;
                }

                // Play 'Wing Flap' animation
                isFlapping = true;

                // Give horizontal boost in movement direction
                player.rb.velocity = new Vector3(player.rb.velocity.x + moveDirection.normalized.x * wingBoost,
                    player.rb.velocity.y, player.rb.velocity.z + moveDirection.z * wingBoost);
            }
            else
            {
                // If double jump is enabled (Player has wings)
                if (doubleJumpEnabled)
                {
                    // Allow player to double jump (In certain conditions)
                    canDoubleJump = true;
                }
            }

            // Update states
            isJumping = true;
            isGrounded = false;

            // Reset loseVelocityInAirSpeed
            loseVelocityInAirSpeed = 1f;

            // Store velocity before jump to player's velocity at time of starting jump
            velocityBeforeJump = player.rb.velocity;

            // Turn player's gravity on
            player.rb.useGravity = true;

            // Create jump vector
            Vector3 jumpVector = new Vector3(0, jumpHeight, 0);


            // Add the jump vector as a force to the player
            // Add the player's current velocity as well to help player jump in direction they are moving
            
            player.rb.AddForce(player.rb.velocity + jumpVector * 100);
        }

        // If player is on a moving platform (Has the platform as their parent object)
        if (transform.parent != null)
        {
            // Remove platform from being player's parent object
            transform.parent = null;
        }

        if (onPlatform)
        {
            onPlatform = false;

            transform.localScale = Vector3.one;
        }
    }

    void EnableDoubleJump()
    {
        doubleJumpEnabled = true;
    }

    public void ToggleUnlimitedJumps()
    {
        unlimitedJumpsEnabled = !unlimitedJumpsEnabled;
    }

    void HandleDashing()
    {
        if (isDashing)
        {
            // Fix Dash direction if not on ground
            if (!isGrounded)
            {
                dashDir = new Vector3(dashDir.x, 0, dashDir.z);
            }

            // Change player velocity based on dash direction and dash speed (APPLY THE DASH)
            player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0) + (dashDir * dashSpeed);

            // Align dash with slope
            if (isGrounded)
            {
                //player.rb.velocity = new Vector3()
            }

            // If player has dashed further than dashLength
            if (Vector3.Distance(dashStartPos, transform.position) >= dashLength)
            {
                // Set player's velocity to 0 if player is on ground
                if (isGrounded)
                {
                    player.rb.velocity = Vector3.zero;
                }
                // Store velocity from the dash to use when falling
                else 
                {
                    velocityBeforeJump = player.rb.velocity;
                }
                
                // Update state
                isDashing = false;
            }


            // If player moves away from dash direction
            if (Vector3.Angle(moveDirection, dashDir) > 100)
            {
                
                print("Moved away");
                isDashing = false;
            }
        }
    }

    void Dash()
    {
        // Don't do anything if dash locked
        if (!isDashUnlocked) return;

        // Don't do anything if frozen
        if (isFrozen) return;

        // Don't dash if can't dash
        if (!canDash) return;

        // Don't dash if not moving
        if (moveDirection == Vector3.zero) return;

        // If dashing while in air, stop from dashing again
        if (!isGrounded) canDash = false;

        // Store dash start position
        dashStartPos = transform.position;

        // Update state
        isDashing = true;

        // Store current move direction as dash direction
        dashDir = moveDirection.normalized;


        // If not targeting
        if (!player.Targeting.isTargeting)
        {
            // Rotate player toward dash direction
            Vector3 targetPos = transform.position + moveDirection;
            transform.LookAt(targetPos);
        }
        
        if (isGrounded)
        {
            dashStartGround = groundingHit.collider;
        }
        else
        {
            dashStartGround = null;
        }
    }

    void HitObstacle()
    {
        if (isDashing)
        {
            print("hit obstacle stop dash");
            isDashing = false;
        }
    }

    public void RotateToward(float targetYEulerAngle, float timeInSeconds)
    {
        // Store info in references
        rotateTowardOriginalY = transform.localEulerAngles.y;
        rotateTowardTargetY = targetYEulerAngle;
        rotateTowardCurrTime = 0f;
        rotateTowardTotalTime = timeInSeconds;

        isRotatingTowardDirection = true;
    }

    void HandleRotatingTowardDirection()
    {
        // Dont do anything if not rotating toward a direction
        if (!isRotatingTowardDirection) return;

        // Calculate time since started rotating toward direction
        rotateTowardCurrTime += Time.deltaTime;

        // Calculate current percentage through rotation
        float percentageThroughRotation = rotateTowardCurrTime / rotateTowardTotalTime;

        // Calculate difference between original and target rotation direction
        float differenceBetweenRotations = Mathf.Abs((rotateTowardOriginalY - rotateTowardTargetY));

        // Calculate percentage of the difference, to add the original rotation
        float newRotationY = rotateTowardOriginalY
            + (differenceBetweenRotations * percentageThroughRotation);

        // Create new rotation euler
        Vector3 newRotationEuler = new Vector3(
            transform.localEulerAngles.x,
            newRotationY,
            transform.localEulerAngles.z);

        // Set players rotation to new rotation
        transform.localEulerAngles = newRotationEuler;

        // If rotation should be complete
        if (rotateTowardCurrTime >= rotateTowardTotalTime)
        {
            isRotatingTowardDirection = false;

            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                rotateTowardTargetY,
                transform.localEulerAngles.z);
        }
    }

    public void StartRotatingToTarget()
    {
        isRotatingToTarget = true;
    }

    void HandleRotatingToTarget()
    {
        if (!isRotatingToTarget) return;

        Vector3 targetPos = player.Targeting.target.transform.position;
        targetPos.y = transform.position.y;

        Vector3 dirToTarget = targetPos - transform.position;

        float singleStep = rotateToTargetSpeed * Time.deltaTime;

        Quaternion qTo = Quaternion.LookRotation(dirToTarget);

        //Vector3 newDirection = Mathf.Lerp(transform.forward, dirToTarget, singleStep);

        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, singleStep);

        if (Vector3.Angle(transform.forward, dirToTarget) <= 1)
        {
            isRotatingToTarget = false;
        }
    }

    public void Unfreeze()
    {
        isFrozen = false;
    }

    public void Freeze()
    {
        isFrozen = true;

        player.rb.useGravity = false;
        player.rb.velocity = Vector3.zero;

        isMoving = false;
        isFalling = false;
        isJumping = false;
        isDashing = false;
    }
}
