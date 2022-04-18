using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerState : ChestState
{ 
    public void StartTimer()
    {          
       if(!ChestService.Instance.IsChestTimerStart)
       {    
           if(ChestSlot.ChestButton.gameObject.activeInHierarchy == false)
           {
              ChestSlot.ChestButton.gameObject.SetActive(true);
           }
           if(ChestSlot.WaitingListText.gameObject.activeInHierarchy == true)
           {
             ChestSlot.WaitingListText.gameObject.SetActive(false);
           }      

           ChestSlot.ChestButton.onClick.RemoveAllListeners();
           ChestService.Instance.IsChestTimerStart = true;      

            UpdateOpenNowButtonText();
           
            ChestSlot.ChestButton.onClick.AddListener( ()=> OpenNowButton() );
            
            ChestSlot.TimerText.gameObject.SetActive(true);

            ChestSlot.TimerText.text ="Timer - " + ChestSlot.TimeRequiredToOpen.ToString();
           
            ChestSlot.TimerCoroutine = StartCoroutine(Timer());

           if(ChestSlot.TimeRequiredToOpen <= 0)
           {
               ChestSlot.SetState(ChestSlot.OpenChestState);
           }
       }
       else
       {
           StartCoroutine(ShowWaitingPanel());
       }
    }

    private IEnumerator ShowWaitingPanel()
    {
        ChestSlot.AddToQueueButton.onClick.AddListener( ()=> AddChestToWaitingQueue());
        ChestSlot.WaitingPanel.SetActive(true);
       
        
        yield return new WaitForSeconds(1.5f);

        ChestSlot.AddToQueueButton.onClick.RemoveAllListeners();
        ChestSlot.WaitingPanel.SetActive(false);
    } 

    void AddChestToWaitingQueue()
    {
        if(ChestService.Instance.CanAddChestToQueue())
        {
           ChestSlot chest = ChestSlot;
           ChestService.Instance.AddChestInWaitingQueue(chest);
           ChestSlot.ChestButton.gameObject.SetActive(false);
           ChestSlot.WaitingListText.gameObject.SetActive(true);
        }
        else
        {
            ChestService.Instance.InvokeOnWaitingQueueFull();
        }

    }


    private IEnumerator Timer()
    {
        int count = 0;
        while(ChestSlot.TimeRequiredToOpen > 0 && ChestSlot.IsChestClosed)
        {

            ChestSlot.TimeRequiredToOpen--;
            count++;
            if( count >= 10 && ChestSlot.TimeRequiredToOpen > 10) 
           {
                count = 0;
                ChestSlot.GemsRequiredToOpen--;
                UpdateOpenNowButtonText();
           }
           ChestSlot.TimerText.text ="Timer - " + ChestSlot.TimeRequiredToOpen.ToString();

            yield return new WaitForSeconds(1f);
        }

        if(ChestSlot.TimeRequiredToOpen <= 0)
        {
            //    this.OpenChest();
               ChestSlot.SetState(ChestSlot.OpenChestState);
        }
    }


    private void OpenNowButton()
    {      
      ChestService.Instance.InvokeOnPressedOpenNowButton(ChestSlot.GemsRequiredToOpen);
     
      // IF player hase sufficient gems
      if(PlayerService.Instance.IsGemsSufficient(ChestSlot.GemsRequiredToOpen))
      {
          StopCoroutine(ChestSlot.TimerCoroutine);
          ChestSlot.SetState(ChestSlot.OpenChestState);
      }

    }

    void Update(){}


    void UpdateOpenNowButtonText()
    {
        ChestSlot.ButtonText.text = "Open Now "+ChestSlot.GemsRequiredToOpen;
        
        if(ChestSlot.CrystalImage.gameObject.activeInHierarchy == false)
        {ChestSlot.CrystalImage.gameObject.SetActive(true);}

    }
    

	public override void OnEnter()
	{
		this.enabled = true;

       ChestSlot.IsChestClosed = true;
       ChestSlot.StoredCoinsText.text ="-"+ChestSlot.chestScriptableObject.CoinsStored;
       ChestSlot.StoredGemsText.text ="-"+ChestSlot.chestScriptableObject.GemsStored;

        ChestSlot.TimeRequiredToOpen = ChestSlot.chestScriptableObject.TimeRequiredToOpen;
        ChestSlot.GemsRequiredToOpen = ChestSlot.chestScriptableObject.GemsRequiredToOpen;

       ChestSlot.ChestButton.gameObject.SetActive(true);
       ChestSlot.ChestImage.gameObject.SetActive(true); 
       ChestSlot.EmptySlotImage.SetActive(false);
        
       ChestSlot.ChestImage.sprite = ChestSlot.chestScriptableObject.ClosedChestImage;

       ChestSlot.ButtonText.text = ChestSlot.chestScriptableObject.TextForTimerButton;
      
       ChestSlot.ChestButton.onClick.AddListener( ()=> StartTimer());
	}

    private void ClearChestTexts()
    {
       ChestSlot.StoredCoinsText.text ="- 0 ";
       ChestSlot.StoredGemsText.text ="- 0 ";
       ChestSlot.TimerText.text ="- 0";
    }


	public override void OnExit()
	{
        ChestSlot.TimerText.gameObject.SetActive(false);


       ChestSlot.ChestButton.onClick.RemoveAllListeners(); 
       ChestSlot.AddToQueueButton.onClick.RemoveAllListeners();

       ClearChestTexts(); 

       ChestSlot.WaitingListText.gameObject.SetActive(false);
       ChestSlot.ChestButton.gameObject.SetActive(false);
       
       ChestSlot.CrystalImage.gameObject.SetActive(false);
       ChestSlot.TimeRequiredToOpen = 0;
       ChestSlot.GemsRequiredToOpen = 0;
      
        this.enabled = false;
	}
}
