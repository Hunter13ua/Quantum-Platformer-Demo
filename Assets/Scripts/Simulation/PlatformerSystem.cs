using UnityEngine.Scripting;
using Photon.Deterministic;

namespace Quantum.PlatformerDemo
{
    [Preserve]
    public unsafe class PlatformerSystem : SystemMainThreadFilter<PlatformerSystem.Filter>, ISignalOnMapChanged 
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public PhysicsBody3D* Body;
            public PlayerCharacter* PlayerCharacter;
        }

        private bool sceneChanged = false;

        public override void Update(Frame frame, ref Filter filter)
        {
            Input* input = default;
            if (frame.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
            {
                input = frame.GetPlayerInput(playerLink->PlayerRef);
            }

            UpdatePlayerMovement(frame, ref filter, input);
        }

        public void OnMapChanged(Frame frame, AssetRef<Map> previousMap)
        {
            sceneChanged = true;
        }

        private void UpdatePlayerMovement(Frame frame, ref Filter filter, Input* input)
        {
            FP _playerAcceleration = 8;
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

            // if player falls of the map or the scene was changed, return to spawn pos
            if (IsPlayerOutOfBounds(frame, filter.Transform) || sceneChanged)
            {
                ReturnPlayerToSpawnPosition(frame, ref filter);
                sceneChanged = false;
            }
        }

        private bool IsPlayerGrounded(Frame frame, Filter filter)
        {
            FP height = FP._0_50; // half height of the character
            FP tolerance = FP._0_05; // small offset

            var origin = filter.Transform->Position;
            var direction = FPVector3.Down;
            var distance = height + tolerance;


            return frame.Physics3D.Raycast(origin, direction, distance) != null;
        }

        private bool IsPlayerOutOfBounds(Frame frame, Transform3D* playerTransform)
        {
            FP threshold = -FP._10;
            return playerTransform->Position.Y < threshold;
        }
        
        private void ReturnPlayerToSpawnPosition(Frame frame, ref Filter filter)
        {
            // Example spawn position - replace later with proper spawn point system
            FPVector3 spawnPos = new FPVector3(-FP._1 - FP._0_50, FP._0, FP._0);

            // clear forces
            filter.Body->ClearForce();
            filter.Body->ClearTorque();
            //reset inertia
            filter.Body->Velocity = FPVector3.Zero;
            filter.Body->AngularVelocity = FPVector3.Zero;

            // teleport
            filter.Transform->Teleport(frame, spawnPos);
        }
    }
}
