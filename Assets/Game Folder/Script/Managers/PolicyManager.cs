using System;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PolicyManager : MonoBehaviour
{
    public GameObject PolicyCardUI;
    public Cards[] cards;

    public static PolicyManager Instance;
    public bool alreadyChose = false;

    bool isOpen = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenPolicyCardUI(bool b)
    { 
        PolicyCardUI.SetActive(b);
        if (isOpen == false)
            GenerateNewPolicy();

        if (b == false)
            alreadyChose = true;
    }

    public void GenerateNewPolicy()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            int rand = UnityEngine.Random.Range(0, 3);
            string title = "No Policy";
            string desc = "No Policy Description";
            if (rand == 0)
            {
                EconomyManager.Instance.GetRandomPolicy(PolicyType.Tax, out title, out desc, out PolicyModifier mods);
                cards[i].Title.text = title;
                cards[i].Description.text = desc;
                cards[i].cardss.PolicyModifier = mods;
            } else if (rand == 1)
            {
                EconomyManager.Instance.GetRandomPolicy(PolicyType.Regulation, out title, out desc, out PolicyModifier mods);
                cards[i].Title.text = title;
                cards[i].Description.text = desc;
                cards[i].cardss.PolicyModifier = mods;
            }
            else
            {
                EconomyManager.Instance.GetRandomPolicy(PolicyType.Subsidy, out title, out desc, out PolicyModifier mods);
                cards[i].Title.text = title;
                cards[i].Description.text = desc;
                cards[i].cardss.PolicyModifier = mods;
            }
        }
        isOpen = true;
    }


}

[Serializable]
public class Cards
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public PolicyCardss cardss;
}
