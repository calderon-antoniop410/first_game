using System.Collections;
using System.Collections.Generic;
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
    private readonly List<SlimeEnemy> slimePool = new List<SlimeEnemy>();

    private void Start()
    {
        mainCamera = Camera.main;
        if (slimePrefab == null)
        {
            Debug.LogError("SlimeSpawner requires a slime prefab.", this);
            return;
        }

        for (int i = 0; i < maxSlimes; i++)
        {
            GameObject slimeObject = Instantiate(slimePrefab, transform.position, Quaternion.identity, transform);
            SlimeEnemy slime = slimeObject.GetComponent<SlimeEnemy>();
            if (slime == null)
            {
                Debug.LogError("The slime prefab requires a SlimeEnemy component.", slimeObject);
                Destroy(slimeObject);
                return;
            }

            slime.SetSpawner(this);
            slimeObject.SetActive(false);
            slimePool.Add(slime);
        }

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

        SlimeEnemy slime = slimePool.Find(pooledSlime => !pooledSlime.gameObject.activeSelf);
        if (slime != null)
        {
            slime.transform.position = spawnPosition;
            slime.transform.rotation = Quaternion.identity;
            slime.gameObject.SetActive(true);
            currentSlimeCount++;
        }
    }

    public void ReturnSlime(SlimeEnemy slime)
    {
        if (slime == null || !slime.gameObject.activeSelf)
        {
            return;
        }

        slime.gameObject.SetActive(false);
        currentSlimeCount = Mathf.Max(0, currentSlimeCount - 1);
    }
}