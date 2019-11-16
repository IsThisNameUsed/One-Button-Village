using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ContinueButton : MonoBehaviour {
    public Data data;
    public ChangeScene changeScene;
    EventsManager eventsManager;

	public void pressContinue()
    {
        Debug.Log("Push continue");
        eventsManager = GameObject.Find("GameManager").GetComponent<EventsManager>();
        data = GameObject.Find("GameManager").GetComponent<Data>();
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
}
