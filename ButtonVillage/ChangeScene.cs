using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadTiers1()
    {

        SceneManager.LoadScene("Tiers1");
    }

    public void LoadTiers2()
    {
        SceneManager.LoadScene("Tiers2");
    }

    public void LoadTiers3()
    {
        SceneManager.LoadScene("Tiers3");
    }

    public void LoadTiers4()
    {
        SceneManager.LoadScene("Tiers4");
    }

    public void LoadMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void LoadYearEnd() {

        SceneManager.LoadScene("yearEnd");
     }

}
