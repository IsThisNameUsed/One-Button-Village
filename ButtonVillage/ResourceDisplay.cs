using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used to display resources UI
public class ResourceDisplay : MonoBehaviour
{
    public GameObject ResourceChangePrefab;
    public Color PlusColor = Color.green;
    public Color MinusColor = Color.red;
    
    private List<Transform> resourcesTf;

    private void Awake()
    {
        resourcesTf = new List<Transform>();
        resourcesTf.Add(transform.Find("Resource 1"));
        resourcesTf.Add(transform.Find("Resource 2"));
        resourcesTf.Add(transform.Find("Resource 3"));
        resourcesTf.Add(transform.Find("Resource 4"));
    }
    
    public void UpdateDisplay(bool decay = false)
    {
        // Foreach resource, check if value was changed and create animation if it changed
        for(int i = 0; i < resourcesTf.Count; ++i)
        {
            UpdateResource(resourcesTf[i], GameManager.Instance.ResourcesManager.Resources[i], decay);
        }
    }

    public void UpdateResource(Transform resourceTf, Resource resource, bool decay)
    {
        if (resourceTf == null)
        {
            Debug.LogWarning("Can't display resource quantity because can't find UI transform");
            return;
        }
        
        Text QuantityText = resourceTf.Find("Text").GetComponent<Text>();
        string newQuantityStr = resource.Quantity.ToString();

        int oldQ = int.Parse(QuantityText.text);
        int difference = resource.Quantity - oldQ;

        // If the new value == old value, do nothing
        if (QuantityText.text == newQuantityStr)
            return;

        // Else, update UI ...
        QuantityText.text = newQuantityStr;

        string value = difference.ToString();
        if (difference > 0)
            value = "+" + value;

        // ... And pop an animation
        GameObject popup = Instantiate(ResourceChangePrefab, QuantityText.transform);
        popup.GetComponent<ResourceChangeBehaviour>().Pop(value, decay);

        // Set max value filler
        resourceTf.Find("Image").GetComponent<Image>().fillAmount = (float)resource.Quantity / (float)GameManager.Instance.ResourcesManager.CurrentLimit;
    }

    // Set auras around losing and gaining resources
    public void SetSeason(int minus, int plus)
    {
        foreach(Transform resourceTf in resourcesTf)
        {
            resourceTf.Find("Aura").GetComponent<Image>().enabled = false;
        }

        Image minusImage = resourcesTf[minus].Find("Aura").GetComponent<Image>();
        minusImage.color = MinusColor;
        minusImage.enabled = true;

        Image plusImage = resourcesTf[plus].Find("Aura").GetComponent<Image>();
        plusImage.color = PlusColor;
        plusImage.enabled = true;
    }
}
