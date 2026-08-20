using Game.Shared;
using UnityEngine;

namespace Game.Features.Signal
{
    public interface ISignalSaveStore
    {
        bool TryLoad(ContentId contentId, out SignalSaveData data);

        void Save(ContentId contentId, SignalSaveData data);
    }

    public sealed class SignalPlayerPrefsSaveStore : ISignalSaveStore
    {
        private const string KeyPrefix = "validation.signal.";

        public bool TryLoad(ContentId contentId, out SignalSaveData data)
        {
            var key = ToKey(contentId);
            if (!PlayerPrefs.HasKey(key))
            {
                data = null;
                return false;
            }

            data = JsonUtility.FromJson<SignalSaveData>(PlayerPrefs.GetString(key));
            return data != null;
        }

        public void Save(ContentId contentId, SignalSaveData data)
        {
            PlayerPrefs.SetString(ToKey(contentId), JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        private static string ToKey(ContentId contentId)
        {
            return KeyPrefix + contentId.Value;
        }
    }
}
