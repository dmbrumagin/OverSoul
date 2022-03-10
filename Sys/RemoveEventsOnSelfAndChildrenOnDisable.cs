using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class RemoveEventsOnSelfAndChildrenOnDisable : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnDisable()
    {
        foreach(Transform transform in this.gameObject.transform)
        {
            EventTrigger eventTrigger =transform.gameObject.GetComponent<EventTrigger>();

            if(eventTrigger)
            {
                foreach(EventTrigger.Entry entry in eventTrigger.triggers)
                {
                entry.callback.RemoveAllListeners();
                }            
            }
                        
        }
    }
}
