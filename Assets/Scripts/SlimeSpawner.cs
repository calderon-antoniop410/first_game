using System.Collections;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxSlimes = 10;

    [Header("Off-Screen Spawn Buffer")]
    [Tooltip("Distance past the screen edge where slimes spawn.")]
    [SerializeField] private float spawnPadding = 2f; 

    private Camera mainCamera;
    private int currentSlimeCount = 0;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnSlimesRoutine());
    }

    private IEnumerator SpawnSlimesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentSlimeCount < maxSlimes)
            {
                SpawnSlimeOffScreen();
            }
        }
    }

    private void SpawnSlimeOffScreen()
    {
        if (mainCamera == null) return;

        // 1. Get the camera's half-height and half-width in world units
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        Vector3 camPos = mainCamera.transform.position;

        // 2. Pick a random side of the screen: 0 = Top, 1 = Bottom, 2 = Left, 3 = Right
        int side = Random.Range(0, 4);
        Vector3 spawnPosition = Vector3.zero;

        switch (side)
        {
            case 0: // TOP edge
                spawnPosition.x = Random.Range(camPos.x - camWidth, camPos.x + camWidth);
                spawnPosition.y = camPos.y + camHeight + spawnPadding;
                break;

            case 1: // BOTTOM edge
                spawnPosition.x = Random.Range(camPos.x - camWidth, camPos.x + camWidth);
                spawnPosition.y = camPos.y - camHeight - spawnPadding;
                break;

            case 2: // LEFT edge
                spawnPosition.x = camPos.x - camWidth - spawnPadding;
                spawnPosition.y = Random.Range(camPos.y - camHeight, camPos.y + camHeight);
                break;

            case 3: // RIGHT edge
                spawnPosition.x = camPos.x + camWidth + spawnPadding;
                spawnPosition.y = Random.Range(camPos.y - camHeight, camPos.y + camHeight);
                break;
        }

        spawnPosition.z = 0f; // Ensure standard 2D depth

        // 3. Instantiate the slime at the calculated off-screen position
        GameObject newSlime = Instantiate(slimePrefab, spawnPosition, Quaternion.identity);

        SlimeEnemy slimeEnemy = newSlime.GetComponent<SlimeEnemy>();
        if (slimeEnemy != null)
        {
            slimeEnemy.SetSpawner(this);
        }
        
        currentSlimeCount++;
    }

    public void OnSlimeDied()
    {
        currentSlimeCount = Mathf.Max(0, currentSlimeCount - 1);
    }
}