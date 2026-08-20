namespace Game.Features.Signal
{
    public static class SignalRules
    {
        public static SignalSessionState Toggle(SignalSessionState current)
        {
            return new SignalSessionState(current.ContentId, !current.IsActive);
        }
    }
}
