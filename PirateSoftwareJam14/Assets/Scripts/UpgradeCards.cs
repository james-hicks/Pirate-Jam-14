using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UpgradeCards : ScriptableObject
{
    public Sprite Background;
    public string Title;
    public Color TextColor;
    public Sprite Art;
    public int Price;
    [TextArea(0, 3)]
    public string Description;

    
}
