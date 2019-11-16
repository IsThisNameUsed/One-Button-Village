using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class DrawYearsForResultScreen : MonoBehaviour {
    // Use this for initialization
    public Data data;
    public GameObject SpringSprite1;
    public GameObject SummerSprite1;
    public GameObject AutumnSprite1;
    public GameObject WinterSprite1;
    public GameObject SpringSprite2;
    public GameObject SummerSprite2;
    public GameObject AutumnSprite2;
    public GameObject WinterSprite2;
    RectTransform rt;
    float xPosition;
    float anchorMin;
    float anchorMax;

    float calculateAnchor(float val)
    {
        return (100 - val) / 100;
    }

    void Start()
    {
        data = GameObject.Find("GameManager").GetComponent<Data>();
        //Positions des sprites année passé
        rt = SpringSprite1.transform as RectTransform;
        anchorMin = calculateAnchor(data.SummerBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.SpringBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        rt = SummerSprite1.transform as RectTransform;
        anchorMin = calculateAnchor(data.AutumnBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.SummerBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        rt = AutumnSprite1.transform as RectTransform;
        anchorMin = calculateAnchor(data.WinterBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.AutumnBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        rt = WinterSprite1.transform as RectTransform;
        anchorMin = calculateAnchor(100);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.WinterBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        //Positions des sprites année future
        rt = SpringSprite2.transform as RectTransform;
        anchorMin = calculateAnchor(data.nextSummerBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.nextSpringBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        rt = SummerSprite2.transform as RectTransform;
        anchorMin = calculateAnchor(data.nextAutumnBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.nextSummerBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        rt = AutumnSprite2.transform as RectTransform;
        anchorMin = calculateAnchor(data.nextWinterBeginning);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.nextAutumnBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);

        rt = WinterSprite2.transform as RectTransform;
        anchorMin = calculateAnchor(100);
        rt.anchorMin = new Vector2(anchorMin, 0);
        anchorMax = calculateAnchor(data.nextWinterBeginning);
        rt.anchorMax = new Vector2(anchorMax, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
