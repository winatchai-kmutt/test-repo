using Game.Features.Signal;
using Game.Shared;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class SignalRulesTests
    {
        [Test]
        public void Toggle_flips_the_runtime_state_without_changing_its_content_id()
        {
            var initial = new SignalSessionState(new ContentId("neutral-signal"), false);

            var toggled = SignalRules.Toggle(initial);

            Assert.That(toggled.ContentId, Is.EqualTo(initial.ContentId));
            Assert.That(toggled.IsActive, Is.True);
        }

        [Test]
        public void Current_save_schema_round_trips_a_stable_content_id()
        {
            var expectedId = new ContentId("neutral-signal");
            var initial = new SignalSessionState(expectedId, true);

            var restored = SignalSaveMapper.FromSaveData(SignalSaveMapper.ToSaveData(initial), expectedId);

            Assert.That(restored.ContentId, Is.EqualTo(expectedId));
            Assert.That(restored.IsActive, Is.True);
        }

        [Test]
        public void Legacy_schema_migrates_when_the_definition_identity_is_known()
        {
            var restored = SignalSaveMapper.FromSaveData(
                new SignalSaveData
                {
                    schemaVersion = 0,
                    signalId = "obsolete-signal-name",
                    isActive = true,
                },
                new ContentId("neutral-signal"));

            Assert.That(restored.ContentId.Value, Is.EqualTo("neutral-signal"));
            Assert.That(restored.IsActive, Is.True);
        }

        [Test]
        public void Unknown_future_schema_falls_back_to_a_safe_default()
        {
            var restored = SignalSaveMapper.FromSaveData(
                new SignalSaveData
                {
                    schemaVersion = SignalSaveMapper.CurrentSchemaVersion + 1,
                    signalId = "neutral-signal",
                    isActive = true,
                },
                new ContentId("neutral-signal"));

            Assert.That(restored.IsActive, Is.False);
        }
    }
}
