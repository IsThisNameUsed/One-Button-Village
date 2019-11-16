using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    
    //O=bouffe 1=bois 2=pierre 3=eau
    public int[] conditionTiers2 = new int[4];
    public int[] conditionTiers3 = new int[4];
    public int[] conditionTiers4 = new int[4];
    
    public int stockMaxTiers1;
    
    public int stockMaxTiers2;
    
    public int stockMaxTiers3;
    
    public int stockMaxTiers4;

    public int actualTiers;
    public float WinterBeginning;
    public float AutumnBeginning;
    public float SummerBeginning;
    public float SpringBeginning;

    public float nextWinterBeginning;
    public float nextAutumnBeginning;
    public float nextSummerBeginning;
    public float nextSpringBeginning;

    public string[] EventTiers1; 
    public string[] EventTiers2; 
    public string[] EventTiers3; 
    public string[] EventTiers4;

    public bool besiege = false;
    public int yearsBeforeBesiege=1;
    public int timeBeforeEndBesiege = 1;
    public string currentEvent;
    public GameObject BesiegeSprite;
    public int malusInChained = 0;

    public void changeYear()
    {
        WinterBeginning = nextWinterBeginning;
        AutumnBeginning = nextAutumnBeginning;
        SummerBeginning = nextSummerBeginning;
        SpringBeginning = nextSpringBeginning;
    }
    void Awake()
    {
        EventTiers1 = new string[2] { "RessourcesBonus", "RessourcesMalus"};
        EventTiers2 = new string[4] { "EfficiencyMultiplyBy2", "RessourcesBonus", "EfficiencyDivideBy2", "RessourcesMalus" };
        EventTiers3 = new string[6] { "EfficiencyMultiplyBy2","RessourcesBonus", "LessPassive", "EfficiencyDivideBy2",
                                        "RessourcesMalus", "MorePassive" };
        EventTiers4 = new string[8] {"RessourcesBonus","EfficiencyMultiplyBy2","SpamEncourage","LessPassive", "RessourcesMalus", "EfficiencyDivideBy2",
                                    "SpamDiscourage", "MorePassive"};
    }
}
/*{ EfficiencyMultiplyBy2, EfficiencyDivideBy2 } 
 * { EfficiencyMultiplyBy2, EfficiencyDivideBy2, SpamEncourage, SpamDiscourage }
 * { EfficiencyMultiplyBy2, EfficiencyDivideBy2, SpamEncourage, SpamDiscourage, PassifPlus, PassifMoins }
 * { EfficiencyMultiplyBy2, EfficiencyDivideBy2, SpamEncourage, SpamDiscourage, Besiege, PassifPlus, PassifMoins } */
