using Photon.Deterministic;
using UnityEngine;

namespace Quantum.PlatformerDemo
{
    /// <summary>
    /// Component that polls Unity input and translates it to Quantum input for the platformer game.
    /// Maps keyboard inputs to movement and jump actions, supporting both WASD and arrow key controls.
    /// </summary>
    public class PlatformerInput : MonoBehaviour
    {
        /// <summary>
        /// Called when the component becomes enabled and active.
        /// Subscribes to Quantum's input polling callback.
        /// </summary>
        private void OnEnable()
        {
            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        }

        /// <summary>
        /// Polls current Unity input state and converts it to Quantum input structure.
        /// Called by Quantum's input system to gather player input for the simulation.
        /// </summary>
        /// <param name="callback">The callback object used to set the input data for the current frame.</param>
        public void PollInput(CallbackPollInput callback)
        {
            Quantum.Input i = new Quantum.Input();

            // Map movement inputs - supporting both WASD and arrow keys for accessibility
            // Note: Use GetKey() instead of GetKeyDown/Up. Quantum calculates up/down internally.
            i.Left = UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow);
            i.Right = UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow);
            i.Forward = UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow);
            i.Backwards = UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow);
            i.Jump = UnityEngine.Input.GetKey(KeyCode.Space);

            callback.SetInput(i, DeterministicInputFlags.Repeatable);
        }
    }
}
