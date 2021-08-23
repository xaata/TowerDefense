using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : Singleton<EnemySpawner>
{
    EnemyController enemiesCtl;
    float SpawnDelayHelper = 2.0f;//Задержка перед спавном следующей кучи
    const float spawnDelay = .3f;//Задержка перед спавном следующего крипа в куче
    float enemyPerStack;//Переменная содержащая случайное кол-во врагов
    float enemyPerStackHelper = 3;//кол-во крипов в куче
    int countOfStacksPerWave = 5;//Кол-во куч в волне
    private List<GameObject> totalEnemyNumber = new List<GameObject>();//нужен для определения есть ли враг еще в игре для вывода кнопки след уровня
    //-------------wave--------------
    [SerializeField]//атрибут для отображения private поля в инспекторе
    private GameObject waveBtn;//Поле для кнопки старата волны
    [SerializeField]//атрибут для отображения private поля в инспекторе
    private Text waveTxt;//Поле с текстом в GUI
    private float waves;//счетчик волны
    private float wavesHelper;
    private int totalWavesNumber = 5;//кол-во волн
    [SerializeField]//атрибут для отображения private поля в инспекторе
    private GameObject spawnPoint;//Точка спавна
    public GameObject[] enemyPrefab;//Префаб врага


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
    //Реализация волны
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
    //--------------Удаляет объект(врага) с коллекции List totalEnemyNumber 
    public void RemoveEnemyFromList(GameObject tmpEnemy)
    {
        totalEnemyNumber.Remove(tmpEnemy);
        if (!TotalWavesNumber)
        {
            waveBtn.SetActive(true);
        }
    }

    public float Waves//волны
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
