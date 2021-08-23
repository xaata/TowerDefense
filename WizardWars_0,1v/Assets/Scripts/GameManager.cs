using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    //-------PlayerLives----------
    [SerializeField]
    private Text livesTxt;
    private int lives;
    //----------Menu----------
    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]
    private GameObject OptionsMenu;
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject LevelCompleteMenu;
    [SerializeField]
    private GameObject GameOverMenu;
    [SerializeField]
    private GameObject TowerPanel;

    public TowerBtn ClickedTowerBtn { get;  set; }
    private Tower selectedTower;
    public  ObjectPool Pool { get; set; }
   
    //---------Money--------
    private int currency;
    [SerializeField]
    private Text currencyTxt;

    [SerializeField]
    private Text sellText;

    //------------GameEnder----------

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        Lives = 100;
        Currency = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();        
    }
    public int Currency
    {
        get => currency;
        set
        {
            this.currency = value;
            this.currencyTxt.text = currency.ToString() + " <color=lime>$</color>";
        }
    }

    public float Lives
    {
        get => lives;
        set
        {
            this.lives = (int)value;
            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            } 
            this.livesTxt.text = lives.ToString();
        }
    }

    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price )
        {
            this.ClickedTowerBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
        
    }
    public void BuyTower()
    {
        if (Currency >= ClickedTowerBtn.Price)
        {
            Currency -= ClickedTowerBtn.Price;
            Hover.Instance.Deactivate();
        } 
    }

    public void SelectTower(Tower tower)
    {
        //if (selectedTower != null)
        //{
            
        //}
        selectedTower = tower;

        sellText.text = "+" + (selectedTower.Price / 2).ToString();
        TowerPanel.SetActive(true);

    }
    public void DeselectTower()
    {
        selectedTower = null;
        TowerPanel.SetActive(false);
    }
    public void SellTower()
    {
        if (selectedTower != null)
        {
            Currency += selectedTower.Price / 2;
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;
            Destroy(selectedTower.transform.gameObject);
            DeselectTower();
        }
    }
    public void HandleEscape()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Hover.Instance.Deactivate();
            if (Hover.Instance.IsVisible)
            {
                DropTower();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
            if (Hover.Instance.IsVisible)
            {
                DropTower();
            }
            ShowPauseMenu();
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            GameOverMenu.SetActive(true);
        }

    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
        //-------Баг при рестарте не ставится тавер(фикс есть где то в гайде)
        //Hover.Instance.Deactivate();
        //DropTower();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    public void LevelComplete()
    {
        Time.timeScale = 0;
        LevelCompleteMenu.SetActive(true);
    }
    public void ShowPauseMenu()
    {
        //if(LevelCompleteMenu.activeSelf || GameOverMenu.activeSelf)
        if (OptionsMenu.activeSelf)
        {
            OptionsMenu.SetActive(false);
            PauseMenu.SetActive(true);
        }
        else
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            if (!PauseMenu.activeSelf)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
    private void DropTower()
    {
        ClickedTowerBtn = null;
        Hover.Instance.Deactivate();
    }
    public void ShowOptions()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    } 
}