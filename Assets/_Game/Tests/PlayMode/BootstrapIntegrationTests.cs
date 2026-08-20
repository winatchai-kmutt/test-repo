using System.Collections;
using Game.Bootstrap;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Game.Tests.PlayMode
{
    public sealed class BootstrapIntegrationTests : InputTestFixture
    {
        [UnityTest]
        public IEnumerator Bootstrap_routes_a_bound_input_action_to_state_and_presentation()
        {
            var keyboard = InputSystem.AddDevice<Keyboard>();
            yield return SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);
            yield return null;

            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>();
            Assert.That(bootstrap, Is.Not.Null);
            Assert.That(bootstrap.IsReady, Is.True);
            Assert.That(bootstrap.HasPresenter, Is.True);
            Assert.That(bootstrap.PresenterMatchesCurrentState, Is.True);

            var previousState = bootstrap.CurrentState.IsActive;
            Press(keyboard.spaceKey);
            yield return null;

            Assert.That(bootstrap.CurrentState.IsActive, Is.Not.EqualTo(previousState));
            Assert.That(bootstrap.PresenterMatchesCurrentState, Is.True);
        }
    }
}
