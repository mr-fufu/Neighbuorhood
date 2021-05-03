using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public List<AudioClip> hardwoodFootsteps;
    public List<AudioClip> carpetFootsteps;
    public List<AudioClip> grassFootsteps;
    public List<AudioClip> cementFootsteps;
    public List<AudioClip> tileFootsteps;

    public GameController game;

    private List<List<AudioClip>> footsteps;
    private List<int> footstepIndex = new List<int>();

    private PolygonCollider2D check;
    private ContactFilter2D filter;
    private Collider2D[] results = new Collider2D[30];

    private float footstepTimer = 0;

    private void Start()
    {
        check = GetComponent<PolygonCollider2D>();
        filter = new ContactFilter2D();
        filter.NoFilter();
        filter.useTriggers = true;

        footsteps = new List<List<AudioClip>>();

        footsteps.Add(hardwoodFootsteps);
        footsteps.Add(carpetFootsteps);
        footsteps.Add(grassFootsteps);
        footsteps.Add(cementFootsteps);
        footsteps.Add(tileFootsteps);

        for (int i = 0 ; i < footsteps.Count ; i++)
        {
            footstepIndex.Add(0);
        }
    }

    private void Update()
    {
        if (footstepTimer > 0)
        {
            footstepTimer -= Time.deltaTime;
        }
        else if (footstepTimer < 0)
        {
            footstepTimer = 0;
        }
    }

    public void PlayFootstep()
    {
        if (footstepTimer == 0)
        {
            int surfaceType = GetSurfaceType();

            List<AudioClip> clipToPlay = new List<AudioClip>();
            clipToPlay.Add(footsteps[surfaceType][footstepIndex[surfaceType]]);
            game.audioControl.PlayMultipleClips(clipToPlay);

            footstepIndex[surfaceType] += 1;

            if (footstepIndex[surfaceType] >= footsteps[surfaceType].Count)
            {
                footstepIndex[surfaceType] = 0;
            }

            footstepTimer = 0.35f;
        }
    }
    int GetSurfaceType()
    {
        results = new Collider2D[30];
        check.OverlapCollider(filter, results);
        SurfaceType checkSurface = SurfaceType.EXTERIOR;
        bool found = false;

        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] != null)
            {
                if (results[i].gameObject.tag == "surface")
                {
                    SurfaceScript foundSurface = results[i].gameObject.GetComponent<SurfaceScript>();

                    if (foundSurface != null)
                    {
                        if (foundSurface.enabled)
                        {
                            checkSurface = foundSurface.surface;
                            found = true;
                            break;
                        }
                    }
                }
            }
        }

        if (!found)
        {
            checkSurface = game.roomType;
        }

        switch (checkSurface)
        {
            case SurfaceType.EXTERIOR:
                return 2;
            case SurfaceType.HARDWOOD:
                return 0;
            case SurfaceType.FLOORBOARD:
                return 0;
            case SurfaceType.TILE:
                return 4;
            case SurfaceType.CEMENT:
                return 3;
            case SurfaceType.CARPET:
                return 1;
            default:
                return 0;
        }

    }
}
