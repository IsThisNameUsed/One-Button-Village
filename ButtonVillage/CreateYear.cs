using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateYear : MonoBehaviour {

    int typeOfYear;
    int summerBegin;
    int autumnBegin;
    int winterBegin;
    [Header("4 mini, increase value to have more standard year")]
    public int ratio = 10;
    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void createYear(string currentEvent)
    {
        
    System.Random rnd = new System.Random();
        //0 long spring 1 long summer 2 long autumn 3 long winter 4 to 10 balanced year
        typeOfYear = rnd.Next(0, ratio);

        if (typeOfYear == 0) { summerBegin = 40; ; autumnBegin = 60; winterBegin = 80; }
        else if (typeOfYear == 1) { summerBegin = 20; autumnBegin = 60; winterBegin = 80; }
        else if (typeOfYear == 2) { summerBegin = 20; autumnBegin = 40; winterBegin = 80; }
        else if (typeOfYear == 3) { summerBegin = 20; autumnBegin = 40; winterBegin = 60; }
        else { summerBegin  = 25; autumnBegin = 50; winterBegin = 75; }

        gameManager.data.nextWinterBeginning = winterBegin;
        gameManager.data.nextAutumnBeginning = autumnBegin;
        gameManager.data.nextSummerBeginning = summerBegin;
        gameManager.data.nextSpringBeginning = 0; 
    }
	
	
	// Update is called once per frame
	void Update () {
		
	}


}
