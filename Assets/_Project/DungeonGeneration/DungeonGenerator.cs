using System.Collections;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject loadingScreenPanel;
    public Transform playerTransform;
    public float characterSize;

    [Header("Object Generation")]
    public GameObject enemyHordePrefab;
    public GameObject foodPrefab;
    public GameObject healthPrefab;
    public int foodMin, foodMax, healthMin, healthMax;


    [Header("Environment Generation")]
    public Transform roomContainer;
    public int dungeonBoundsLayer;
    public int doorsLayer;
    public int ppu = 2;
    public int tileSize = 16;
    public Sprite rightCornerTop, rightCornerBottom, leftCornerTop, leftCornerBottom;
    public Sprite[] bottomWalls, topWalls, rightWalls, leftWalls;
    public Sprite[] doors;
    public Sprite[] floorTiles;
    public Sprite rightDoor, leftDoor, topDoor, bottomDoor;
    public Sprite rightDoorClosed, leftDoorClosed, topDoorClosed, bottomDoorClosed;
    public int minHeight, maxHeight, minWidth, maxWidth;

    int roomX, roomY;
    int increment;
    Vector2[] spawnPositions;
    float _playerWidth;
    int spawnIndex;
    int hordesToSpawn;

    public static DungeonGenerator instance;

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

    private void Start()
    {
        spawnIndex = Random.Range(0, 4);
        NextLevel(spawnIndex);
    }

    public void NextLevel(int oldSpawnIndex)
    {
        // 0 is right, 1 is left, 2 is top, 3 is bottom
        switch (oldSpawnIndex)
        {
            case 0:
                spawnIndex = 1;
                break;
            case 1:
                spawnIndex = 0;
                break;
            case 2:
                spawnIndex = 3;
                break;
            case 3:
                spawnIndex = 2;
                break;

        }

        loadingScreenPanel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(StartGeneration());
    }

    IEnumerator StartGeneration()
    {
        //start generation after a delay of 1 second to prevent freezing on Start
        yield return new WaitForSeconds(1);

        roomX = Random.Range(minWidth, maxWidth);
        roomY = Random.Range(minHeight, maxHeight);
        hordesToSpawn = roomX * roomY / 100;
        _playerWidth = (GameManager.instance.humansAliveCount + GameManager.instance.data.chainSpacing) * 5;

        ClearRoom();
        GenerateDungeon();
        GenerateObjects();
        PlacePlayer();
        Tail.instance.SetupChainMembers();
        GameManager.instance.StartInvincibility();
        loadingScreenPanel.SetActive(false);
    }

    void ClearRoom()
    {
        int _len = roomContainer.childCount;
        if (_len == 0)
        {
            return;
        }

        for (int i = 0; i < _len; i++)
        {
            Destroy(roomContainer.GetChild(i).gameObject);
        }
    }

    private void GenerateDungeon()
    {  
        //setting up the increment value and room bounds
        increment = tileSize / ppu;
        GameManager.instance.data.roomHeight = (roomY - 1) * increment;
        GameManager.instance.data.roomWidth = (roomX - 1) * increment;

        GenerateFloor();
        GenerateWalls();
        GenerateDoors();

    }

    void GenerateFloor()
    {
        var floorObj = new GameObject
        {
            name = "Floor"
        };
        Transform floorT = floorObj.transform;
        floorT.parent = roomContainer;

        //generate floor tiles
        int length = floorTiles.Length;
        GameObject floorTile;
        SpriteRenderer spriteR;
        Vector2 currentPos = Vector2.zero;
        for (int i = 0; i < roomY; i++)
        {
            for (int j = 0; j < roomX; j++)
            {
                floorTile = new GameObject();
                spriteR = floorTile.AddComponent<SpriteRenderer>();
                spriteR.sprite = floorTiles[Random.Range(0, length)];
                floorTile.transform.position = currentPos;
                floorTile.transform.parent = floorT;
                currentPos.x += increment;
            }
            currentPos.x = 0;
            currentPos.y += increment;
        }

    }

    void GenerateWalls()
    {
        var wallObj = new GameObject
        {
            layer = dungeonBoundsLayer,
            name = "Walls"
        };
        Transform wallsT = wallObj.transform;
        wallsT.parent = roomContainer;

        //adding four wall colliders
        for (int i = 0; i < 4; i++)
        {
            var _box = wallObj.AddComponent<BoxCollider2D>();
            var _boxSize = new Vector2();
            var _boxOffset = new Vector2();

            //adjusting size and offsets
            if (i == 0 || i == 1) //bottom and top walls
            {
                _boxSize.x = (roomX + 2) * increment;
                _boxSize.y = tileSize;
                _boxOffset.x = roomX * increment / 2 - increment / 2;

                if (i == 0) //bottom wall
                {
                    _boxOffset.y = -increment - tileSize / 4;
                }
                else
                {
                    _boxOffset.y = roomY * increment + tileSize / 4;
                }
            }
            else //right and left walls
            {
                _boxSize.x = tileSize;
                _boxSize.y = (roomY + 2) * increment;
                _boxOffset.y = roomY * increment / 2 - increment / 2;

                if (i == 3) //right wall
                {
                    _boxOffset.x = roomX * increment + tileSize / 4;
                }
                else
                {
                    _boxOffset.x = -increment - tileSize / 4;
                }
            }

            _box.size = _boxSize;
            _box.offset = _boxOffset;

        }

        //adding tiles
        GameObject wallTile;
        SpriteRenderer spriteR;

        //bottom and top walls
        //start with bottom wall
        int length = bottomWalls.Length;
        Sprite[] spriteArray = bottomWalls;
        Sprite leftCorner = leftCornerBottom, rightCorner = rightCornerBottom;
        Vector2 currentPos = new Vector2(-increment, -increment);
        for (int i = 0; i < 2; i++)
        {
            //from left corner to right corner
            for (int j = -1; j <= roomX; j++)
            {
                wallTile = new GameObject();
                spriteR = wallTile.AddComponent<SpriteRenderer>();
                if (j == -1)
                {
                    spriteR.sprite = leftCorner;
                }
                else if (j == roomX)
                {
                    spriteR.sprite = rightCorner;
                }
                else
                {
                    spriteR.sprite = spriteArray[Random.Range(0, length)];
                }

                wallTile.transform.position = currentPos;
                wallTile.transform.parent = wallsT;
                currentPos.x += increment;
            }
            length = topWalls.Length;
            spriteArray = topWalls;
            leftCorner = leftCornerTop;
            rightCorner = rightCornerTop;
            currentPos = new Vector2(-increment, roomY * increment);

        }

        //right and left walls
        //start with right wall
        length = rightWalls.Length;
        spriteArray = rightWalls;
        currentPos = new Vector2(roomX * increment, 0);
        for (int i = 0; i < 2; i++)
        {
            //from bottom to top, skipping corners
            for (int j = 0; j < roomY; j++)
            {
                wallTile = new GameObject();
                spriteR = wallTile.AddComponent<SpriteRenderer>();
                spriteR.sprite = spriteArray[Random.Range(0, length)];
                wallTile.transform.position = currentPos;
                wallTile.transform.parent = wallsT;
                currentPos.y += increment;
            }
            length = leftWalls.Length;
            spriteArray = leftWalls;
            currentPos = new Vector2(-increment, 0);

        }


    }

    void GenerateDoors()
    {
        spawnPositions = new Vector2[4];
        var doorsObj = new GameObject
        {
            name = "Doors"
        };
        Transform doorsT = doorsObj.transform;
        doorsT.parent = roomContainer;

        //generate doors
        int length = floorTiles.Length;
        GameObject door;
        SpriteRenderer spriteR;
        var doorSafePadding = _playerWidth;
        Vector2 _currentPos = new Vector2();
        Vector2 _spawnPos = new Vector2();
        Sprite _openS, _closedS;
        _openS = _closedS = rightDoor;
        for (int i = 0; i < 4; i++)
        {
           
            switch (i)
            {
                //rightDoor
                case 0:
                    _currentPos = new Vector2(roomX * increment, Random.Range(0 + doorSafePadding, (roomY - 1) * increment - doorSafePadding));
                    _spawnPos = _currentPos - new Vector2(increment, 0);
                    _openS = rightDoor;
                    _closedS = rightDoorClosed;
                    break;
                //left door
                case 1:
                    _currentPos = new Vector2(-increment, Random.Range(0 + doorSafePadding, (roomY - 1) * increment - doorSafePadding));
                    _spawnPos = _currentPos + new Vector2(_playerWidth - increment, 0);
                    _openS = leftDoor;
                    _closedS = leftDoorClosed;
                    break;
                //top door
                case 2:
                    _currentPos = new Vector2(Random.Range(0 + doorSafePadding, (roomX - 1) * increment - doorSafePadding), roomY * increment);
                    _spawnPos = _currentPos - new Vector2(0, increment);
                    _openS = topDoor;
                    _closedS = topDoorClosed;
                    break;
                //bottom door
                case 3:
                    _currentPos = new Vector2(Random.Range(0 + doorSafePadding, (roomX - 1) * increment - doorSafePadding), -increment);
                    _spawnPos = _currentPos + new Vector2(0, increment);
                    _openS = bottomDoor;
                    _closedS = bottomDoorClosed;
                    break;
                default:
                    break;
            }

            door = new GameObject();
            spriteR = door.AddComponent<SpriteRenderer>();
            spriteR.sortingOrder = 1;
            door.transform.position = _currentPos;
            door.transform.parent = doorsT;
            var _doorComp = door.AddComponent<Door>();
            door.layer = doorsLayer;
            spawnPositions[i] = _spawnPos;
            _doorComp.linkedSpawnIndex = i;
            //open or close
            if (spawnIndex == i)
            {
                spriteR.sprite = _closedS;
            }
            else
            {
                door.AddComponent<CircleCollider2D>();
                spriteR.sprite = _openS;
            }
        }

    }

    void GenerateObjects()
    {
        int foodToSpawn = Random.Range(foodMin, foodMax);
        int healthToSpawn = Random.Range(healthMin, healthMax);

        Vector2[] usedPositions = new Vector2[foodToSpawn + healthToSpawn + hordesToSpawn];
        int roomSizeX = roomX * increment;
        int currentIndex = 0;
        int currentY = 2 * increment;

        //setting min pos values for placement of objects
        int _minX, _minY, _maxX, _maxYIncrement;
        _minX = _minY = 2 * increment;
        _maxX = roomSizeX - 1 - tileSize;
        _maxYIncrement = 4 * increment;


        //spawn food, health and enemies
        int _len = 0;
        GameObject _prefabToUse = foodPrefab;

        for (int ob = 0; ob < 3; ob++)
        {
            switch (ob)
            {
                case 0: _len = foodToSpawn; _prefabToUse = foodPrefab; break;
                case 1: _len = healthToSpawn; _prefabToUse = healthPrefab; break;
                case 2: _len = hordesToSpawn; _prefabToUse = enemyHordePrefab; break;
            }

            for (int i = 0; i < _len;)
            {
                var pos = new Vector2(Random.Range(_minX, _maxX), currentY);
                //if there is no object at pos, spawn the object here
                if (System.Array.IndexOf(usedPositions, pos) == -1)
                {
                    Instantiate(_prefabToUse, pos, Quaternion.identity, roomContainer);
                    usedPositions[currentIndex++] = pos;

                    currentY += Random.Range(_minY, _maxYIncrement);
                    if (currentY > (roomY - 2) * increment)
                    {
                        currentY = _minY;
                    }
                    //update counter only if we spawned
                    i++;
                }

            }

            currentY = _minY;

        }

    }

    void PlacePlayer()
    {
        playerTransform.position = spawnPositions[spawnIndex];
    }

}
