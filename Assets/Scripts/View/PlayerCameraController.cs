using Photon.Deterministic;
using UnityEngine;

namespace Quantum.PlatformerDemo
{
    public unsafe class PlayerCameraController : QuantumEntityViewComponent
    {
        [SerializeField] private Camera playerCamera;

        public override void OnUpdateView()
        {
            if (PredictedFrame == null) return;

            // Check if this entity has a PlayerLink component
            if (!PredictedFrame.Unsafe.TryGetPointer(EntityRef, out PlayerLink* playerLink))
            {
                // Not a player entity, disable camera
                playerCamera.enabled = false;
                return;
            }

            // Check if this is the local player's entity
            bool isLocalPlayer = QuantumRunner.Default.Game.PlayerIsLocal(playerLink->PlayerRef);

            // Enable camera only for local player
            playerCamera.enabled = isLocalPlayer;
        }

        private void OnValidate()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponent<Camera>();
            }
        }
    }
}
