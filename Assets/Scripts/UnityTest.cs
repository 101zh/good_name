using UnityEngine;
public class InstantiationExample : MonoBehaviour 
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;

    // This script will simply instantiate the Prefab when the game starts.
    public void SwordAttack()
    {
        for (var i = 0; i < 10; i++)
        {
        // Instantiate at position (0, 0, 0) and zero rotation.
        Instantiate(myPrefab, new Vector2(transform.position.x, transform.position.y + i * 5.0f), Quaternion.identity);
        }
    }
}