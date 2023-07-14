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
        public static int maxRecordArrayLength = 150;

        private bool isRewinding;
        private InputAction rewindAction;

        private void Start()
        {
            rewindAction = Input.GetPlayerInput().actions["Rewind"];
        }

        private void Update()
        {
            if(rewindAction.WasPressedThisFrame())
            {
                isRewinding = true;
                StartRewindEvent?.Invoke();
            }
            else if(rewindAction.WasReleasedThisFrame())
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