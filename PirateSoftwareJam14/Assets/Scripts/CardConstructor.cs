using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class CardConstructor : MonoBehaviour
{
    public UpgradeCards Card;

    [SerializeField] private TMPro.TMP_Text Title;
    [SerializeField] private Image Art;
    [SerializeField] private TMPro.TMP_Text Price;

    [SerializeField] private TMPro.TMP_Text TextBox;

    [SerializeField] TMPro.TMP_Text money;


    public void ConstructCard()
    {
        gameObject.GetComponent<Image>().sprite = Card.Background;
        Art.GetComponent<Image>().sprite = Card.Art;

        Title.text = Card.Title;
        Title.color = Card.TextColor;
        Price.text = $"{Card.Price}$";
    }

    public void HoverText()
    {
        TextBox.text = Card.Description;
    }

    public void UnHoverText()
    {
        TextBox.text = "Feel free to set out now. or you could buy somthing, it would probably help.";
    }

    public void Buy()
    {
        if (CardList.instance.money >= Card.Price)
        {
            CardList.instance.money -= Card.Price;
            money.text = $"{CardList.instance.money}$";
            CardList.instance.ActiveCards.Add(Card);
            CardList.instance.upgradeCards.Remove(Card);
            TextBox.text = "Thank you for your money. Maybe buy some more of my junk, I mean upgrades";
            gameObject.SetActive(false);
        }
        else
        {
            TextBox.text = "This isn't a charity. Get some money then we can talk about this upgrade";
        }
    }

}
