using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    public static CardList instance { get; private set; }

    public List<UpgradeCards> upgradeCards;
    public List<UpgradeCards> ActiveCards;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

}