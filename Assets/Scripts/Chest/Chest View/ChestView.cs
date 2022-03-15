 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour , Container
{
  [HideInInspector] public ChestController ChestController;

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


  public bool IsContainerEmpty()
  {
    if( ChestController == null)
    {
        return true;
    }
    return false;
  }

}
