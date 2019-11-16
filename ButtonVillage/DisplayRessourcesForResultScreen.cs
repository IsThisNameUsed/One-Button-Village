using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class DisplayRessourcesForResultScreen : MonoBehaviour {
    //Récupère les conditions de tiers et le tiers actuel dans data
    //renvoi le tiers actuel à data
    //Récupère les quantités de ressource du joueur dans ressource manager
    //load la scène correspondant au tiers en cours
    //O=bouffe 1=bois 2=pierre 3=eau
    public Text foodGoal;
    public Text woodGoal;
    public Text stoneGoal;
    public Text waterGoal;
    public RessourcesManager ResourcesManager;
    
    public Text playerFood;
    public Text playerWood;
    public Text playerStone;
    public Text playerWater;
    public Text Time;
    public Data data;

    private int[] goals;

    private GameObject antislash;


    void Start () {
        data = GameObject.Find("GameManager").GetComponent<Data>();
        ResourcesManager = GameObject.Find("GameManager").GetComponent<RessourcesManager>();

        //Ecran de fin d'anné Tiers 1 à 3
        if (data.actualTiers < 4)
        {
            switch (data.actualTiers)
            {
                case 1:
                    goals = data.conditionTiers2;
                    SoundManager.Instance.ChangeMusic("tier2", 0.8f);
                    GameManager.Instance.ResourcesManager.CurrentLimit = GameManager.Instance.data.stockMaxTiers2;
                    break;
                case 2:
                    goals = data.conditionTiers3;
                    SoundManager.Instance.ChangeMusic("tier3", 0.8f);
                    GameManager.Instance.ResourcesManager.CurrentLimit = GameManager.Instance.data.stockMaxTiers3;
                    break;
                case 3:
                    goals = data.conditionTiers4;
                    SoundManager.Instance.ChangeMusic("tier4", 0.8f);
                    GameManager.Instance.ResourcesManager.CurrentLimit = GameManager.Instance.data.stockMaxTiers4;
                    break;
                default:
                    Debug.Log("Impossible de récupérer les objectifs");
                    break;
            }
            foodGoal.text = goals[0].ToString();
            woodGoal.text = goals[1].ToString();
            stoneGoal.text = goals[2].ToString();
            waterGoal.text = goals[3].ToString();

            if (ResourcesManager.Resources[0].Quantity >= goals[0] && ResourcesManager.Resources[1].Quantity >= goals[1]
            && ResourcesManager.Resources[2].Quantity >= goals[2] && ResourcesManager.Resources[3].Quantity >= goals[3]
            && data.actualTiers < 4)
            {
                data.actualTiers = data.actualTiers + 1;
            }
        }
        //Ecran fin d'anné Tiers 4
        else
        {
            /* GameObject BesiegeDisplay = GameObject.Find("Siege");
             if (BesiegeDisplay == null) Debug.Log("SPRITE NULL");
             BesiegeDisplay.SetActive(true);
              GameObject BesiegeDisplay = GameObject.Find("Siege");
              BesiegeDisplay.GetComponent<Image>().enabled=true;
              GameObject.Find("SpriteSiege").SetActive(true);
              BesiegeDisplay.SetActive(true);*/
            Time.text = data.yearsBeforeBesiege.ToString();
            foodGoal.enabled = false;
            woodGoal.enabled = false;
            stoneGoal.enabled = false;
            waterGoal.enabled = false;
            antislash = GameObject.Find("antislash1");
            antislash.SetActive(false);
            antislash = GameObject.Find("antislash2");
            antislash.SetActive(false);
            antislash = GameObject.Find("antislash3");
            antislash.SetActive(false);
            antislash = GameObject.Find("antislash4");
            antislash.SetActive(false);
        }

        

        playerFood.text = ResourcesManager.Resources[0].Quantity.ToString();
        playerWood.text = ResourcesManager.Resources[1].Quantity.ToString();
        playerStone.text = ResourcesManager.Resources[2].Quantity.ToString();
        playerWater.text = ResourcesManager.Resources[3].Quantity.ToString();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
