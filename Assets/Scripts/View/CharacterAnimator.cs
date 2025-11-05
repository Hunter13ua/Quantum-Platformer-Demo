using Photon.Deterministic;
using UnityEngine;

namespace Quantum.PlatformerDemo
{
    public unsafe class CharacterAnimator : QuantumEntityViewComponent
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float rotationSpeed = 10f;

        private static readonly int MovementSpeedHash = Animator.StringToHash("MovementSpeed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

        public override void OnUpdateView()
        {
            if (PredictedFrame == null) return;

            // Try to get the components we need
            if (!PredictedFrame.Unsafe.TryGetPointer(EntityRef, out Transform3D* transform)) return;
            if (!PredictedFrame.Unsafe.TryGetPointer(EntityRef, out PhysicsBody3D* body)) return;
            if (!PredictedFrame.Unsafe.TryGetPointer(EntityRef, out PlayerCharacter* playerCharacter)) return;

            // Get horizontal velocity magnitude for movement speed
            var horizontalVelocity = new Vector3(
                (float)body->Velocity.X,
                0f,
                (float)body->Velocity.Z
            );
            float movementSpeed = horizontalVelocity.magnitude;

            // Set animator parameters
            animator.SetFloat(MovementSpeedHash, movementSpeed);
            animator.SetBool(IsGroundedHash, playerCharacter->IsGrounded);

            // Rotate character based on movement direction
            if (horizontalVelocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
    }
}
