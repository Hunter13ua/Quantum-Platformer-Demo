using UnityEngine;

/// <summary>
/// Component that forces the character's anchor point to maintain world-aligned rotation.
/// Prevents the character from rotating with the camera or other transformations.
/// </summary>
public class CharacterAnchor : MonoBehaviour
{
    /// <summary>
    /// Called after all Update methods have been called.
    /// Resets the transform rotation to identity if it has changed, maintaining world alignment.
    /// </summary>
    private void LateUpdate()
    {
        if (transform.hasChanged)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
