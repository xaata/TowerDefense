using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }
    
    private SpriteRenderer spriteRenderer;

    private Tower myTower;
    public bool IsEmpty { get; set; }
    private Color32 fullColor = new Color32(255, 118, 118, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        
    }
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
    }
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedTowerBtn != null)
        {
            if (IsEmpty)
            {
                ColorTile(emptyColor);
            }
            if (!IsEmpty)
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
        else if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedTowerBtn == null && Input.GetMouseButtonDown(0)) 
        {
            if (myTower != null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
    }
    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }
    private void PlaceTower()
    {
        GameObject tower = Instantiate(GameManager.Instance.ClickedTowerBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        tower.transform.SetParent(transform);
        this.myTower = tower.GetComponent<Tower>();
        IsEmpty = false;
        ColorTile(Color.white);
        myTower.Price = GameManager.Instance.ClickedTowerBtn.Price;
        GameManager.Instance.BuyTower();
    }
    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}