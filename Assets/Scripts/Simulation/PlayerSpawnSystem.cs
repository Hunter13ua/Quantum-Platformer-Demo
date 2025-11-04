using System.Runtime.Versioning;
using UnityEngine.Scripting;
using Photon.Deterministic;

namespace Quantum.PlatformerDemo
{
    [Preserve]
    public unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
        {
            RuntimePlayer data = frame.GetPlayerData(player);

            // resolve the reference to the avatar prototype.
            var entityPrototypeAsset = frame.FindAsset<EntityPrototype>(data.PlayerAvatar);

            // Create a new entity for the player based on the prototype.
            var playerEntity = frame.Create(entityPrototypeAsset);

            // Create a PlayerLink component. Initialize it with the player. Add the component to the player entity.
            frame.Add(playerEntity, new PlayerLink { PlayerRef = player });

            SetSpawnPosition(frame, playerEntity);
        }
        
        private void SetSpawnPosition(Frame frame, EntityRef playerEntity)
        {
            // Get transform and set position
            if (frame.Unsafe.TryGetPointer(playerEntity, out Transform3D* transform))
            {
                // Example spawn position - replace later with proper spawn point system
                FPVector3 spawnPos = new FPVector3(-FP._1 - FP._0_50, FP._0, FP._0);
                transform->Position = spawnPos;
            }
        }
    }
}