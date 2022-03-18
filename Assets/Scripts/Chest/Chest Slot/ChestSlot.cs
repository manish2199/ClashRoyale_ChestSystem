using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChestSlot : MonoBehaviour , Container
{
  [HideInInspector]  public ChestScriptableObject chestScriptableObject;  
  [HideInInspector]  public int TimeRequiredToOpen;
  [HideInInspector]  public int GemsRequiredToOpen;
  [HideInInspector]  public Coroutine TimerCoroutine;
  [HideInInspector]  public bool IsChestClosed;


  public GameObject WaitingPanel;
  public Button AddToQueueButton;
  public Text StoredCoinsText;
  public Text StoredGemsText;
  public Text TimerText;
  public Text WaitingListText;
  public Image ChestImage;
  public GameObject EmptySlotImage;
  public Button ChestButton;
  public Text ButtonText;
  public Image CrystalImage;
  public Transform ChestSlotTransform;


  private ChestState CurrentState = null;

  public TimerState timerState;
  public OpenChestState OpenChestState;
 
  public void SetState(ChestState state)
  {
      if(CurrentState != null)
      {
          CurrentState.OnExit();
      }
      CurrentState = state;
      
      CurrentState.OnEnter();
  }

  public bool IsContainerEmpty()
  {
    if( chestScriptableObject == null)
    {
        return true;
    }
    return false;
  }

   
  public void SetChestType(ChestScriptableObject chestScriptableObject)
  {     
    this.chestScriptableObject = chestScriptableObject;

    SetState(timerState);
  }
}






