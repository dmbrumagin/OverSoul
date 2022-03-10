using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using InventoryRelated;

public static class CursorData
    {
        public static Vector3 position;
        public static MenuDisplay interfaceCursorOn;
        public static GameObject tempItemDrag;
        public static InventorySlot cursorInventorySlot;
        public static InventorySlot inventorySlotHoveredOver;
        public static InventorySlot toolTipSlotToShow;
        public static AbilityInventorySlot abilityTooltip;
        public static AbilityInventorySlot abilitySlotHoveredOver;
        public static Ability abilityHoveredOver;
        public static GameObject slotHoveredOver;
    }
