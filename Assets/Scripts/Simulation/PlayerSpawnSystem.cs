using System.Runtime.Versioning;
using UnityEngine.Scripting;
using Photon.Deterministic;

namespace Quantum.PlatformerDemo
{
    /// <summary>
    /// System responsible for spawning player entities when players join the game.
    /// Creates entities based on player avatar prototypes and positions them at spawn locations.
    /// </summary>
    [Preserve]
    public unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        /// <summary>
        /// Called when a new player is added to the game session.
        /// Creates a player entity from the player's avatar prototype and sets up initial state.
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="player">Reference to the player being added.</param>
        /// <param name="firstTime">True if this is the first time the player is being added.</param>
        public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
        {
            RuntimePlayer data = frame.GetPlayerData(player);

            // Resolve the reference to the avatar prototype asset
            var entityPrototypeAsset = frame.FindAsset<EntityPrototype>(data.PlayerAvatar);

            // Create a new entity for the player based on the prototype
            var playerEntity = frame.Create(entityPrototypeAsset);

            // Create a PlayerLink component to associate the entity with the player
            frame.Add(playerEntity, new PlayerLink { PlayerRef = player });

            SetSpawnPosition(frame, playerEntity);
        }

        /// <summary>
        /// Sets the initial spawn position for a newly created player entity.
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="playerEntity">The entity reference of the player to position.</param>
        private void SetSpawnPosition(Frame frame, EntityRef playerEntity)
        {
            // Get the transform component and set the initial position
            if (frame.Unsafe.TryGetPointer(playerEntity, out Transform3D* transform))
            {
                // Example spawn position - replace later with proper spawn point system
                // Positioned slightly left of origin to avoid conflicts with other players
                FPVector3 spawnPos = new FPVector3(-FP._1 - FP._0_50, FP._0, FP._0);
                transform->Position = spawnPos;
            }
        }
    }
}
