using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blank.RewindSystem;

namespace Blank.GamePlay
{
    public class RewindableBall : RewindableBase
    {
        private class RecordInstance
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 velocity;

            public RecordInstance(Vector3 position, Quaternion rotation, Vector3 velocity)
            {
                this.position = position;
                this.rotation = rotation;
                this.velocity = velocity;
            }
        }

        private Rigidbody rb;
        [SerializeField] float initalForce = 25;

        private List<RecordInstance> recordList;
        private RecordInstance currentReadingRecordInstance;

        private void Start()
        {
            Vector3 dir = Random.onUnitSphere;
            rb = GetComponent<Rigidbody>();
            rb.velocity = dir.normalized * initalForce;
            //rb.AddForce((dir.normalized * initalForce), ForceMode.Impulse);

            recordList = new List<RecordInstance>();
            recordList.Capacity = RewindHandler.maxRecordArrayLength;

            SetUpDistanceBasedSubscription();
        }
        
        private void OnDestroy()
        {
            if(rewindSubscribed)
                UnSubRewindEvents();
        }

        private void Update()
        {
            UpdateDistanceBasedSubscription();
        }

        protected override void OnRewinEventSubscribe()
        {
            RecordData();
        }

        protected override void OnRewinEventUnSubscribe()
        {
            recordList.Clear();
        }

        protected override void RecordData()
        {
            if(recordList.Count < RewindHandler.maxRecordArrayLength)
            {
                recordList.Insert(0, new RecordInstance(transform.position, transform.rotation, rb.velocity));
            }
            else
            {
                recordList.RemoveAt(recordList.Count - 1);
                recordList.Insert(0, new RecordInstance(transform.position, transform.rotation, rb.velocity));
            }
        }

        protected override void StartRewind()
        {
            rb.velocity = Vector3.zero;
        }

        protected override void Rewind()
        {
            currentReadingRecordInstance = recordList[0];
            transform.position = currentReadingRecordInstance.position;
            transform.rotation = currentReadingRecordInstance.rotation;
            if(recordList.Count > 1)
                recordList.RemoveAt(0);
        }

        protected override void EndRewind()
        {
            rb.velocity = currentReadingRecordInstance.velocity;
        }
    }
}