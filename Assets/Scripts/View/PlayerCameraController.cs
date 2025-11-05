using Photon.Deterministic;
using UnityEngine;

namespace Quantum.PlatformerDemo
{
    /// <summary>
    /// View component that controls camera activation based on player ownership.
    /// Ensures only the local player's camera is active in multiplayer scenarios.
    /// </summary>
    public unsafe class PlayerCameraController : QuantumEntityViewComponent
    {
        /// <summary>
        /// Reference to the Camera component that should be controlled by this script.
        /// </summary>
        [SerializeField] private Camera playerCamera;

        /// <summary>
        /// Called each frame to update the camera's active state based on simulation data.
        /// Enables the camera only for the local player in multiplayer games.
        /// </summary>
        public override void OnUpdateView()
        {
            if (PredictedFrame == null) return;

            // Check if this entity has a PlayerLink component (indicating it's a player entity)
            if (!PredictedFrame.Unsafe.TryGetPointer(EntityRef, out PlayerLink* playerLink))
            {
                // Not a player entity, disable camera
                playerCamera.enabled = false;
                return;
            }

            // Check if this is the local player's entity (the player controlling this client)
            bool isLocalPlayer = QuantumRunner.Default.Game.PlayerIsLocal(playerLink->PlayerRef);

            // Enable camera only for the local player to avoid multiple active cameras
            playerCamera.enabled = isLocalPlayer;
        }

        /// <summary>
        /// Called when the script is loaded or a value is changed in the inspector.
        /// Automatically assigns the Camera component if not set.
        /// </summary>
        private void OnValidate()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponent<Camera>();
            }
        }
    }
}
