 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour , Container
{
   [HideInInspector] public ChestController ChestController;

   public Text StoredCoinsText;
    
   public Text StoredGemsText;

   public Text TimerText;
 
   public Image ChestImage;

   public GameObject EmptySlotImage;
  
   public Button ChestButton;
   public Text ButtonText;
  

    // ChestButton.gameObject.SetActive(true);
    //   ChestImage.gameObject.SetActive(true); 
   


  public bool IsContainerEmpty()
  {
    if( ChestController == null)
    {
        return true;
    }
    return false;
  }


}
