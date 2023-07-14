using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blank.RewindSystem;

namespace Blank.GamePlay
{
    public class RewindableBase : MonoBehaviour
    {
        protected virtual void RecordData() { }

        protected virtual void StartRewind() { }

        protected virtual void Rewind() { }

        protected virtual void EndRewind() { }

        protected void SubRewindEvents()
        {
            Debug.Log("Subbed");
            RewindHandler.RecoredEvent += RecordData;
            RewindHandler.StartRewindEvent += StartRewind;
            RewindHandler.RewindEvent += Rewind;
            RewindHandler.EndRewindEvent += EndRewind;
        }

        protected void UnSubRewindEvents()
        {
            Debug.Log("UnSubbed");
            RewindHandler.RecoredEvent -= RecordData;
            RewindHandler.StartRewindEvent -= StartRewind;
            RewindHandler.RewindEvent -= Rewind;
            RewindHandler.EndRewindEvent -= EndRewind;
        }
    }
}