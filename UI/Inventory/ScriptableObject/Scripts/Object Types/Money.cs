using UnityEngine;
using Player;

public class Money :MonoBehaviour, ISerializationCallbackReceiver
{
    public Stat money;
    public int amount;
    private int range;
    private int[] moneyValues;
    public Sprite icon;

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
        GetComponentInChildren<SpriteRenderer>().sprite = icon;
    }
}
