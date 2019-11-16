using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSeason : MonoBehaviour
{
    public Camera Camera;
    public string CurrentSeason;
    public float ChangeSpeed;
    GameManager gameManager;

    private void Start()
    {
        if (Camera == null)
            Camera = Camera.main;

        Camera.cullingMask = ChangeMask(Camera.cullingMask, CurrentSeason, true);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeSeason("Spring");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeSeason("Winter");
        }
    }

    public void ChangeSeason(string season)
    {
        if (Camera == null) Camera = gameManager.Camera;
        StartCoroutine(CorChangeSeason(season));
    }

    public IEnumerator CorChangeSeason(string season)
    {
        GameManager.Instance.TransitionsAnimator.SetBool("round", true);
        yield return new WaitForSeconds(ChangeSpeed);
        Camera.cullingMask = ChangeMask(Camera.cullingMask, CurrentSeason, false);
        Camera.cullingMask = ChangeMask(Camera.cullingMask, season, true);
        CurrentSeason = season;
        GameManager.Instance.TransitionsAnimator.SetBool("round", false);
    }

    // Behold traveller, don't go further, it's dangerous in this function
    public LayerMask ChangeMask(LayerMask mask, string layerName, bool showing)
    {
        int index = LayerMask.NameToLayer(layerName);
        string initial = LayerMask.LayerToName(index);
        if (showing)
            return mask | (1 << index);
        else
            return mask & ~(1 << index);
    }
}
