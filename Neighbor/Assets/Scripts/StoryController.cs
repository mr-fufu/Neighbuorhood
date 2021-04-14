using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StoryController : MonoBehaviour
{
    public Tilemap objectsMap;

    public List<Tile> cellarTiles;
    public List<Tile> carpetTiles;

    public Vector2Int cellarInitialPosition;
    public Vector2Int cellarSize;

    private List<Vector2Int> cellarPositions;

    public Vector2Int carpetInitialPosition;
    public Vector2Int carpetSize;

    private List<Vector2Int> carpetPositions;

    // Start is called before the first frame update
    void Start()
    {
        cellarPositions = CalculateTilePositions(cellarInitialPosition, cellarSize);
        carpetPositions = CalculateTilePositions(carpetInitialPosition, carpetSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void storyEvent(string eventName)
    {
        if (eventName != null)
        {
            switch (eventName)
            {
                case "CellarDoorOpen":
                    for (int i = 0; i < cellarPositions.Count; i++)
                    {
                        Vector3Int cellarPosition = new Vector3Int(cellarPositions[i][0], cellarPositions[i][1], 0);
                        objectsMap.SetTile(cellarPosition, cellarTiles[i]);
                    }
                    break;
                case "LiftRug":
                    for (int i = 0; i < carpetPositions.Count; i++)
                    {
                        Vector3Int carpetPosition = new Vector3Int(carpetPositions[i][0], carpetPositions[i][1], 0);
                        objectsMap.SetTile(carpetPosition, carpetTiles[i]);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private List<Vector2Int> CalculateTilePositions(Vector2Int initialPosition, Vector2Int size)
    {
        List<Vector2Int> returnList = new List<Vector2Int>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y<size.y; y++)
            {
                returnList.Add(new Vector2Int(initialPosition.x - x, initialPosition.y - y));
            }
        }

        Debug.Log(returnList);

        return returnList;
    } 
}
