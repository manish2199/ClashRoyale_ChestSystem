using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChestState : ChestState
{
    private IEnumerator DetachTheView()
    {
        ChestSlot.ChestImage.sprite = ChestSlot.chestScriptableObject.OpenedChestImage;

        yield return new WaitForSeconds(0.5f);
       
       ChestSlot.EmptySlotImage.SetActive(true);
       ChestSlot.ChestImage.gameObject.SetActive(false);
     
        yield return new WaitForSeconds(0.5f);
     
        ChestSlot.chestScriptableObject = null;
    }  

    void Update(){}
    
    public override void OnEnter()
    {
       this.enabled = true;
        
       ChestSlot.IsChestClosed = false;
       ChestService.Instance.IsChestTimerStart = false;
            
       ChestService.Instance.InvokeOnChestOpen(ChestSlot.chestScriptableObject.CoinsStored,ChestSlot.chestScriptableObject.GemsStored);
       ChestService.Instance.OpenChestInsideWaitingQueue();

       StartCoroutine(DetachTheView());
    }

	public override void OnExit()
	{
       ChestSlot.TimerCoroutine = null;
       ChestSlot.ChestImage.sprite = null;

       this.enabled = false;
	}
}
