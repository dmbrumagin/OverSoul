using UnityEngine;
using InventoryRelated;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    ItemObject item;
    public int AttackValue;
    public int DefenceValue;
    public ItemType _type;

    private EnemyDamageHandler.typeOfMonster[] monsterToLearn = {
        EnemyDamageHandler.typeOfMonster.bird,
        EnemyDamageHandler.typeOfMonster.slime,
        EnemyDamageHandler.typeOfMonster.tree,
        EnemyDamageHandler.typeOfMonster.rock,
        EnemyDamageHandler.typeOfMonster.water
    };

    public void Awake()
    {
        item = this;
        type = _type;
    }
}
