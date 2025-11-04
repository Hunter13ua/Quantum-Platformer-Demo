using UnityEngine.Scripting;
using Photon.Deterministic;

namespace Quantum.PlatformerDemo
{
    [Preserve]
    public unsafe class PlatformerSystem : SystemMainThreadFilter<PlatformerSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public PhysicsBody3D* Body;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            Input* input = default;
            if (frame.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
            {
                input = frame.GetPlayerInput(playerLink->PlayerRef);
            }

            UpdatePlayerMovement(frame, ref filter, input);
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
    }
}
