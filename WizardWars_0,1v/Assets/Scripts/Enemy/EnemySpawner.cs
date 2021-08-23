using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : Singleton<EnemySpawner>
{
    EnemyController enemiesCtl;
    float SpawnDelayHelper = 2.0f;//�������� ����� ������� ��������� ����
    const float spawnDelay = .3f;//�������� ����� ������� ���������� ����� � ����
    float enemyPerStack;//���������� ���������� ��������� ���-�� ������
    float enemyPerStackHelper = 3;//���-�� ������ � ����
    int countOfStacksPerWave = 5;//���-�� ��� � �����
    private List<GameObject> totalEnemyNumber = new List<GameObject>();//����� ��� ����������� ���� �� ���� ��� � ���� ��� ������ ������ ���� ������
    //-------------wave--------------
    [SerializeField]//������� ��� ����������� private ���� � ����������
    private GameObject waveBtn;//���� ��� ������ ������� �����
    [SerializeField]//������� ��� ����������� private ���� � ����������
    private Text waveTxt;//���� � ������� � GUI
    private float waves;//������� �����
    private float wavesHelper;
    private int totalWavesNumber = 5;//���-�� ����
    [SerializeField]//������� ��� ����������� private ���� � ����������
    private GameObject spawnPoint;//����� ������
    public GameObject[] enemyPrefab;//������ �����


    public bool TotalWavesNumber
    {
        get
        {
            return totalEnemyNumber.Count > 0;
        }
    }
     
    
    void Start()
    {
        enemiesCtl = FindObjectOfType<EnemyController>();
        this.waveTxt.text = waves.ToString() + " / " + totalWavesNumber.ToString();
    }

    void Update()
    {

    }
   
    //Wave start by pressing button
    public void StartWave()
    {
        SoundManager.Instance.PlaySFX("WaveStart");//Bell ring in the beggining of wave
        Waves++;//Wave counter
        StartCoroutine(SpawnEnemy(countOfStacksPerWave));//StartCoroutine
        countOfStacksPerWave++;//Increase num of count of stacks per wave
        enemyPerStackHelper++;//Increase num of enemy per stack
        SpawnDelayHelper = 2.0f;//Delay per stack of creeps
        waveBtn.SetActive(false);//Hiding "Next Wave" button
    }
    //���������� �����
    IEnumerator SpawnEnemy(float enemyCount)
    {
        SpawnDelayHelper += 0.1f;
        for (int j = 0; j < enemyCount; j++)
        {
            enemyPerStack = Random.Range(1, enemyPerStackHelper);//Num of enemy in stack   
            for (int i = 0; i < enemyPerStack; i++)
            {
                wavesHelper = waves * 0.5f;
                int enemyType = Random.Range(0, (enemiesCtl.AllEnemies.Count - 1 + (int)wavesHelper));//randomizer of enemy type
                GameObject tmpEnemy = Instantiate(enemyPrefab[enemyType]);//Creating enemy as GameObject
                tmpEnemy.transform.SetParent(gameObject.transform, false);

                tmpEnemy.GetComponent<Enemy>().selfEnemy = new Enemies(enemiesCtl.AllEnemies[enemyType]);
                tmpEnemy.transform.position = spawnPoint.transform.position;
                totalEnemyNumber.Add(tmpEnemy);//Adding enemy in EnemyList for checking waves end
                //Debug.Log(totalEnemyNumber + "total");
                yield return new WaitForSeconds(spawnDelay);//Spawn delay between monters in 1 stack
            }
            yield return new WaitForSeconds(SpawnDelayHelper);
        }
    }
    //--------------������� ������(�����) � ��������� List totalEnemyNumber 
    public void RemoveEnemyFromList(GameObject tmpEnemy)
    {
        totalEnemyNumber.Remove(tmpEnemy);
        if (!TotalWavesNumber)
        {
            waveBtn.SetActive(true);
        }
    }

    public float Waves//�����
    {
        get => waves;
        set
        {
            this.waves = (int)value;
            this.waveTxt.text = waves.ToString() + " / " + totalWavesNumber.ToString();
            if (Waves > totalWavesNumber )
            {
                this.waveTxt.text = totalWavesNumber.ToString() + " / " + totalWavesNumber.ToString();
                GameManager.instance.LevelComplete();
            }
            
        }
    }
}
