using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objSpawnPrefabs;

    [SerializeField]
    [Range(1, 100)] private int numToSpawn = 20;

    [SerializeField]
    [Range(0, 10000)] private float minDistanceBetweenObj = 10;

    [SerializeField] private Collider culTarget;

    [SerializeField] private float hideSpawnedObjDistance = 50f;

    protected GameObject[] CurrentSpawnedObj;

    private BoxCollider bounds;

    public void SpawnObjects()
    {
        CurrentSpawnedObj = new GameObject[numToSpawn];
        bounds = GetComponent<BoxCollider>();

        for (var i = 0; i < numToSpawn; i++)
        {
            var rndPosWithin = new Vector3(Random.Range(-1f, 1f) * bounds.size.x / 2,
                Random.Range(-1f, 1f) * bounds.size.x / 2,
                Random.Range(-1f, 1f) * bounds.size.z / 2);
            rndPosWithin += transform.position;

            if (!IsObjectTooClose(rndPosWithin))
            {
                var spawnedObject = Instantiate(objSpawnPrefabs[Random.Range(0, objSpawnPrefabs.Length)], rndPosWithin, Quaternion.identity);
                CurrentSpawnedObj[i] = spawnedObject;
                CurrentSpawnedObj[i].transform.parent = transform;

                var cullingSphere = new GameObject("Culling Sphere");
                cullingSphere.transform.position = rndPosWithin;
                cullingSphere.transform.parent = spawnedObject.transform;

                var spawnCollider = cullingSphere.AddComponent<SphereCollider>();
                spawnCollider.radius = hideSpawnedObjDistance;
                var spawnCuller = cullingSphere.AddComponent<CullObject>();
                spawnCuller.culTarget = culTarget;
            }
        }
    }

    private bool IsObjectTooClose (Vector3 targetPosition)
    {
        foreach(var t in CurrentSpawnedObj)
        {
            if (t != null)
            {
                if (Vector3.Distance(targetPosition, t.transform.position) <= minDistanceBetweenObj)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
