using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "ScriptableObject", menuName = "ScriptableObject/NewChestScriptableObject")]
public class ChestScriptableObject : ScriptableObject
{
   public ChestType ChestType;

   public int CoinsStored;

   public int GemsStored;

   public int TimeRequiredToOpen;

   public int GemsRequiredToOpen;

   public Image ClosedChestImage;

   public Image OpenedChestImage;
  
   public Button StartTimerButton;

   public Button OpenNowButton;

}

public enum ChestType
{
    None,
    Silver,
    Golden,
    Magical,
    SuperMagical
}