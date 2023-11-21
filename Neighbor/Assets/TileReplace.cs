using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileReplace : MonoBehaviour
{
    public List<TileBase> startTiles;
    public List<TileBase> altTiles;

    public List<TileBase> startColliderTiles;
    public List<TileBase> altColliderTiles;

    private List<Vector2Int> tilePositions;
    public Vector2Int initialTilePosition;
    public Vector2Int tileSize;

    private List<Vector2Int> colliderPositions;
    public Vector2Int initialColliderPosition;
    public Vector2Int colliderSize;

    public Tilemap tileMap;
    public Tilemap colliderMap;

    [System.NonSerialized] public bool alt = false;
    public bool swapColliders;

    // Start is called before the first frame update
    void Start()
    {
        tilePositions = CalculateTilePositions(initialTilePosition, tileSize);
        if (swapColliders)
        {
            colliderPositions = CalculateColliderPositions(initialColliderPosition, colliderSize);
        }
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            swapAlt();
        }
        */
    }

    // Update is called once per frame
    public void swapAlt()
    {
        if (!alt)
        {
            switchTiles(altTiles, altColliderTiles);
        }
        else
        {
            switchTiles(startTiles, startColliderTiles);
        }

        alt = !alt;
    }

    void switchTiles(List<TileBase> tileReplace, List<TileBase> colliderReplace)
    {
        for (int i = 0; i < tilePositions.Count; i++)
        {
            Vector3Int tileLocation = new Vector3Int(tilePositions[i][0], tilePositions[i][1], 0);
            tileMap.SetTile(tileLocation, tileReplace[i]);
        }
        if (swapColliders)
        {
            for (int i = 0; i < colliderPositions.Count; i++)
            {
                Vector3Int colliderLocation = new Vector3Int(colliderPositions[i][0], colliderPositions[i][1], 0);
                colliderMap.SetTile(colliderLocation, colliderReplace[i]);
            }
        }
    }

    private List<Vector2Int> CalculateTilePositions(Vector2Int initialPosition, Vector2Int size)
    {
        List<Vector2Int> returnList = new List<Vector2Int>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                returnList.Add(new Vector2Int(initialPosition.x - x, initialPosition.y - y));
                //Debug.Log(tileMap.GetTile(new Vector3Int(initialPosition.x - x, initialPosition.y - y, 0)));
                startTiles.Add(tileMap.GetTile(new Vector3Int(initialPosition.x - x, initialPosition.y - y, 0)));
            }
        }

        return returnList;
    }

    private List<Vector2Int> CalculateColliderPositions(Vector2Int initialPosition, Vector2Int size)
    {
        List<Vector2Int> returnList = new List<Vector2Int>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                returnList.Add(new Vector2Int(initialPosition.x - x, initialPosition.y - y));
                //Debug.Log(colliderMap.GetTile(new Vector3Int(initialPosition.x - x, initialPosition.y - y, 0)));
                startColliderTiles.Add(colliderMap.GetTile(new Vector3Int(initialPosition.x - x, initialPosition.y - y, 0)));
            }
        }

        return returnList;
    }
}
