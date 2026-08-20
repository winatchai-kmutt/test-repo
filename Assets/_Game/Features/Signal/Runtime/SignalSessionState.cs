using Game.Shared;

namespace Game.Features.Signal
{
    public readonly struct SignalSessionState
    {
        public SignalSessionState(ContentId contentId, bool isActive)
        {
            ContentId = contentId;
            IsActive = isActive;
        }

        public ContentId ContentId { get; }

        public bool IsActive { get; }
    }
}
