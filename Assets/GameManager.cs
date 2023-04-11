using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 1;
    public int maxLevel = 10;

    public GameObject gameOverPanel;

    private bool isGameOver = false;
    public List<GameObject> rewardPrefabs;
    public List<int> collectedRewards;

    public static GameManager instance;

    public Color[] colors = { Color.red, Color.blue, Color.yellow };

    public Text currentLevelText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        // İlk seviye ayarlamaları ve nesnelerin yerleştirilmesi
        SetupLevel(currentLevel);
    }
    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Oyunu yeniden başlatma
            }
        }
    }

    void SetupLevel(int level)
    {
        SpawnGhosts(level);
        SpawnRewards(level);
    }

    void SpawnGhosts(int ghostCount)
    {
        // Hayaletlerin başlangıç pozisyonlarını ayarlama ve oluşturma
        // Önceden oluşturulan hayalet nesnelerini yok etme
        GameObject[] existingGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in existingGhosts)
        {
            Destroy(ghost);
        }

        for (int i = 0; i < ghostCount; i++)
        {
            GameObject ghostPrefab = Resources.Load<GameObject>("Prefabs/Ghost");
            GameObject ghostInstance = Instantiate(ghostPrefab);

            // Hayaletlerin renklerini ayarlama
            SpriteRenderer sr = ghostInstance.GetComponent<SpriteRenderer>();
            sr.color = colors[Random.Range(0, colors.Length)];

            // Hayaletlerin başlangıç pozisyonlarını ayarlama
            Vector3 randomPosition = new Vector3(Random.Range(-13f, 13f), Random.Range(-4f, 4f), 0);

            // Hayaletlerin oyuncunun üzerine doğru oluşmaması için bir kontrol yapılıyor
            while (Physics2D.OverlapCircle(randomPosition, 4f, LayerMask.GetMask("Player")))
            {
               randomPosition = new Vector3(Random.Range(-13f, 13f), Random.Range(-4f, 4f), 0);
            }
            ghostInstance.transform.position = randomPosition;

            // İlk hayaletin oyuncuyu takip etmesini sağlama
            GhostController ghostController = ghostInstance.GetComponent<GhostController>();
            if (i == 0)
            {
                ghostController.isChasingPlayer = true;
            }
        }
    }

    void SpawnRewards(int rewardCount)
    {
        for (int i = 0; i < rewardCount; i++)
        {
            // İlgili seviyede belirli ödül prefablarını kullan
            GameObject rewardPrefab = rewardPrefabs[i % rewardPrefabs.Count];

            // Ödül nesnesinin rastgele bir konuma yerleştirilmesi için kod
            Vector2 randomPosition = new Vector2(Random.Range(-12f, 12f), Random.Range(-5f, 5f));

            GameObject rewardInstance = Instantiate(rewardPrefab, randomPosition, Quaternion.identity);
            RewardController rewardController = rewardInstance.GetComponent<RewardController>();
            rewardController.value = i; // ödül değerini ayarla
        }
    }

    public void OnRewardCollected(int value)
    {
        collectedRewards.Add(value);
        collectedRewards.Sort();
        if (collectedRewards.Count == currentLevel)
        {
            LevelUp();
        }

        // Eğer toplanan ödül sayısı mevcut seviyedeki ödül sayısına eşitse seviyeyi yükselt

    }

    void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            collectedRewards.Clear();
            SetupLevel(currentLevel);
        }
        else
        {
            currentLevel = maxLevel;
            GameOver();
        }
    }

    public void GameOver()
    {
        // Oyunu durdur
        Time.timeScale = 0;
        currentLevelText.text = "Level " + currentLevel;
        // "Oyun Bitti" mesajını göster
        gameOverPanel.SetActive(true);
        
    }
}


