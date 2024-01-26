using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text money;
    public int Seed;
    public GameObject[] ShopPlaces;
    //public static List<UpgradeCards> upgradeCards {  get; private set; }


    private int[] ThreeCards = new int[3];


    private void Start()
    {
        Random.seed = Seed;

        money.text = $"{PlayerController.PlayerInstance.Money}$";

        bool t = true;
        if (CardList.instance.upgradeCards.Count >= 3)
            while (t)
            {

                ThreeCards[0] = Random.Range(0, CardList.instance.upgradeCards.Count);

                ThreeCards[1] = Random.Range(0, CardList.instance.upgradeCards.Count);

                ThreeCards[2] = Random.Range(0, CardList.instance.upgradeCards.Count);

                if (ThreeCards[0] != ThreeCards[1] && ThreeCards[0] != ThreeCards[2] && ThreeCards[1] != ThreeCards[2])
                {
                    t = false;
                }
            }
        else
        {
            for (int i = 0; i < CardList.instance.upgradeCards.Count; i++)
            {
                CardToShop(i,i);
            }
            return;
        }


        for (int i = 0; i < ThreeCards.Length; i++)
        {
            CardToShop(ThreeCards[i], i);
        }


    }

    private void CardToShop(int index, int storeIndex)
    {
        ShopPlaces[storeIndex].SetActive(true);
        ShopPlaces[storeIndex].GetComponent<CardConstructor>().Card = CardList.instance.upgradeCards[index];

        Debug.Log($"{storeIndex}, {index}");
        ShopPlaces[storeIndex].GetComponent<CardConstructor>().ConstructCard();
    }

}

