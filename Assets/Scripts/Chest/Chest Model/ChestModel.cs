using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestModel : MonoBehaviour
{
   public ChestModel(ChestScriptableObject chest)
   {
      this.ChestType = chest.ChestType;
      
      this.CoinsStored = chest.CoinsStored;
      
      this.GemsStored = chest.GemsStored;
      
      this.TimeRequiredToOpen = chest.TimeRequiredToOpen;
      
      this.GemsRequiredToOpen = chest.GemsRequiredToOpen;
      
      this.ClosedChestImage = chest.ClosedChestImage;

      this.OpenedChestImage = chest.OpenedChestImage;
   } 

   public ChestType ChestType { get; }
  
   public int CoinsStored { get; }

   public int GemsStored { get; }

   public int TimeRequiredToOpen { get; }

   public int GemsRequiredToOpen { get; }

   public Image ClosedChestImage;

   public Image OpenedChestImage;
  
//    public Button StartTimerButton;

//    public Button OpenNowButton; 

}
