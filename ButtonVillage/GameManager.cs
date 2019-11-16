using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

// Main script
public class GameManager : MonoBehaviour
{
    // Public singleton, can be accessed from anywhere
    public static GameManager Instance;

    public bool IsPaused;
    public bool TutoFinished;

    // Resources manager and UI scripts
    public RessourcesManager ResourcesManager;
    public ResourceDisplay ResourceDisplay;
    public Data data;
    public EventsManager eventsManager;
    public CreateYear createYear;

    // Overdrive display script
    public OverdriveDisplay OverDriveDisplay;

    // Transitions animator
    public Animator TransitionsAnimator;

    // Main Camera
    public Camera Camera;
    

    // Coroutine called each second for resource decay
    //private Coroutine _corDecay;

    // Main button, moving in each scene
    private Button button;

    // Animator for end year canvas
    private Animator _yearEndCanvasAnimator;

	// Use this for initialization
	void Awake ()
    {
        // Checking singleton
		if (Instance != null)
        {
            Debug.Log("Game Manager already spawned");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        button = GameObject.FindGameObjectWithTag("Button Canvas").transform.Find("Button").GetComponent<Button>();
        TransitionsAnimator = transform.parent.Find("Transitions Canvas").GetComponent<Animator>();

        // Set listener to call OnSceneChanged when a new scene is loaded
        SceneManager.activeSceneChanged += OnSceneChanged;

        data = GetComponent<Data>();
        eventsManager = GetComponent<EventsManager>();
        createYear = GetComponent<CreateYear>();
    }

    private void OnSceneChanged(Scene arg0, Scene arg1)
    {
        // Stop decay coroutine
        StopAllCoroutines();

        // Remove button action
        button.onClick.RemoveAllListeners();

        switch (arg1.name)
        {
            case "Tiers1":
                eventsManager.applyEvent();
                button.gameObject.SetActive(true);
                InitTier();
                ResourcesManager.CurrentLimit = data.stockMaxTiers1;
                break;
            case "Tiers2":
                ResourcesManager.Efficiency = ResourcesManager.Tiers2Efficiency;
                ResourcesManager.Consommation = ResourcesManager.Tiers2Consommation;
                eventsManager.applyEvent();
                button.gameObject.SetActive(true);
                InitTier();
                break;
            case "Tiers3":
                ResourcesManager.Efficiency = ResourcesManager.Tiers3Efficiency;
                ResourcesManager.Consommation = ResourcesManager.Tiers3Consommation;
                eventsManager.applyEvent();
                button.gameObject.SetActive(true);
                InitTier();
                break;
            case "Tiers4":
                ResourcesManager.Consommation = ResourcesManager.Tiers4Consommation;
                button.gameObject.SetActive(true);
                InitTier();
                if (data.yearsBeforeBesiege > 0)
                {
                    ResourcesManager.Efficiency = ResourcesManager.Tiers4Efficiency;
                    data.yearsBeforeBesiege -= 1;
                    eventsManager.applyEvent();
                }
                else
                {
                    data.besiege = true;
                    data.timeBeforeEndBesiege -= 1;
                    eventsManager.StartBesiege();
                }
                
                break;
            case "yearEnd":
                InitYearEnd();
                break;
            case "Menu":
                button.onClick.AddListener(GoToLevel1);
                break;
            case "Lose":
                SoundManager.Instance.ChangeMusic("Lose", 0.8f);
                button.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
                break;
            case "Win":
                SoundManager.Instance.ChangeMusic("Win", 0.8f);
                button.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
                break;
            default:
                Debug.Log("Impossible de charger le tiers");
            return;
        }
        
    }

    #region Menu Logic
    
    private void GoToLevel1()
    {
        SceneManager.LoadScene("Tiers1");
    }

    #endregion

    #region Year End Logic

    public void InitYearEnd()
    {
        _yearEndCanvasAnimator = GameObject.Find("ResultScreenCanvas").GetComponent<Animator>();
        _yearEndCanvasAnimator.transform.Find("Panel/Resources/Siege").gameObject.SetActive(data.actualTiers == 4);

        ResourcesManager.InGame = false;

        eventsManager.ResetEvent();
        eventsManager.createEvent();
        createYear.createYear(data.currentEvent);
        switch (data.currentEvent)
        {
            case "RessourcesBonus":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/RessourcesBonus").gameObject.SetActive(true);
                break;
            case "RessourcesMalus":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/RessourcesMalus").gameObject.SetActive(true);
                break;
            case "EfficiencyMultiplyBy2":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/MoreEfficiency").gameObject.SetActive(true);
                break;
            case "EfficiencyDivideBy2":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/LessEfficiency").gameObject.SetActive(true);
                break;
            case "SpamEncourage":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/Text").GetComponent<Text>().text = data.currentEvent;
                //_yearEndCanvasAnimator.transform.Find("Panel (1)/SpamEncourage").gameObject.SetActive(true);
                break;
            case "SpamDiscourage":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/Text").GetComponent<Text>().text = data.currentEvent;
                //_yearEndCanvasAnimator.transform.Find("Panel (1)/SpamDiscourage").gameObject.SetActive(true);
                break;
            case "LessPassive":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/LessPassive").gameObject.SetActive(true);
                break;
            case "MorePassive":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/MorePassive").gameObject.SetActive(true);
                break;
            case "Besiege":
                _yearEndCanvasAnimator.transform.Find("Panel (1)/Text").GetComponent<Text>().text = data.currentEvent + "BESIEGE"; ;
                _yearEndCanvasAnimator.transform.Find("Panel (1)/RessourceBonus").gameObject.SetActive(true);
                break;

        }
        _yearEndCanvasAnimator.transform.Find("Panel (1)/LessEfficiency").gameObject.SetActive(true);
        StartCoroutine(CorInitYearEnd());
    }

    private IEnumerator CorInitYearEnd()
    {
        yield return new WaitForSeconds(1.5f);
        button.onClick.AddListener(() => StartCoroutine(CorActionGoToEvent()));
    }

    private IEnumerator CorActionGoToEvent()
    {
        GameObject.Find("ResultScreenCanvas").GetComponent<Animator>().SetTrigger("event");
        button.onClick.RemoveAllListeners();
        yield return new WaitForSeconds(1.5f);
        button.onClick.AddListener(ActionEndYear);
    }

    public void ActionEndYear()
    {
        button.onClick.RemoveAllListeners();

        switch (data.actualTiers)
        {
            case 1:
                data.changeYear();
                SceneManager.LoadScene("Tiers1");
                break;
            case 2:
                data.changeYear();
                SceneManager.LoadScene("Tiers2");
                break;
            case 3:
                data.changeYear();
                SceneManager.LoadScene("Tiers3");
                break;
            case 4:
                data.changeYear();
                SceneManager.LoadScene("Tiers4");
                break;
            default:
                Debug.Log("Impossible de charger le tiers");
                return;
        }
    }

    #endregion

    #region Tier Logic

    public void InitTier()
    {
        // Set CamSeason
        transform.parent.GetComponent<CamSeason>().CurrentSeason = "Spring";

        // Fetch Overdrive Display script
        OverDriveDisplay = GameObject.FindGameObjectWithTag("Resource Canvas").transform.Find("Ressources Panel").GetComponent<OverdriveDisplay>();

        // Fetch resources scripts and start year
        ResourceDisplay = OverDriveDisplay.GetComponent<ResourceDisplay>();
        ResourcesManager = GetComponent<RessourcesManager>();
        ResourcesManager.StartYear();

        // Get main camera
        Camera = Camera.main;

        // Set resource decay
        /*_corDecay = */
        StartCoroutine(CorDecayResources());

        // Set button behaviour
        button.onClick.AddListener(ActionTier);
    }

    // Action called when button is pressed in game
    public void ActionTier()
    {
        ResourcesManager.Action();
        ResourceDisplay.UpdateDisplay();
    }

    // Called each second to decay resources
    private IEnumerator CorDecayResources()
    {
        yield return new WaitUntil(() => ResourcesManager.Resources != null);

        while (true)
        {
            if (!IsPaused)
            {
                ResourcesManager.PassiveConsommation();
                ResourceDisplay.UpdateDisplay(true);
            }
            
            yield return new WaitForSeconds(ResourcesManager.DecayInterval);
        }
    } 

    #endregion
}
