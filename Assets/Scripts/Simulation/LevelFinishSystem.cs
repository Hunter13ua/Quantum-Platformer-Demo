using Photon.Deterministic;
using Quantum.Physics3D;

namespace Quantum.PlatformerDemo
{
    /// <summary>
    /// System that handles level completion when players enter finish zones.
    /// Transitions to the next level when a player character collides with a finish zone trigger.
    /// </summary>
    public unsafe class LevelFinishSystem : SystemSignalsOnly, ISignalOnTriggerEnter3D
    {
        /// <summary>
        /// Called when any 3D trigger collision occurs in the physics simulation.
        /// Checks if a player has entered a finish zone and transitions to the next level.
        /// </summary>
        /// <param name="frame">The current simulation frame.</param>
        /// <param name="info">Information about the trigger collision event.</param>
        public void OnTriggerEnter3D(Frame frame, TriggerInfo3D info)
        {
            EntityRef triggerEntity = info.Entity;
            EntityRef otherEntity = info.Other;

            // Check if the collision involves a player entering a finish zone
            if (frame.Has<FinishZone>(triggerEntity) && frame.Has<PlayerCharacter>(otherEntity))
            {
                // Load the next level map as configured in ScenesConfig
                ScenesConfig config = frame.FindAsset(frame.RuntimeConfig.ScenesConfig);
                frame.Map = frame.FindAsset(config.Level2Map);
            }
        }
    }
}
