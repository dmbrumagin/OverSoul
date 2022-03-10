using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UI;
using System;
using UI;

namespace Player
{
    public class Tooltip : MonoBehaviour
    {
        public  Animator tooltipAnimator;
        public  GameObject tooltipName;
        public  GameObject tooltipType;
        public  GameObject tooltipBuffs;
        public  GameObject tooltipDescription;
        public string text;
      

        public  void ShowTooltip()
        {           
            tooltipName.GetComponent<Text>().text = CursorData.toolTipSlotToShow.item.Name;
            tooltipType.GetComponent<Text>().text = CursorData.toolTipSlotToShow.item.type.ToString();
            
                if (CursorData.toolTipSlotToShow.item.buffs.Length > 0)
                {
                tooltipBuffs.GetComponent<Text>().text = "";
                for (int i = 0; i < CursorData.toolTipSlotToShow.item.buffs.Length; i++)
                    {
                    tooltipBuffs.GetComponent<Text>().text += CursorData.toolTipSlotToShow.item.buffs[i].stat.statType + "-" + CursorData.toolTipSlotToShow.item.buffs[i].value + "\n";
                    }
                }
            else
            {
                tooltipBuffs.GetComponent<Text>().text = "";
            }

            tooltipDescription.GetComponent<Text>().text = CursorData.toolTipSlotToShow.item.description;
        }

        public void showToolTip2()
        {
            tooltipName.GetComponent<Text>().text = CursorData.abilityTooltip.ability.Name;
            tooltipType.GetComponent<Text>().text = CursorData.abilityTooltip.ability.requiredAbilityPoints.ToString() +"XP Needed";

            if (CursorData.abilityTooltip.ability.buffs.Length > 0)
            {
                tooltipBuffs.GetComponent<Text>().text = "";
                for (int i = 0; i < CursorData.abilityTooltip.ability.buffs.Length; i++)
                {
                    tooltipBuffs.GetComponent<Text>().text += CursorData.abilityTooltip.ability.buffs[i].stat.ToString() + "-" + CursorData.abilityTooltip.ability.buffs[i].value.ToString() + "\n";
                }
            }

            else
            {
                tooltipBuffs.GetComponent<Text>().text = "";
            }

            tooltipDescription.GetComponent<Text>().text = CursorData.abilityTooltip.ability.description;
        }

        public  void HideTooltip()
        {
            tooltipAnimator.SetTrigger("Hide");
            /* text = "";
            tooltipBuffs.GetComponent<Text>().text = "";
             tooltipDescription.GetComponent<Text>().text = "";
             trigger = "Hide";
             if (tooltipAnimator == null || string.IsNullOrEmpty(trigger)) return;
             tooltipAnimator.SetTrigger(trigger);*/
        }

    }
}