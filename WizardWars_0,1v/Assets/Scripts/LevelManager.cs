using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovment cameraMovment;

    [SerializeField]
    private Transform map;
    public Dictionary<Point, TileScript> Tiles { get; set; }

   // public List<GameObject> wayPoints = new List<GameObject>();
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }
    // Update is called once per frame
    void Update()
    {

    }
    //------------�������� ������ � ����������� Tile-��---------
    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero;
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        cameraMovment.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
    }
   
    //----------����� ���������� Tile-��----------------
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
   {
        int tileIndex = int.Parse(tileType);
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);
        if (tileIndex == 7)
        {
            newTile.IsEmpty = false;
        }
        Tiles.Add(new Point(x, y), newTile);      
    }
    //-------------������ ������� ������ �� ���������� �����-------------
    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level1") as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }
}