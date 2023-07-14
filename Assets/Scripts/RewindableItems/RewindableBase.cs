using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blank.RewindSystem;

namespace Blank.GamePlay
{
    public class RewindableBase : MonoBehaviour, IRewindable
    {
        private void Start()
        {
            SubRewindEvents();
        }

        private void OnDestroy()
        {
            UnSubRewindEvents();
        }

        public virtual void RecordData() { }

        public virtual void StartRewind() { }

        public virtual void Rewind() { }

        public virtual void EndRewind() { }

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