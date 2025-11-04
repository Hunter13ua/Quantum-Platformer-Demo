using Photon.Deterministic;
using Quantum.Physics3D;

namespace Quantum.PlatformerDemo
{
    public unsafe class LevelFinishSystem : SystemSignalsOnly, ISignalOnTriggerEnter3D
    {
        public void OnTriggerEnter3D(Frame frame, TriggerInfo3D info) 
        {
            EntityRef triggerEntity = info.Entity;
            EntityRef otherEntity   = info.Other;

            if (frame.Has<FinishZone>(triggerEntity) && frame.Has<PlayerCharacter>(otherEntity)) 
            {
                Log.Info("Level finished! Raising event...");
                frame.Events.OnLevelFinished();
            }
        }
    }
}
