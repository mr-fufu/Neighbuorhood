using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StoryController : MonoBehaviour
{
    public GameController game;

    public Tilemap overlayRaised2Map;
    public Tilemap colliderWallMap;
    public Tilemap objectsMap;
    public Tilemap objectsAboveMap;
    public Tilemap floorRaisedMap;

    public List<Tile> cellarTiles;
    public List<Tile> carpetTiles;
    public List<Tile> gateOpenTiles;
    public List<Tile> gateClosedTiles;
    public Tile mailboxTile;

    public List<Tile> gateColliderOpenTiles;
    public List<Tile> gateColliderClosedTiles;

    public Vector2Int cellarInitialPosition;
    public Vector2Int cellarSize;
    private List<Vector2Int> cellarPositions;

    public Vector2Int carpetInitialPosition;
    public Vector2Int carpetSize;
    private List<Vector2Int> carpetPositions;

    public Vector2Int gateInitialPosition;
    public Vector2Int gateSize;
    private List<Vector2Int> gatePositions;

    public Vector2Int gateColliderInitialPosition;
    public Vector2Int gateColliderSize;
    private List<Vector2Int> gateColliderPositions;

    public Vector2Int mailboxPosition;

    public GameObject etchedKey;
    public GameObject hookLight;
    public GameObject cellarDoor;
    public GameObject unnaturalDark;

    public GameObject darkInteracts;
    public GameObject lightInteracts;

    public SpriteRenderer mail;

    public List<Sprite> testSlides;
    public List<float> testSlideTimings;

    // Start is called before the first frame update
    void Start()
    {
        cellarPositions = CalculateTilePositions(cellarInitialPosition, cellarSize);
        carpetPositions = CalculateTilePositions(carpetInitialPosition, carpetSize);
        gatePositions = CalculateTilePositions(gateInitialPosition, gateSize);
        gateColliderPositions = CalculateTilePositions(gateColliderInitialPosition, gateColliderSize);

        etchedKey.SetActive(false);
        cellarDoor.SetActive(false);

        hookLight.SetActive(false);
        unnaturalDark.SetActive(true);
        darkInteracts.SetActive(true);
        lightInteracts.SetActive(false);

        mail.enabled = false;
    }

    public void storyEvent(string eventName, Interactable interact, string interactItem)
    {
        if (eventName != null)
        {
            switch (eventName)
            {
                case "CellarDoorOpen":

                    for (int i = 0; i < cellarPositions.Count; i++)
                    {
                        Vector3Int cellarPosition = new Vector3Int(cellarPositions[i][0], cellarPositions[i][1], 0);
                        objectsAboveMap.SetTile(cellarPosition, cellarTiles[i]);
                    }

                    interact.enabled = false;

                    cellarDoor.SetActive(true);

                    break;

                case "LiftRug":

                    for (int i = 0; i < carpetPositions.Count; i++)
                    {
                        Vector3Int carpetPosition = new Vector3Int(carpetPositions[i][0], carpetPositions[i][1], 0);
                        floorRaisedMap.SetTile(carpetPosition, carpetTiles[i]);
                    }

                    etchedKey.SetActive(true);

                    interact.enabled = false;

                    break;

                case "OpenGate":

                    for (int i = 0; i < gatePositions.Count; i++)
                    {
                        Vector3Int gatePosition = new Vector3Int(gatePositions[i][0], gatePositions[i][1], 0);
                        overlayRaised2Map.SetTile(gatePosition, gateOpenTiles[i]);
                    }

                    for (int i = 0; i < gateColliderPositions.Count; i++)
                    {
                        Vector3Int gateColliderPosition = new Vector3Int(gateColliderPositions[i][0], gateColliderPositions[i][1], 0);
                        colliderWallMap.SetTile(gateColliderPosition, gateColliderOpenTiles[i]);
                    }

                    interact.enabled = false;
                    interact.interactText = null;
                    interact.interact = false;

                    //interact.interactEvent = "CloseGate";

                    break;

                case "CloseGate":

                    for (int i = 0; i < gatePositions.Count; i++)
                    {
                        Vector3Int gatePosition = new Vector3Int(gatePositions[i][0], gatePositions[i][1], 0);
                        overlayRaised2Map.SetTile(gatePosition, gateClosedTiles[i]);
                    }

                    for (int i = 0; i < gateColliderPositions.Count; i++)
                    {
                        Vector3Int gateColliderPosition = new Vector3Int(gateColliderPositions[i][0], gateColliderPositions[i][1], 0);
                        colliderWallMap.SetTile(gateColliderPosition, gateColliderClosedTiles[i]);
                    }

                    //interact.interactEvent = "OpenGate";

                    break;

                case "LightSwitch":

                    unnaturalDark.SetActive(hookLight.activeSelf ? true : false);

                    if (interact.interactText != null)
                    {
                        darkInteracts.SetActive(hookLight.activeSelf ? true : false);
                        lightInteracts.SetActive(hookLight.activeSelf ? false : true);

                        interact.interactText = null;
                    }

                    hookLight.SetActive(hookLight.activeSelf ? false : true);

                    interact.interact_name = "Lightswitch";
                    interact.unknown = false;

                    break;

                case "CheckMail":

                    objectsAboveMap.SetTile(new Vector3Int (mailboxPosition.x, mailboxPosition.y, 0), mailboxTile);
                    mail.enabled = true;
                    interact.pickup = true;
                    interact.interact = false;
                    interact.interact_name = "Mail";

                    break;

                case "TestSlides":

                    game.slideControl.ShowSlides(testSlides, testSlideTimings);

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

        return returnList;
    } 
}
