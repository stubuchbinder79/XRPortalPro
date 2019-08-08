using UnityEngine;

public class SpawnObjectOnAwake : MonoBehaviour
{
    [Tooltip("the prefab to instantiate once this game object is added to the scene")]
    public GameObject prefabToSpawn;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }


    private void Awake()
    {
        if (prefabToSpawn != null)
            spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        Destroy(spawnedObject);
    }
}
