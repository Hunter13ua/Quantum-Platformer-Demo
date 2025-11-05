using Photon.Deterministic;
using UnityEngine;

namespace Quantum.PlatformerDemo
{
    /// <summary>
    /// View component that synchronizes character animation and rotation with the simulation state.
    /// Updates animator parameters based on movement speed and grounded state, and rotates the character towards movement direction.
    /// </summary>
    public unsafe class CharacterAnimator : QuantumEntityViewComponent
    {
        /// <summary>
        /// Reference to the Unity Animator component that controls character animations.
        /// </summary>
        [SerializeField] private Animator animator;

        /// <summary>
        /// Speed at which the character rotates to face movement direction (degrees per second).
        /// </summary>
        [SerializeField] private float rotationSpeed = 10f;

        /// <summary>
        /// Cached hash for the "MovementSpeed" animator parameter to avoid string lookups.
        /// </summary>
        private static readonly int MovementSpeedHash = Animator.StringToHash("MovementSpeed");

        /// <summary>
        /// Cached hash for the "IsGrounded" animator parameter to avoid string lookups.
        /// </summary>
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

        /// <summary>
        /// Called each frame to update the view representation based on simulation state.
        /// Synchronizes animation parameters and character rotation with the predicted frame data.
        /// </summary>
        public override void OnUpdateView()
        {
            if (PredictedFrame == null) return;

            // Try to get the required simulation components
            if (!PredictedFrame.Unsafe.TryGetPointer(EntityRef, out Transform3D* transform)) return;
            if (!PredictedFrame.Unsafe.TryGetPointer(EntityRef, out PhysicsBody3D* body)) return;
            if (!PredictedFrame.Unsafe.TryGetPointer(EntityRef, out PlayerCharacter* playerCharacter)) return;

            // Calculate horizontal movement speed from physics velocity (ignoring vertical component)
            var horizontalVelocity = new Vector3(
                (float)body->Velocity.X,
                0f,
                (float)body->Velocity.Z
            );
            float movementSpeed = horizontalVelocity.magnitude;

            // Update animator parameters for movement and grounded state
            animator.SetFloat(MovementSpeedHash, movementSpeed);
            animator.SetBool(IsGroundedHash, playerCharacter->IsGrounded);

            // Rotate character to face movement direction if moving
            if (horizontalVelocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Called when the script is loaded or a value is changed in the inspector.
        /// Automatically assigns the Animator component if not set.
        /// </summary>
        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
    }
}
