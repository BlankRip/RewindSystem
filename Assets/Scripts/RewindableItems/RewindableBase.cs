using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blank.RewindSystem;

namespace Blank.GamePlay
{
    public class RewindableBase : MonoBehaviour
    {
        [SerializeField] protected int recordingFrameGap = 5;
        protected int frameCounter = 1;
        protected int lerpOverFrames;

        protected void RecordData()
        {
            if(frameCounter == recordingFrameGap)
            {
                AddDataToRecordList();
                frameCounter = 1;
            }
            else
            {
                frameCounter++;
            }
        }

        protected virtual void AddDataToRecordList() { }

        protected virtual void StartRewind() { }

        protected virtual void Rewind() { }

        protected virtual void EndRewind() { }

        protected void SubRewindEvents()
        {
            RewindHandler.RecoredEvent += RecordData;
            RewindHandler.StartRewindEvent += StartRewind;
            RewindHandler.RewindEvent += Rewind;
            RewindHandler.EndRewindEvent += EndRewind;
        }

        protected void UnSubRewindEvents()
        {
            RewindHandler.RecoredEvent -= RecordData;
            RewindHandler.StartRewindEvent -= StartRewind;
            RewindHandler.RewindEvent -= Rewind;
            RewindHandler.EndRewindEvent -= EndRewind;
        }
    }
}