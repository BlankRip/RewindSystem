using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blank.RewindSystem;

namespace Blank.GamePlay
{
    public class RewindableBase : MonoBehaviour
    {
        [SerializeField] float requiredDistanceToRewindCenter = 100;
        protected bool rewindSubscribed;
        private bool usingDistanceBasedSubscrition;
        private float distance;
        private Coroutine unsubCoroutine;
        private WaitForSeconds unsubWaitTime;

        protected void SetUpDistanceBasedSubscription()
        {
            requiredDistanceToRewindCenter = requiredDistanceToRewindCenter * requiredDistanceToRewindCenter;
            rewindSubscribed = false;
            usingDistanceBasedSubscrition = true;
            unsubCoroutine = null;
            unsubWaitTime = new WaitForSeconds(RewindHandler.maxRewindTime);
        }

        protected void UpdateDistanceBasedSubscription()
        {
            if(RewindHandler.IsRewinding())
                return;
            
            distance = (transform.position - RewindHandler.rewindCenterPoint.position).sqrMagnitude;
            if(distance < requiredDistanceToRewindCenter)
            {
                if(!rewindSubscribed)
                    SubRewindEvents();
                if(unsubCoroutine != null)
                {
                    StopCoroutine(unsubCoroutine);
                    unsubCoroutine = null;
                }
            }
            else if(rewindSubscribed && unsubCoroutine == null)
            {
                unsubCoroutine = StartCoroutine(UnSubRewindEventAfterAwhile());
            }
        }

        protected virtual void RecordData() { }

        protected virtual void StartRewind() { }

        protected virtual void Rewind() { }

        protected virtual void EndRewind() { }

        protected virtual void OnRewinEventSubscribe()
        {
            RecordData();
        }

        protected virtual void OnRewinEventUnSubscribe() { }

        protected void SubRewindEvents()
        {
            Debug.Log("Subbed");
            RewindHandler.RecoredEvent += RecordData;
            RewindHandler.StartRewindEvent += StartRewind;
            RewindHandler.RewindEvent += Rewind;
            RewindHandler.EndRewindEvent += EndRewind;

            rewindSubscribed = true;
            if(usingDistanceBasedSubscrition)
                OnRewinEventSubscribe();
        }

        private IEnumerator UnSubRewindEventAfterAwhile()
        {
            yield return unsubWaitTime;
            UnSubRewindEvents();
        }

        protected void UnSubRewindEvents()
        {
            Debug.Log("UnSubbed");
            RewindHandler.RecoredEvent -= RecordData;
            RewindHandler.StartRewindEvent -= StartRewind;
            RewindHandler.RewindEvent -= Rewind;
            RewindHandler.EndRewindEvent -= EndRewind;

            rewindSubscribed = false;
            if(usingDistanceBasedSubscrition)
                OnRewinEventUnSubscribe();
        }
    }
}