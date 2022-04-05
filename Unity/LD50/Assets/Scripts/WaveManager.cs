using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Shapes;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] TMP_Text gameModeText, scoreText, moneyText;
    [SerializeField] GameObject buildModeMenu, gameModeMenu, loseMenu;
    [SerializeField] Disc enemiesProgress;


    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] Transform[] SpawnGates;
    [SerializeField] int waveSpawnAmount = 10;
    int currentSpawnAmount;
    int enemiesCount;

    public int waveNumber = 1, score, _money;

    float waveLength;

    public int money
    {
        get { return _money; }
        set { _money = value; moneyText.text = "Money: " + _money; }
    }
    bool inWave;

    Transform enemiesTransform;

    ActionsHandler actionsHandler;

    // Start is called before the first frame update
    void Start()
    {
        enemiesTransform = new GameObject("Enemies Transform").transform;
        actionsHandler = FindObjectOfType<ActionsHandler>();

        FindObjectOfType<Temple>().GetComponent<Health>().onDie += OnTempleDeath;

        StartWave();
        money = 15;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartBuildMode()
    {
       
        scoreText.text = "Score: " + score;

        gameModeText.text = "Build Mode";
        buildModeMenu.SetActive(true);
        gameModeMenu.SetActive(false);
        actionsHandler.SetEnabled(true);
        inWave = false;

        FindObjectOfType<Temple>().GetComponent<Health>().ResetHealth();
    }

    public void StartWave()
    {
        enemiesProgress.AngRadiansEnd = Mathf.Deg2Rad * Mathf.Lerp(90, 450, 1);
        waveLength = waveNumber * 5;
        inWave = true;
        currentSpawnAmount = 0;
        gameModeText.text = "Wave in Progress: " + waveNumber;
        buildModeMenu.SetActive(false);
        gameModeMenu.SetActive(true);
        actionsHandler.SetEnabled(false);

        waveSpawnAmount = (int)Mathf.Pow(1.5f, waveNumber);

        StartCoroutine(StartSpawning());
    }

    public void CancelWave()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var item in enemies)
        {
            Destroy(item.gameObject);
        }
        enemiesCount = 0;

        StopAllCoroutines();
        StartBuildMode();
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator StartSpawning()
    {
        while (currentSpawnAmount < waveSpawnAmount)
        {
            currentSpawnAmount++;
            SpawnEnemy();
            yield return new WaitForSeconds(waveLength/waveSpawnAmount);
        }
    }

    void SpawnEnemy()
    {
        GameObject g = Instantiate(EnemyPrefab, SpawnGates[Random.Range(0, SpawnGates.Length)].position, Quaternion.identity, enemiesTransform);
        g.GetComponent<Health>().onDie += OnEnemyDeath;
        enemiesCount++;
    }

    bool gameOver;
    public void OnTempleDeath(Health h)
    {
        gameOver = true;
        loseMenu.SetActive(true);
        buildModeMenu.SetActive(false);
        gameModeMenu.SetActive(false);
        actionsHandler.SetEnabled(false);
    }


    void OnEnemyDeath(Health h)
    {
        enemiesCount--;
        enemiesProgress.AngRadiansEnd = Mathf.Deg2Rad * Mathf.Lerp(90f, 450f, (waveSpawnAmount - (currentSpawnAmount - enemiesCount)) / (float)waveSpawnAmount);
        if (enemiesCount == 0 && !gameOver)
        {
            StopCoroutine("StartSpawning");
            waveNumber++;
            score += waveSpawnAmount;
            money += waveSpawnAmount * 3;
            StartBuildMode();
        }
    }
}
