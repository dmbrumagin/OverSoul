using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Player;

public class Money :MonoBehaviour, ISerializationCallbackReceiver
{
    public Attributes money;
    public int amount;
    private int range;
    private int[] moneyValues;
    public void Awake()
    {
        moneyValues = new int[] { 1, 5, 10};
        range = Random.Range(0, 2);
        amount = moneyValues[range];
    }
    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = money.icon;

    }
}
