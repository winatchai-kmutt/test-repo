using Game.Shared;

namespace Game.Features.Signal
{
    public static class SignalSaveMapper
    {
        public const int CurrentSchemaVersion = 1;

        public static SignalSaveData ToSaveData(SignalSessionState state)
        {
            return new SignalSaveData
            {
                schemaVersion = CurrentSchemaVersion,
                signalId = state.ContentId.Value,
                isActive = state.IsActive,
            };
        }

        public static SignalSessionState FromSaveData(SignalSaveData data, ContentId expectedContentId)
        {
            if (!expectedContentId.IsValid || data == null || data.schemaVersion > CurrentSchemaVersion)
            {
                return new SignalSessionState(expectedContentId, false);
            }

            if (data.schemaVersion == 0)
            {
                return new SignalSessionState(expectedContentId, data.isActive);
            }

            if (!string.Equals(data.signalId, expectedContentId.Value, System.StringComparison.Ordinal))
            {
                return new SignalSessionState(expectedContentId, false);
            }

            return new SignalSessionState(expectedContentId, data.isActive);
        }
    }
}
