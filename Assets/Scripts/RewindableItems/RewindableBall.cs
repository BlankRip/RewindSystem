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
        private RecordInstance currentRecordInstance;
        private RecordInstance targetRecordInstance;

        private void Start()
        {
            Vector3 dir = Random.onUnitSphere;
            rb = GetComponent<Rigidbody>();
            rb.velocity = dir.normalized * initalForce;
            //rb.AddForce((dir.normalized * initalForce), ForceMode.Impulse);

            recordList = new List<RecordInstance>();
            recordList.Capacity = RewindHandler.maxRecordArrayLength;
            frameCounter = 1;
            AddDataToRecordList();

            SubRewindEvents();
        }
        
        private void OnDestroy()
        {
            UnSubRewindEvents();
        }

        protected override void AddDataToRecordList()
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
            rb.useGravity = false;
            lerpOverFrames = frameCounter;
            frameCounter = 1;
            currentRecordInstance = new RecordInstance(transform.position, transform.rotation, rb.velocity);
            targetRecordInstance = recordList[0];
            rewinding = true;
        }

        bool rewinding;
        private void Update() {
            if(rewinding)
            {
                float lerpValue = frameCounter/lerpOverFrames;
                transform.position = Vector3.Lerp(transform.position, targetRecordInstance.position, 10 * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRecordInstance.rotation, 10 * Time.deltaTime);
            }
        }
        
        protected override void Rewind()
        {
            // float lerpValue = frameCounter/lerpOverFrames;
            // transform.position = Vector3.Lerp(currentRecordInstance.position, targetRecordInstance.position, lerpValue);
            // transform.rotation = Quaternion.Slerp(currentRecordInstance.rotation, targetRecordInstance.rotation, lerpValue);
            if(frameCounter == lerpOverFrames)
            {
                frameCounter = 1;
                lerpOverFrames = recordingFrameGap;
                if(recordList.Count > 1)
                    recordList.RemoveAt(0);
                currentRecordInstance = targetRecordInstance;
                targetRecordInstance = recordList[0];
            }
            else
            {
                frameCounter++;
            }
        }

        protected override void EndRewind()
        {
            frameCounter = recordingFrameGap - frameCounter;
            rb.useGravity = true;
            rb.velocity = currentRecordInstance.velocity;
            rewinding = false;
        }
    }
}