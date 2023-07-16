using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blank.GamePlay
{
    public class RewindablePlayer : MonoBehaviour
    {
        [SerializeField] float speed = 10;
        [SerializeField] float gravity = -18f;
        [SerializeField] float jumpHight = 4.0f;
        [SerializeField] Transform meshTransform;
        private CharacterController cc;
        private bool grounded, jump;
        private Vector3 gravityVelocity;

        private InputAction moveAction;
        private InputAction jumpAction;

        private void Start()
        {
            cc = GetComponent<CharacterController>();
            if(gravity > 0)
                gravity *= -1;
            
            PlayerInput playerInput = Input.GetPlayerInput();
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
        }

        private void Update()
        {
            grounded = cc.isGrounded;
            Vector2 moveData = moveAction.ReadValue<Vector2>();
            HandleMovement(moveData.x, moveData.y);
            jump = jumpAction.WasPressedThisFrame();
        }
        
        public void HandleMovement(float horizontalInput, float verticalInput)
        {
            if(grounded && gravityVelocity.y <= 0.0f)
                gravityVelocity.y = -2.0f;

            Vector3 moveDir =  (transform.right * horizontalInput) + (transform.forward * verticalInput);
            Vector3 moveVelocity = moveDir * speed * Time.deltaTime;
            cc.Move(moveVelocity);

            if(jump)
            {
                if(grounded)
                    gravityVelocity.y = Mathf.Sqrt(-2 * gravity * jumpHight);
            }
            gravityVelocity.y += gravity * Time.deltaTime;
            cc.Move(gravityVelocity * Time.deltaTime);

            meshTransform.forward = moveDir.normalized;
        }
    }
}