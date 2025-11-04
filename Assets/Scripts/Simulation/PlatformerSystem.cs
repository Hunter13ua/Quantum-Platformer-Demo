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
            // gets the input for player 0
            var input = frame.GetPlayerInput(0);

            UpdatePlayerMovement(frame, ref filter, input);
        }

        private void UpdatePlayerMovement(Frame frame, ref Filter filter, Input* input)
        {
            FP _playerAcceleration = 10;
            FP _playerJumpForce = 50;

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

            if (input->Jump)
            {
                filter.Body->AddForce(FPVector3.Up * _playerJumpForce);
            }
        }
    }
}
