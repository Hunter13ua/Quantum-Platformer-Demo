using UnityEngine;

/// <summary>
/// Forces anchor rotation to always be identical to world rotation.
/// </summary>
public class CharacterAnchor : MonoBehaviour
{
    private void LateUpdate()
    {
        if (transform.hasChanged)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
