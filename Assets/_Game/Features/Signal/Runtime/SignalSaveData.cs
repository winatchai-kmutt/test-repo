using System;

namespace Game.Features.Signal
{
    [Serializable]
    public sealed class SignalSaveData
    {
        public int schemaVersion;
        public string signalId;
        public bool isActive;
    }
}
