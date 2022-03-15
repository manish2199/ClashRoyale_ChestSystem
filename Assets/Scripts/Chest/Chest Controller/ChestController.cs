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

   public static event Action<int,int> OnChestOpen;

   public static event Action<int> OnPressedOpeNowButton;

   public ChestController ( ChestModel chestModel , ChestView chestView)
   { 
       this.ChestModel = chestModel;
       this.ChestView = chestView;    
       ChestView.ChestController = this;
       IsChestClosed = true;

       InitializeChest();
   }


   private void InitializeChest()
   {
       ChestView.StoredCoinsText.text ="-"+ChestModel.CoinsStored;
       ChestView.StoredGemsText.text ="-"+ChestModel.GemsStored;

       ChestView.ChestButton.gameObject.SetActive(true);
       ChestView.ChestImage.gameObject.SetActive(true); 
        
       ChestView.ChestImage.sprite = ChestModel.ClosedChestImage;

      ChestView.ButtonText.text = ChestModel.TextForTimerButton;
      
      ChestView.ChestButton.onClick.AddListener( ()=> StartTimer());
   }


   public async void StartTimer()
    {          
       if(!ChestService.Instance.IsChestTimerStart)
       {    
           if(ChestView.ChestButton.gameObject.activeInHierarchy == false)
           {
               ChestView.ChestButton.gameObject.SetActive(true);
           }
           if(ChestView.WaitingListText.gameObject.activeInHierarchy == true)
           {
             ChestView.WaitingListText.gameObject.SetActive(false);
           }
            
            ChestView.ChestButton.onClick.RemoveAllListeners();
            ChestService.Instance.IsChestTimerStart = true;

            UpdateOpenNowButtonText();
           
            ChestView.ChestButton.onClick.AddListener( ()=> OpenNowButton() );
            
            ChestView.TimerText.gameObject.SetActive(true);

            ChestView.TimerText.text ="Timer - " + ChestModel.TimeRequiredToOpen.ToString();
           
            await Timer();

           if(ChestModel.TimeRequiredToOpen <= 0)
           {
               OpenChest();
           }
       }
       else
       {
           await ShowWaitingPanel();
       }
    }

    private async  System.Threading.Tasks.Task ShowWaitingPanel()
    {
        ChestView.AddToQueueButton.onClick.AddListener( ()=> AddChestToWaitingQueue());
        ChestView.WaitingPanel.SetActive(true);
       
        await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.5));

        ChestView.AddToQueueButton.onClick.RemoveAllListeners();
        ChestView.WaitingPanel.SetActive(false);
    } 

    void AddChestToWaitingQueue()
    {
        if(ChestService.Instance.CanAddChestToQueue())
        {
           ChestService.Instance.AddChestInWaitingQueue(ChestView.ChestController);
           ChestView.ChestButton.gameObject.SetActive(false);
           ChestView.WaitingListText.gameObject.SetActive(true);
        }
        else
        {
            ChestService.Instance.InvokeOnWaitingQueueFull();
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
                UpdateOpenNowButtonText();
           }
          
          if(ChestView != null)
          {
          ChestView.TimerText.text ="Timer - " + ChestModel.TimeRequiredToOpen.ToString();
          }
         await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
        
        }
    }


    private void OpenNowButton()
    {
       OnPressedOpeNowButton?.Invoke(ChestModel.GemsRequiredToOpen);
     
      // IF player hase sufficient gems
      if(PlayerService.Instance.IsGemsSufficient(ChestModel.GemsRequiredToOpen))
      {
       OpenChest();
      }
    }


    void UpdateOpenNowButtonText()
    {
        if(ChestView != null)
        {
            ChestView.ButtonText.text = "Open Now "+ChestModel.GemsRequiredToOpen;
            ChestView.CrystalImage.gameObject.SetActive(true);
        }
    }


    private async void OpenChest()
    {
       IsChestClosed = false;
       ChestService.Instance.IsChestTimerStart = false;
          
       OnChestOpen?.Invoke(ChestModel.CoinsStored,ChestModel.GemsStored);    
       
       await DetachTheView();
    }  


    private async System.Threading.Tasks.Task DetachTheView()
    {
       if(ChestView != null)
       {
       ChestView.ChestButton.onClick.RemoveAllListeners(); 
       ChestView.AddToQueueButton.onClick.RemoveAllListeners();

       ClearChestViewText(); 

       ChestView.WaitingListText.gameObject.SetActive(false);
       ChestView.ChestButton.gameObject.SetActive(false);
       ChestView.TimerText.gameObject.SetActive(false);
       ChestView.CrystalImage.gameObject.SetActive(false);
        
       ChestView.ChestImage.sprite = ChestModel.OpenedChestImage;
       
    
       await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5)); 

       ChestView.ChestImage.gameObject.SetActive(false); 
       ChestView.ChestImage.sprite = null;
       }
       await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5));

       ChestView.ChestController = null;

    }  

    private void ClearChestViewText()
    {
       ChestView.StoredCoinsText.text ="- 0 ";
       ChestView.StoredGemsText.text ="- 0 ";
       ChestView.TimerText.text ="- 0";
    }

}




