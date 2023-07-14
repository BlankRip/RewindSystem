using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blank.RewindSystem
{
    public class RewindHandler : MonoBehaviour
    {
        public static Action StartRewindEvent;
        public static Action RewindEvent;
        public static Action EndRewindEvent;
        public static Action RecoredEvent;
        public static int maxRecordArrayLength;
        public static float maxRewindTime;
        public static Transform rewindCenterPoint;

        private bool isRewinding;
        private InputAction rewindAction;
        private float rewindTimer;
        [SerializeField] float rewindTimeLimit = 5f;
        [SerializeField] Transform rewindCenter;  // Remove after player is added

        private void Awake()
        {
            maxRecordArrayLength = Mathf.RoundToInt((rewindTimeLimit + 1)/Time.fixedDeltaTime);
            maxRewindTime = rewindTimeLimit;
            rewindCenterPoint = rewindCenter;
        }

        private void Start()
        {
            rewindAction = Input.GetPlayerInput().actions["Rewind"];
            rewindTimer = 0.0f;
        }

        private void Update()
        {
            if(isRewinding)
            {
                rewindTimer += Time.deltaTime;
                if(rewindTimer > maxRewindTime)
                {
                    EndRewind();
                }
            }
            if(rewindAction.WasPressedThisFrame())
            {
                isRewinding = true;
                rewindTimer = 0.0f;
                StartRewindEvent?.Invoke();
            }
            else if(rewindAction.WasReleasedThisFrame())
            {
                EndRewind();
            }
        }

        private void EndRewind()
        {
            if(isRewinding)
            {
                isRewinding = false;
                EndRewindEvent?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            if(isRewinding)
                RewindEvent?.Invoke();
            else
                RecoredEvent?.Invoke();
        }

        public bool IsRewinding()
        {
            return isRewinding;
        }
    }
}