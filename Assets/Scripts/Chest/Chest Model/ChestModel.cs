using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestModel 
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

      this.TextForTimerButton = chest.TextForTimerButton;
   } 

   public ChestType ChestType { get; }
  
   public int CoinsStored { get; }

   public int GemsStored { get; }

   public int TimeRequiredToOpen { get; set; }

   public int GemsRequiredToOpen { get;set; }

   public Sprite ClosedChestImage;

   public Sprite OpenedChestImage;
  
   public string TextForTimerButton;

//    public Button OpenNowButton; 

}
