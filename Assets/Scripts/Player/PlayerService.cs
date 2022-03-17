using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerService : GenericSingleton<PlayerService>
{

   [SerializeField] int Coins;

   [SerializeField] int Gems;
 
   [SerializeField] Text CoinText;
   
   [SerializeField] Text GemText;

   public static event Action OnInsufficientGems;

   void OnEnable()
   {
       ChestService.OnChestOpen += ChestOpened; 
       ChestService.OnPressedOpeNowButton += ReduceGems; 
   }

   void OnDisable()
   {
       ChestService.OnChestOpen -= ChestOpened; 
       ChestService.OnPressedOpeNowButton -= ReduceGems; 
   }

   void Start()
   {
       UpdateCollectableText();
   }
   
   void ChestOpened(int Coin,int Gem)
   {
      Coins += Coin;
      Gems += Gem;
      UpdateCollectableText();
   }

   void UpdateCollectableText()
   {
      if(CoinText!=null && GemText!=null)
      {
      CoinText.text = Coins.ToString(); 
      GemText.text = Gems.ToString();
      }
   }

   public bool IsGemsSufficient(int requiredGems)
   {
       if( Gems > requiredGems)
       {
           return true;
       }
       return false;
   }

   public void ReduceGems(int requiredGems)
   {
      if(Gems < requiredGems)
      {
          OnInsufficientGems?.Invoke();
          return;
      }
      Gems -= requiredGems;
   }
}
