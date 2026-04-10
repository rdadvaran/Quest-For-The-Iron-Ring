using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject cylinderPrefab;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private GameObject warningPrefab;

    [Header("Plane Bounds")]
    [SerializeField] private float minX = -5.6f;
    [SerializeField] private float maxX = 5.6f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    [Header("Spawn Timing")]
    [SerializeField] private float warningTime = 1f;
    [SerializeField] private float phase2SpawnRate = 2.5f;
    [SerializeField] private float phase3SpawnRate = 2f;

    private int currentPhase = 1;
    private Coroutine spawnRoutine;
    private bool cubeSpawned = false;

    public void UpdateSpawnTable(int phase)
    {
        currentPhase = phase;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        if (currentPhase == 1)
        {
            SpawnPhase1();
        }
        else
        {
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    private void SpawnPhase1()
    {
        if (!cubeSpawned && cubePrefab != null)
        {
            Vector3 cubePos = new Vector3(0f, 3f, 0f);
            StartCoroutine(SpawnWithWarning(cubePrefab, cubePos, Quaternion.identity, Vector2.zero, false));
            cubeSpawned = true;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentPhase == 2)
            {
                SpawnCylinder();
                yield return new WaitForSeconds(phase2SpawnRate);
            }
            else if (currentPhase == 3)
            {
                SpawnCylinder();
                yield return new WaitForSeconds(phase3SpawnRate * 0.5f);

                SpawnSphere();
                yield return new WaitForSeconds(phase3SpawnRate * 0.5f);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SpawnCylinder()
    {
        if (cylinderPrefab == null) return;

        bool spawnLeft = Random.value < 0.5f;
        float y = Random.Range(minY, maxY);

        Vector3 spawnPos;
        Vector2 moveDirection;

        if (spawnLeft)
        {
            spawnPos = new Vector3(minX, y, 0f);
            moveDirection = Vector2.right;
        }
        else
        {
            spawnPos = new Vector3(maxX, y, 0f);
            moveDirection = Vector2.left;
        }

        StartCoroutine(SpawnWithWarning(cylinderPrefab, spawnPos, Quaternion.identity, moveDirection, true));
    }

    private void SpawnSphere()
    {
        if (spherePrefab == null) return;

        float x = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(x, maxY, 0f);
        Vector2 moveDirection = Vector2.down;

        StartCoroutine(SpawnWithWarning(spherePrefab, spawnPos, Quaternion.identity, moveDirection, true));
    }

    private IEnumerator SpawnWithWarning(GameObject prefab, Vector3 spawnPos, Quaternion rotation, Vector2 moveDirection, bool setDirection)
    {
        GameObject warning = null;

        if (warningPrefab != null)
        {
            warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);
        }

        yield return new WaitForSeconds(warningTime);

        if (warning != null)
        {
            Destroy(warning);
        }

        GameObject spawnedObject = Instantiate(prefab, spawnPos, rotation);

        if (setDirection)
        {
            CylinderBehavior cylinder = spawnedObject.GetComponent<CylinderBehavior>();
            if (cylinder != null)
            {
                cylinder.SetDirection(moveDirection);
            }

            SphereBehavior sphere = spawnedObject.GetComponent<SphereBehavior>();
            if (sphere != null)
            {
                sphere.SetDirection(moveDirection);
            }
        }
    }
}