using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Little UI game object to pop ressource changes
public class ResourceChangeBehaviour : MonoBehaviour
{
    internal void Pop(string value, bool decay)
    {
        GetComponent<Text>().text = value;

        if (decay)
            GetComponent<Animator>().SetTrigger("Decay");
        else
            GetComponent<Animator>().SetTrigger("Pop");

        Destroy(gameObject, 1f);
    }
}
