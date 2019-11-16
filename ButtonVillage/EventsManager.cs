using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour {
    public Data data;
    public GameManager gameManager;
    public int MalusRessources= 10;
    public int BonusRessources= 10;
    public int BonusPassive=1;
    public int MalusPassive=-1;
    public int MultiplyEfficiency=2;
    public int DivideEfficiency=2;
    public float OverdriveIncreaseSpamEncourage = 0f;
    public float OverdriveIncreaseSpamDiscourage = 0.4f;
    public int BesiegeRessourcesLost = 50;
    public int BesiegeEfficciency = 2;
    private int numberOfEvent;

    // Use this for initialization
    void Awake () {
        data = GameObject.Find("GameManager").GetComponent<Data>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (data == null) Debug.Log("data = null");
	}
	
    public void createEvent()
    {
        System.Random rnd = new System.Random();

        switch (data.actualTiers)
        {
            case 1:
                if(data.malusInChained == 2)
                {
                    Debug.Log("ALOOOOOO");
                    data.currentEvent = data.EventTiers1[0];
                    data.malusInChained = 0;
                }
                else numberOfEvent = rnd.Next(0, data.EventTiers1.Length);
                data.currentEvent = data.EventTiers1[numberOfEvent];
                break;
            case 2:
                if (data.malusInChained == 2)
                {
                    numberOfEvent = rnd.Next(0, data.EventTiers2.Length/2);
                    data.malusInChained = 0;
                }
                else numberOfEvent = rnd.Next(0, data.EventTiers2.Length);
                data.currentEvent = data.EventTiers2[numberOfEvent];
                break;
            case 3:
                if (data.malusInChained == 2)
                {
                    numberOfEvent = rnd.Next(0, data.EventTiers3.Length / 2);
                    data.malusInChained = 0;
                }
                else numberOfEvent = rnd.Next(0, data.EventTiers3.Length);
                data.currentEvent = data.EventTiers3[numberOfEvent];
                break;
            case 4:
                if (data.malusInChained == 2)
                {
                    numberOfEvent = rnd.Next(0, data.EventTiers4.Length / 2);
                    data.malusInChained = 0;
                }
                else numberOfEvent = rnd.Next(0, data.EventTiers4.Length);
                data.currentEvent = data.EventTiers4[numberOfEvent];
                break;
         }
       
    }

    public void applyEvent()
    {
        if (data.currentEvent == null) return;
        switch(data.currentEvent)
        {
            case "EfficiencyMultiplyBy2":
                gameManager.ResourcesManager.Efficiency = gameManager.ResourcesManager.Efficiency * MultiplyEfficiency;
                Debug.Log("EfficiencyMultiplyBy2");
                break;
            case "EfficiencyDivideBy2":
                gameManager.ResourcesManager.Efficiency = gameManager.ResourcesManager.Efficiency/ DivideEfficiency;
                data.malusInChained += 1;
                Debug.Log("EfficiencyDivideBy2");
                break;
            case "MorePassive":
                gameManager.ResourcesManager.actualDecay = gameManager.ResourcesManager.actualDecay + BonusPassive;
                data.malusInChained += 1;
                Debug.Log("PassifPlus");
                break;
            case "LessPassive":
                gameManager.ResourcesManager.actualDecay = gameManager.ResourcesManager.actualDecay + MalusPassive;
                Debug.Log("PassifMoins");
                break;
            case "RessourcesBonus":
                for (int i = 0; i <= 3; i++)
                {
                    gameManager.ResourcesManager.Resources[i].Quantity += BonusRessources;
                }
                Debug.Log("RessourcesBonus");
                break;
            case "RessourcesMalus":
                for (int i = 0; i <= 3; i++)
                {
                    gameManager.ResourcesManager.Resources[i].Quantity -= MalusRessources;
                    if (gameManager.ResourcesManager.Resources[i].Quantity < 0) gameManager.ResourcesManager.Resources[i].Quantity = 0;
                }
                data.malusInChained += 1;
                Debug.Log(data.malusInChained);
                Debug.Log("RessourcesMalus");
                break;
            case "SpamEncourage":
                Debug.Log("SpamEncourage");
                gameManager.ResourcesManager.ActualOverDriveIncrease = OverdriveIncreaseSpamEncourage;
                break;
            case "SpamDiscourage":
                Debug.Log("SpamDiscourage");
                gameManager.ResourcesManager.ActualOverDriveIncrease = OverdriveIncreaseSpamDiscourage;
                data.malusInChained += 1;
                break;
            default:
                Debug.Log("Event non implémenté");
                break;

        }
            
    }

    public void ResetEvent()
    {
        if (data.currentEvent == null) return;
        switch (data.currentEvent)
        {
            case "EfficiencyMultiplyBy2":
                gameManager.ResourcesManager.Efficiency = gameManager.ResourcesManager.Efficiency / MultiplyEfficiency;
                break;
            case "EfficiencyDivideBy2":
                gameManager.ResourcesManager.Efficiency = gameManager.ResourcesManager.Efficiency * DivideEfficiency;
                break;
            case "MorePassive":
                gameManager.ResourcesManager.actualDecay = gameManager.ResourcesManager.BaseDecay;
                break;
            case "LessPassive":
                gameManager.ResourcesManager.actualDecay = gameManager.ResourcesManager.BaseDecay;
                break;
            case "SpamEncourage":
                gameManager.ResourcesManager.ActualOverDriveIncrease = gameManager.ResourcesManager.BaseOverDriveIncrease;
                break;
            case "SpamDiscourage":
                gameManager.ResourcesManager.ActualOverDriveIncrease = gameManager.ResourcesManager.BaseOverDriveIncrease;
                break;
            default:
                Debug.Log("no event to reset");
                break;
           
        }

    }

    public void StartBesiege()
    {
        for (int i = 0; i <= 3; i++)
        {
            gameManager.ResourcesManager.Resources[i].Quantity -= BesiegeRessourcesLost;
            if (gameManager.ResourcesManager.Resources[i].Quantity < 0) gameManager.ResourcesManager.Resources[i].Quantity = 0;

        }
        gameManager.ResourcesManager.Efficiency = BesiegeEfficciency;
        Debug.Log("BESIEGE");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
