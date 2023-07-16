using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Blank.RewindSystem;

namespace Blank.GamePlay
{
    public class RewindablePlayer : RewindableBase
    {
        private class RecordInstance
        {
            public Vector3 position;
            public Quaternion meshRotation;
            public float gravityVelocity;

            public RecordInstance(Vector3 position, Quaternion meshRotation, float gravityVelocity)
            {
                this.position = position;
                this.meshRotation = meshRotation;
                this.gravityVelocity = gravityVelocity;
            }
        }

        [SerializeField] float speed = 10;
        [SerializeField] float gravity = -18f;
        [SerializeField] float jumpHight = 4.0f;
        [SerializeField] Transform meshTransform;
        private CharacterController cc;
        private bool grounded, jump;
        private Vector3 gravityVelocity;

        private InputAction moveAction;
        private InputAction jumpAction;

        private List<RecordInstance> recordList;
        private RecordInstance currentReadingRecordInstance;

        private void Start()
        {
            cc = GetComponent<CharacterController>();
            if(gravity > 0)
                gravity *= -1;
            
            PlayerInput playerInput = Input.GetPlayerInput();
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];

            recordList = new List<RecordInstance>();
            recordList.Capacity = RewindHandler.maxRecordArrayLength;
            RewindHandler.rewindCenterPoint = this.transform;
            SubRewindEvents();
        }

        private void OnDestroy()
        {
            UnSubRewindEvents();
        }

        private void Update()
        {
            if(RewindHandler.IsRewinding())
                return;
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

            if(moveDir.x != 0 || moveDir.z != 0)
                meshTransform.forward = moveDir.normalized;
        }

        protected override void RecordData()
        {
            if(recordList.Count < RewindHandler.maxRecordArrayLength)
            {
                recordList.Insert(0, new RecordInstance(transform.position, meshTransform.rotation, gravityVelocity.y));
            }
            else
            {
                recordList.RemoveAt(recordList.Count - 1);
                recordList.Insert(0, new RecordInstance(transform.position, meshTransform.rotation, gravityVelocity.y));
            }
        }

        protected override void StartRewind()
        {
            cc.enabled = false;
        }

        protected override void Rewind()
        {
            currentReadingRecordInstance = recordList[0];
            transform.position = currentReadingRecordInstance.position;
            meshTransform.rotation = currentReadingRecordInstance.meshRotation;
            if(recordList.Count > 1)
                recordList.RemoveAt(0);
        }

        protected override void EndRewind()
        {
            cc.enabled = true;
            gravityVelocity.y = currentReadingRecordInstance.gravityVelocity;
        }
    }
}