using UnityEngine;

//This class is to randomly generate the level using in project assets.
//It will be used in the future when we have more time to polish the game.
//For now, we will just use the pre-made level.
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int levelLength = 20;
    [SerializeField] private float platformSpacing = 5f;
    [SerializeField] private float platformWidth = 2f;
    [SerializeField] private GameObject[] groundPrefabs;
    [SerializeField] private GameObject[] bridgePrefabs;
    [SerializeField] private GameObject[] propPrefabs;
    [SerializeField] private int propDensity = 3;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        LoadPrefabs();

        for (int i = 0; i < levelLength; i++)
        {
            Vector3 platformPosition = new Vector3(0, 0, i * platformSpacing);

            //Randomly choose between ground and bridge platforms
            GameObject[] selectedPrefabs = Random.value > 0.3f ? groundPrefabs : bridgePrefabs;

            if (selectedPrefabs.Length > 0)
            {
                GameObject prefab = selectedPrefabs[Random.Range(0, selectedPrefabs.Length)];
                Instantiate(prefab, platformPosition, Quaternion.identity);
            }

            //Randomly place props on some platforms
            if (Random.value < (propDensity / 10f) && propPrefabs.Length > 0)
            {
                Vector3 propPosition = platformPosition + new Vector3(Random.Range(-platformWidth, platformWidth), 2f, 0);
                GameObject propPrefab = propPrefabs[Random.Range(0, propPrefabs.Length)];
                Instantiate(propPrefab, propPosition, Quaternion.identity);
            }
        }
    }

    private void LoadPrefabs()
    {
        //Only attempt to load from Resources if not already assigned in Inspector
        if (groundPrefabs == null || groundPrefabs.Length == 0)
        {
            groundPrefabs = Resources.LoadAll<GameObject>("Prefabs/Environments");
            if (groundPrefabs.Length == 0)
            {
                Debug.LogWarning("No ground prefabs assigned. Please assign them in the Inspector.");
            }
        }

        if (bridgePrefabs == null || bridgePrefabs.Length == 0)
        {
            bridgePrefabs = Resources.LoadAll<GameObject>("Prefabs/Environments");
            if (bridgePrefabs.Length == 0)
            {
                Debug.LogWarning("No bridge prefabs assigned. Please assign them in the Inspector.");
            }
        }

        if (propPrefabs == null || propPrefabs.Length == 0)
        {
            propPrefabs = Resources.LoadAll<GameObject>("Prefabs/Props");
        }
    }
}
