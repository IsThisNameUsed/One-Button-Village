using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to keep a game object, is unique (singleton)
public class Keep : MonoBehaviour
{
    public static Keep Instance = null;

	// Use this for initialization
	void Awake ()
    {
		if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
	}
}
