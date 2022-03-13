using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
   public static Player Instance;

   int Coins = 500;

   int Gems = 500;
 
   [SerializeField] Text CoinText;
   [SerializeField] Text GemText;


   void OnEnable()
   {
       ChestService.OnChestOpen += ChestOpened; 
   }

   void OnDisable()
   {
       ChestService.OnChestOpen += ChestOpened; 
   }

   void ChestOpened(int Coin,int Gem)
   {
      Coins += Coin;
      Gems += Gem;
      print("Coin = "+Coins+"Gems"+Gems);
      CoinText.text ="-"+Coins;
      GemText.text ="-"+Gems ;
   }

   void Awake()
   {
       if(Instance == null)
       {
           Instance = this;
       }
   }

   public bool isGemsAvailableToOpenChest(int requiredGems)
   {
      if( Gems >= requiredGems)
      {
          return true;
      }
      return false;
   }

   public void ReduceGems(int gems)
   {
     Gems -= gems;
   }
}
