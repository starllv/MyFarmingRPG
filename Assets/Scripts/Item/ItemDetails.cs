using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int ItemCode;
    public ItemType ItemType;
    public string itemDescription;
    public Sprite itemSprite;
    public string itemLongDescription;
    public short itemUseGridRadius;
    public float itemUseRadius;
    public bool isStartingItem;
    public bool canBePickUp;
    public bool canBeDropped;
    public bool canBeEaten;
    public bool canBeCarried;
    
}
