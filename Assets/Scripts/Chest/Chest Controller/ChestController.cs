using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController 
{
   public ChestModel ChestModel { get; }

   public ChestView ChestView { get; }

   private bool IsChestClosed;


   public ChestController ( ChestModel chestModel , ChestView chestView)
   { 
       this.ChestModel = chestModel;
       this.ChestView = chestView;    
       ChestView.ChestController = this;
       IsChestClosed = true;

       InitializeController();
   }


   private void InitializeController()
   {
       ChestView.StoredCoinsText.text ="-"+ChestModel.CoinsStored;
       ChestView.StoredGemsText.text ="-"+ChestModel.GemsStored;

       ChestView.ChestButton.gameObject.SetActive(true);
       ChestView.ChestImage.gameObject.SetActive(true); 
        
       ChestView.ChestImage.sprite = ChestModel.ClosedChestImage;

      ChestView.ButtonText.text = ChestModel.TextForTimerButton;

      ChestView.ChestButton.onClick.AddListener( ()=> StartTimer());
   }


   private async void StartTimer()
    {          
       if(!ChestService.Instance.IsChestTimerStart)
       {    
            UpdateTheOpenButtonText();
           
            ChestView.ChestButton.onClick.AddListener( ()=> OpenNowButton() );
            
            ChestService.Instance.IsChestTimerStart = true;
            
            ChestView.TimerText.gameObject.SetActive(true);

            ChestView.TimerText.text ="Timer - " + ChestModel.TimeRequiredToOpen.ToString();
            // Invoke the timer of chest starts
           
            await Timer();

           if(ChestModel.TimeRequiredToOpen <= 0)
           {
               await OpenChest();
           }
       }
    }


    private async  System.Threading.Tasks.Task Timer()
    {
        int count = 0;
        while(ChestModel.TimeRequiredToOpen > 0 && IsChestClosed)
        {
            ChestModel.TimeRequiredToOpen--;
            count++;
            if( count >= 10 && ChestModel.TimeRequiredToOpen > 10) 
           {
                count = 0;
                ChestModel.GemsRequiredToOpen--;
                UpdateTheOpenButtonText();
           }
          
          if(ChestView != null)
          {
          ChestView.TimerText.text ="Timer - " + ChestModel.TimeRequiredToOpen.ToString();
          }
         await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
        
        }
    }

         

    private async void OpenNowButton()
    {
       if(!Player.Instance.isGemsAvailableToOpenChest(ChestModel.GemsRequiredToOpen))
       {
          Debug.Log("Not Enough Gems");
          return;
       }

       Player.Instance.ReduceGems(ChestModel.GemsRequiredToOpen);
     
       await OpenChest();
    }


    void UpdateTheOpenButtonText()
    {
       ChestView.ButtonText.text ="Open Now-" + ChestModel.GemsRequiredToOpen;
    }


    private async System.Threading.Tasks.Task OpenChest()
    {
       IsChestClosed = false;
       ChestService.Instance.IsChestTimerStart = false;
       ChestView.ChestButton.onClick.RemoveAllListeners();
       
       ChestView.StoredCoinsText.text ="- 0 ";
       ChestView.StoredGemsText.text ="- 0 ";
       ChestView.TimerText.text ="- 0";

       ChestView.ChestButton.gameObject.SetActive(false);
       ChestView.TimerText.gameObject.SetActive(false);
        
       ChestView.ChestImage.sprite = ChestModel.OpenedChestImage;
       ChestService.Instance.InvokeOnChestOpen(ChestModel.CoinsStored, ChestModel.GemsStored);
       
       await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5)); 

       ChestView.ChestImage.gameObject.SetActive(false); 
       ChestView.ChestImage.sprite = null;
      
       await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));

       ChestView.ChestController = null;
    }    


}




