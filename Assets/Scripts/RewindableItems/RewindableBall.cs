using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blank.GamePlay
{
    public class RewindableBall : RewindableBase
    {
        private Rigidbody rb;
        [SerializeField] float initalForce = 25;

        private void Start()
        {
            Vector3 dir = Random.onUnitSphere;
            rb = GetComponent<Rigidbody>();

            rb.AddForce((dir.normalized * initalForce), ForceMode.Impulse);
        }
    }
}