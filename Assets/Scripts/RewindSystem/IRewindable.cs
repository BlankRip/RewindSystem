using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blank.RewindSystem
{
    public interface IRewindable
    {
        void RecordData();
        void StartRewind();
        void Rewind();
        void EndRewind();
    }
}