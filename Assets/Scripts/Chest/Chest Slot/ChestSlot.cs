using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChestSlot : MonoBehaviour , Container
{
  private ChestScriptableObject chestScriptableObject;

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


  private bool IsChestClosed;

  
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
    IsChestClosed = true;

    this.InitializeChest();
  }


   private void InitializeChest()
   {
       print("Inside Chest Intialize");
       StoredCoinsText.text ="-"+chestScriptableObject.CoinsStored;
       StoredGemsText.text ="-"+chestScriptableObject.GemsStored;

       ChestButton.gameObject.SetActive(true);
       ChestImage.gameObject.SetActive(true); 
        
       ChestImage.sprite = chestScriptableObject.ClosedChestImage;

       ButtonText.text = chestScriptableObject.TextForTimerButton;
      
       ChestButton.onClick.AddListener( ()=> StartTimer());
   }


   public async void StartTimer()
    {          
       if(!ChestService.IsChestTimerStart)
       {    
           print("Start tIMER");
           if(ChestButton.gameObject.activeInHierarchy == false)
           {
              ChestButton.gameObject.SetActive(true);
           }
           if(WaitingListText.gameObject.activeInHierarchy == true)
           {
             WaitingListText.gameObject.SetActive(false);
           }
            
            ChestButton.onClick.RemoveAllListeners();
            ChestService.IsChestTimerStart = true;

            UpdateOpenNowButtonText();
           
            ChestButton.onClick.AddListener( ()=> OpenNowButton() );
            
            TimerText.gameObject.SetActive(true);

            TimerText.text ="Timer - " + chestScriptableObject.TimeRequiredToOpen.ToString();
           
            await Timer();

           if(chestScriptableObject.TimeRequiredToOpen <= 0)
           {
              print("oPEN cHEST");
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
        AddToQueueButton.onClick.AddListener( ()=> AddChestToWaitingQueue());
        WaitingPanel.SetActive(true);
       
        await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.5));

        AddToQueueButton.onClick.RemoveAllListeners();
        WaitingPanel.SetActive(false);
    } 

    void AddChestToWaitingQueue()
    {
        if(ChestService.Instance.CanAddChestToQueue())
        {
           ChestService.Instance.AddChestInWaitingQueue(this);
           ChestButton.gameObject.SetActive(false);
           WaitingListText.gameObject.SetActive(true);
        }
        else
        {
            ChestService.Instance.InvokeOnWaitingQueueFull();
        }

    }


    private async  System.Threading.Tasks.Task Timer()
    {
        int count = 0;
        while(chestScriptableObject.TimeRequiredToOpen > 0 && IsChestClosed)
        {
            chestScriptableObject.TimeRequiredToOpen--;
            count++;
            if( count >= 10 && chestScriptableObject.TimeRequiredToOpen > 10) 
           {
                count = 0;
                chestScriptableObject.GemsRequiredToOpen--;
                UpdateOpenNowButtonText();
           }
          
          TimerText.text ="Timer - " + chestScriptableObject.TimeRequiredToOpen.ToString();
          
         await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
        
        }
    }


    private void OpenNowButton()
    {
      ChestService.Instance.InvokeOnPressedOpenNowButton(chestScriptableObject.GemsRequiredToOpen);
     
      // IF player hase sufficient gems
      if(PlayerService.Instance.IsGemsSufficient(chestScriptableObject.GemsRequiredToOpen))
      {
       OpenChest();
      }
    }


    void UpdateOpenNowButtonText()
    {
          ButtonText.text = "Open Now "+chestScriptableObject.GemsRequiredToOpen;
          CrystalImage.gameObject.SetActive(true);
    }


    private async void OpenChest()
    {
       IsChestClosed = false;
       ChestService.IsChestTimerStart = false;
              
       ChestService.Instance.InvokeOnChestOpen(chestScriptableObject.CoinsStored,chestScriptableObject.GemsStored);

       await DetachTheView();
    }  


    private async System.Threading.Tasks.Task DetachTheView()
    {
       ChestButton.onClick.RemoveAllListeners(); 
       AddToQueueButton.onClick.RemoveAllListeners();

       ClearChestTexts(); 

       WaitingListText.gameObject.SetActive(false);
       ChestButton.gameObject.SetActive(false);
       TimerText.gameObject.SetActive(false);
       CrystalImage.gameObject.SetActive(false);
        
       ChestImage.sprite = chestScriptableObject.OpenedChestImage;
       
    
       await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5)); 

       ChestImage.gameObject.SetActive(false); 
       ChestImage.sprite = null;

       await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5));

       chestScriptableObject = null;

    }  

    private void ClearChestTexts()
    {
       StoredCoinsText.text ="- 0 ";
       StoredGemsText.text ="- 0 ";
       TimerText.text ="- 0";
    }
}




