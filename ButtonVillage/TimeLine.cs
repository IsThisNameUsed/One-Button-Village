using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TimeLine : MonoBehaviour {
    // Use this for initialization
    public Slider slider;
    public Text annonce;
    public GameManager gameManager;
    public Data data;
    public GameObject SpringSprite;
    public GameObject SummerSprite;
    public GameObject AutumnSprite;
    public GameObject WinterSprite;
    public enum seasons {Spring, Summer, Autumn, Winter};
    public seasons actualSeason;
    float nextSeason;
    private CreateYear createYear;
    private int[] newYear;
    RectTransform rt;
    float xPosition;
    float anchorMin;
    float anchorMax;

    public float YearLength = 20f;
    

    IEnumerator SliderAnimation(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            if (!GameManager.Instance.IsPaused)
            {
                animationTime += Time.deltaTime;
                float lerpValue = animationTime / seconds;
                slider.value = Mathf.Lerp(0f, 300f, lerpValue);
            }
            
            yield return null;
        }
    }

    float calculateAnchor(float val)
    {
        return (100 - val) / 100;
    }

    void Start () {
        data = GameObject.Find("GameManager").GetComponent<Data>();
        createYear = GameObject.Find("GameManager").GetComponent<CreateYear>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Position du sprite SpringcreateYear = GameObject.Find("GameManager").GetComponent<CreateYear>();
        rt = SpringSprite.transform as RectTransform;
        anchorMin = calculateAnchor(data.SummerBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.SpringBeginning);
        rt.anchorMax = new Vector2(anchorMax,1);

        rt = SummerSprite.transform as RectTransform;
        anchorMin = calculateAnchor(data.AutumnBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.SummerBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        rt = AutumnSprite.transform as RectTransform;
        anchorMin = calculateAnchor(data.WinterBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.AutumnBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        rt = WinterSprite.transform as RectTransform;
        anchorMin = calculateAnchor(100);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.WinterBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        StartCoroutine(SliderAnimation(YearLength));

        nextSeason = data.SummerBeginning;
	}
	
	// Update is called once per frame
	void Update (){

        if (slider.value > nextSeason)
            ChangeSeason();
        

        if (slider.value == 100)
        {
            gameManager.TutoFinished = true;

            if (gameManager.ResourcesManager.Resources[0].Quantity==0)
            {
                Debug.Log(gameManager.ResourcesManager.Resources[0].Quantity);
                SceneManager.LoadScene("Lose");
                return;
            }
                

            if (data.timeBeforeEndBesiege!=0) SceneManager.LoadScene("YearEnd");
            else
            {
                if (gameManager.ResourcesManager.Resources[0].Quantity > 0)
                    SceneManager.LoadScene("Win");
                else
                    SceneManager.LoadScene("Lose");
            }
        }
        
    }

    void ChangeSeason()
    {
        switch (actualSeason)
        {
            case seasons.Spring:
                actualSeason = seasons.Summer;
                nextSeason = data.AutumnBeginning;
                Keep.Instance.GetComponent<CamSeason>().ChangeSeason("Summer");
                break;
            case seasons.Summer:
                actualSeason = seasons.Autumn;
                nextSeason = data.WinterBeginning;
                Keep.Instance.GetComponent<CamSeason>().ChangeSeason("Autumn");
                break;
            case seasons.Autumn:
                actualSeason = seasons.Winter;
                nextSeason = slider.maxValue;
                Keep.Instance.GetComponent<CamSeason>().ChangeSeason("Winter");
                break;
        }

        GameManager.Instance.ResourcesManager.ChangeSeason(actualSeason);
    }
    
}
