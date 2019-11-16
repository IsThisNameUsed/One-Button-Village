using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RessourcesManager : MonoBehaviour
{
    // Entry point for initial resources
    [SerializeField]
    private string _resource1Name;
    [SerializeField]
    public int _resource1Quantity;

    [SerializeField]
    public string _resource2Name;
    [SerializeField]
    public int _resource2Quantity;

    [SerializeField]
    private string _resource3Name;
    [SerializeField]
    public int _resource3Quantity;

    [SerializeField]
    private string _resource4Name;
    [SerializeField]
    public int _resource4Quantity;
    
    public List<Resource> Resources;
    
    public TimeLine.seasons currentSeason;

    [Header("Resource quantity lost by action")]
    public int Consommation = 1;
    [Header("Resource quantity lost by action in tiers2")]
    public int Tiers2Consommation = 2;
    [Header("Resource quantity lost by action in tiers3")]
    public int Tiers3Consommation = 3;
    [Header("Resource quantity lost by action in tiers4")]
    public int Tiers4Consommation = 4;


    [Header(" Actual base resource quantity granted by action")]
    public float Efficiency = 2f;
    [Header("Base resource quantity granted by action in tiers2")]
    public float Tiers2Efficiency = 4f;
    [Header("Base resource quantity granted by action in tiers3")]
    public float Tiers3Efficiency = 6f;
    [Header("Base resource quantity granted by action in tiers4")]
    public float Tiers4Efficiency = 8f;

    [Header("Resource lost passively")]
    public int actualDecay = 3;
    public int BaseDecay = 3;
    public float DecayInterval = 2f;

    [Header("Max overdrive divisor")]
    public float MaxOverDrive = 1.8f;
    [Header("Overdrive divisor increase by action")]
    public float BaseOverDriveIncrease = 0.1f;
    
    [Header("Base overdrive divisor")]
    public float BaseOverdrive = 1f;
    [Header("Overdrive decay per second")]
    public float OverdriveDecay = 0.5f;

    [Header("/!\\ Debug Read Only /!\\")]
    public float CurrentOverDrive = 4.8f;
    public float ActualOverDriveIncrease = 0.1f;

    internal bool InGame;
    internal int CurrentLimit;

    Data data;
    // Use this for initialization
    void Start ()
    {
        Resources = new List<Resource>();
        Resources.Add(new Resource()
        {
            Name = _resource1Name,
            Quantity = _resource1Quantity
        });
        Resources.Add(new Resource()
        {
            Name = _resource1Name,
            Quantity = _resource2Quantity
        });
        Resources.Add(new Resource()
        {
            Name = _resource1Name,
            Quantity = _resource3Quantity
        });
        Resources.Add(new Resource()
        {
            Name = _resource1Name,
            Quantity = _resource4Quantity
        });
        data = GameObject.Find("GameManager").GetComponent<Data>();
    }

    private void Update()
    {

        if (!InGame)
            return;

        if (CurrentOverDrive > BaseOverdrive)
        {
            CurrentOverDrive -= OverdriveDecay * Time.deltaTime;
            if (CurrentOverDrive < BaseOverdrive)
                CurrentOverDrive = BaseOverdrive;

            float invertedPourcentage = ((CurrentOverDrive - BaseOverdrive) / (MaxOverDrive - BaseOverdrive));
            GameManager.Instance.OverDriveDisplay.SetGauge(1 - invertedPourcentage);
        }
    }

    public bool Action()
    {
        Resource plusResource;
        Resource minusResource;

        switch (currentSeason)
        {
            case TimeLine.seasons.Spring:
                plusResource = Resources[2];
                minusResource = Resources[0];
                break;
            case TimeLine.seasons.Summer:
                plusResource = Resources[0];
                minusResource = Resources[3];
                break;
            case TimeLine.seasons.Autumn:
                plusResource = Resources[1];
                minusResource = Resources[2];
                break;
            case TimeLine.seasons.Winter:
                plusResource = Resources[3];
                minusResource = Resources[1];
                break;
            default:
                return false;
        }

        if (minusResource.Quantity > Consommation)
        {
            minusResource.Quantity -= Consommation;

            plusResource.Quantity += Mathf.CeilToInt(Efficiency / CurrentOverDrive);

            if (plusResource.Quantity > CurrentLimit)
                plusResource.Quantity = CurrentLimit;
        }
        else
        {
            return false;
        }

        if (CurrentOverDrive != MaxOverDrive)
        {
            CurrentOverDrive += ActualOverDriveIncrease;
            if (CurrentOverDrive > MaxOverDrive)
                CurrentOverDrive = MaxOverDrive;

            float invertedPourcentage = ((CurrentOverDrive - BaseOverdrive) / (MaxOverDrive - BaseOverdrive));
            GameManager.Instance.OverDriveDisplay.SetGauge(1 - invertedPourcentage);
        }
        /*if (CurrentOverDrive != MaxOverDrive)
        {
            CurrentOverDrive += OverDriveIncrease;
            if (CurrentOverDrive > MaxOverDrive)
                CurrentOverDrive = MaxOverDrive;

            float invertedPourcentage = ((CurrentOverDrive - BaseOverdrive) / (MaxOverDrive - BaseOverdrive));
            GameManager.Instance.OverDriveDisplay.SetGauge(1 - invertedPourcentage);
        }*/

        return true;
    }

    public void StartYear()
    {
        InGame = true;
        ChangeSeason(TimeLine.seasons.Spring);
        CurrentOverDrive = BaseOverdrive;
        GameManager.Instance.OverDriveDisplay.SetGauge(1);
    }

    public void ChangeSeason(TimeLine.seasons season)
    {
        currentSeason = season;

        int plus;
        int minus;
        switch(season)
        {
            case TimeLine.seasons.Spring:
                plus = 2;
                minus = 0;
                SoundManager.Instance.PlaySound("spring", 0.8f, true);
                break;
            case TimeLine.seasons.Summer:
                plus = 0;
                minus = 3;
                SoundManager.Instance.PlaySound("summer", 0.7f, true);
                break;
            case TimeLine.seasons.Autumn:
                plus = 1;
                minus = 2;
                SoundManager.Instance.PlaySound("autumn", 0.8f, true);
                break;
            case TimeLine.seasons.Winter:
                plus = 3;
                minus = 1;
                SoundManager.Instance.PlaySound("winter", 0.8f, true);
                break;
            default:
                return; 
        }
        GameManager.Instance.ResourceDisplay.SetSeason(minus, plus);
    }

    public void PassiveConsommation()
    {
        Resource resourceToDecay;
        switch(currentSeason)
        {
            case TimeLine.seasons.Spring:
                resourceToDecay = Resources[0];
                break;
            case TimeLine.seasons.Summer:
                resourceToDecay = Resources[3];
                break;
            case TimeLine.seasons.Autumn:
                resourceToDecay = Resources[2];
                break;
            case TimeLine.seasons.Winter:
                resourceToDecay = Resources[1];
                break;
            default:
                return;
        }

        resourceToDecay.Quantity -= actualDecay;
        if (resourceToDecay.Quantity < 0)
            resourceToDecay.Quantity = 0;
    }
}
