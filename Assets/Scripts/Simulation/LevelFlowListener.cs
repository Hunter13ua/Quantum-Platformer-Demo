using System;
using Quantum;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quantum.PlatformerDemo
{
    public class LevelFlowListener : QuantumMonoBehaviour
    {
        [SerializeField] private string nextSceneName = "Level2";

        private IDisposable _eventSubscription;

        private void OnEnable()
        {
            // Subscribe to the OnLevelFinished event
            _eventSubscription = QuantumEvent.SubscribeManual((EventOnLevelFinished e) =>
            {
                // Level finished event received, load next scene
                SceneManager.LoadScene(nextSceneName);
            });
        }

        private void OnDisable()
        {
            // Unsubscribe when disabled
            _eventSubscription?.Dispose();
        }
    }
}
