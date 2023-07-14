using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blank
{
    public class Input : MonoBehaviour
    {
        private static PlayerInput playerInput;
        public static PlayerInput GetPlayerInput()
        {
            if(playerInput == null)
            {
                Debug.LogError("Player Input Does not exist");
                return null;
            }
            return playerInput;
        }

        private void Awake() {
            playerInput = GetComponent<PlayerInput>();
        }
    }
}
