using UnityEngine.Scripting;
using Photon.Deterministic;

namespace Quantum.PlatformerDemo
{
    /// <summary>
    /// Main system responsible for handling player movement, jumping, and respawning in the platformer game.
    /// Processes entities with PlayerCharacter components and applies physics-based movement.
    /// </summary>
    [Preserve]
    public unsafe class PlatformerSystem : SystemMainThreadFilter<PlatformerSystem.Filter>, ISignalOnMapChanged
    {
        /// <summary>
        /// Filter struct defining the components required for this system to process an entity.
        /// </summary>
        public struct Filter
        {
            /// <summary>
            /// Reference to the entity being processed.
            /// </summary>
            public EntityRef Entity;

            /// <summary>
            /// Pointer to the entity's 3D transform component.
            /// </summary>
            public Transform3D* Transform;

            /// <summary>
            /// Pointer to the entity's 3D physics body component.
            /// </summary>
            public PhysicsBody3D* Body;

            /// <summary>
            /// Pointer to the entity's PlayerCharacter component.
            /// </summary>
            public PlayerCharacter* PlayerCharacter;
        }

        /// <summary>
        /// Flag indicating whether the scene/map has changed, used to trigger respawning.
        /// </summary>
        private bool sceneChanged = false;

        /// <summary>
        /// Main update method called each frame for entities matching the filter.
        /// Handles input processing, grounded state updates, and movement.
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="filter">The filter containing pointers to required components.</param>
        public override void Update(Frame frame, ref Filter filter)
        {
            Input* input = default;
            if (frame.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
            {
                input = frame.GetPlayerInput(playerLink->PlayerRef);
            }

            // Update grounded state
            filter.PlayerCharacter->IsGrounded = IsPlayerGrounded(frame, filter);

            UpdatePlayerMovement(frame, ref filter, input);
        }

        /// <summary>
        /// Called when the map changes, sets the sceneChanged flag to trigger respawning.
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="previousMap">Reference to the previously loaded map.</param>
        public void OnMapChanged(Frame frame, AssetRef<Map> previousMap)
        {
            sceneChanged = true;
        }

        /// <summary>
        /// Updates player movement based on input, applying forces for movement and jumping.
        /// Also handles out-of-bounds detection and respawning.
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="filter">The filter containing pointers to required components.</param>
        /// <param name="input">Pointer to the player's input data.</param>
        private void UpdatePlayerMovement(Frame frame, ref Filter filter, Input* input)
        {
            // Horizontal movement acceleration force
            FP _playerAcceleration = 8;

            // Vertical jump impulse force
            FP _playerJumpForce = 128;

            if (input->Left)
            {
                filter.Body->AddForce(FPVector3.Left * _playerAcceleration);
            }

            if (input->Right)
            {
                filter.Body->AddForce(-FPVector3.Left * _playerAcceleration);
            }

            if (input->Forward)
            {
                filter.Body->AddForce(FPVector3.Forward * _playerAcceleration);
            }
            else if (input->Backwards)
            {
                filter.Body->AddForce(-FPVector3.Forward * _playerAcceleration);
            }

            if (input->Jump && IsPlayerGrounded(frame, filter))
            {
                filter.Body->AddForce(FPVector3.Up * _playerJumpForce);
            }

            // if player falls off the map or the scene was changed, return to spawn pos
            if (IsPlayerOutOfBounds(frame, filter.Transform) || sceneChanged)
            {
                ReturnPlayerToSpawnPosition(frame, ref filter);
                sceneChanged = false;
            }
        }

        /// <summary>
        /// Checks if the player is grounded by performing a raycast downward from the player's position.
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="filter">The filter containing pointers to required components.</param>
        /// <returns>True if the player is grounded, false otherwise.</returns>
        private bool IsPlayerGrounded(Frame frame, Filter filter)
        {
            // Half height of the character capsule for raycast origin offset
            FP height = FP._0_50;

            // Small tolerance to account for floating point precision and capsule shape
            FP tolerance = FP._0_05;

            var origin = filter.Transform->Position;
            var direction = FPVector3.Down;
            var distance = height + tolerance;

            return frame.Physics3D.Raycast(origin, direction, distance) != null;
        }

        /// <summary>
        /// Checks if the player has fallen out of bounds (below a certain Y threshold).
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="playerTransform">Pointer to the player's transform component.</param>
        /// <returns>True if the player is out of bounds, false otherwise.</returns>
        private bool IsPlayerOutOfBounds(Frame frame, Transform3D* playerTransform)
        {
            // Y position threshold below which the player is considered out of bounds
            FP threshold = -FP._10;
            return playerTransform->Position.Y < threshold;
        }

        /// <summary>
        /// Teleports the player back to the spawn position and resets their physics state.
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="filter">The filter containing pointers to required components.</param>
        private void ReturnPlayerToSpawnPosition(Frame frame, ref Filter filter)
        {
            // Example spawn position - replace later with proper spawn point system
            FPVector3 spawnPos = new FPVector3(-FP._1 - FP._0_50, FP._0, FP._0);

            // Clear all applied forces
            filter.Body->ClearForce();
            filter.Body->ClearTorque();

            // Reset velocity and angular velocity to zero
            filter.Body->Velocity = FPVector3.Zero;
            filter.Body->AngularVelocity = FPVector3.Zero;

            // Teleport the player to the spawn position
            filter.Transform->Teleport(frame, spawnPos);
        }
    }
}
