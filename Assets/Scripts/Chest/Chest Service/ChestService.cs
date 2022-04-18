using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestService : GenericSingleton<ChestService>
{
    [SerializeField] private ChestSlot ChestSlot;

    [SerializeField] private ChestScriptableObjectList Chests;
    
    [SerializeField] private GameObject ScrollingChestPanel;
    
    [SerializeField] private int NoOfEmptySlots;
    
    private ChestSlot[] ListOfChestSlots;

    Queue<ChestSlot> WaitingQueue;
    [SerializeField] int NumOfChestCanWait;
    private int ChestWaiting ;
    [SerializeField] Text ChestsWaitingText;
    
    [HideInInspector]public bool IsChestTimerStart; 

    public static event Action OnInvalidEmptySlotEntry;
    public static event Action OnSlotsFull;
    public static event Action OnWaitingQueueFull;

    public static event Action<int,int> OnChestOpen;
    public static event Action<int> OnPressedOpeNowButton;
   
   public void InvokeOnChestOpen(int Coin,int gems)
   {
       OnChestOpen?.Invoke(Coin,gems);
   }
   
    public void InvokeOnPressedOpenNowButton(int gem)
   {
       OnPressedOpeNowButton?.Invoke(gem);
   }

   public void InvokeOnWaitingQueueFull()
   {
        OnWaitingQueueFull?.Invoke();
   }
    

   void Start()
   {  
      SetupTheChestSystem();
      CreateEmptyChestSlots();
   }

   void SetupTheChestSystem()
   {
      ChestWaiting = 0;
      ChestsWaitingText.text = ChestWaiting.ToString();
      WaitingQueue = new Queue<ChestSlot>();
      IsChestTimerStart = false; 
   }


   void CreateEmptyChestSlots()
   {
       ListOfChestSlots = new ChestSlot[NoOfEmptySlots];
     
       for(int i = 0; i<NoOfEmptySlots ; i++)
       {
            var temp =GameObject.Instantiate(ChestSlot,new Vector3(ScrollingChestPanel.transform.position.x,ScrollingChestPanel.transform.position.y,ScrollingChestPanel.transform.position.z),Quaternion.identity);
            temp.transform.SetParent(ScrollingChestPanel.transform,false);  
            ListOfChestSlots[i]= temp;
       }
   }

   public void GenerateChest()
   {
        if(NoOfEmptySlots == 0)
        {
            OnInvalidEmptySlotEntry?.Invoke();
            return;
        }

        if(!IsSlotListEmpty())
        {
            OnSlotsFull?.Invoke();
            return;
        }

        // Create Chest at empty slots
       for ( int i = 0; i<ListOfChestSlots.Length; i++)
       {
            if(ListOfChestSlots[i].GetComponent<Container>().IsContainerEmpty())
            {
                // generate random chest at this position
                GenerateRandomChest(ListOfChestSlots[i]);
                break;
            }
       }
   }

   void GenerateRandomChest(ChestSlot chest)
   {
        chest.SetChestType(Chests.chestScriptableObjects[RandomNoGenerator()]);
   }


   int RandomNoGenerator()
   {
       return UnityEngine.Random.Range(0,Chests.chestScriptableObjects.Length-1);
   }


    bool IsSlotListEmpty()
   {
       int count = 0; 

       for( int i = 0; i<ListOfChestSlots.Length; i++)
       {
           if(ListOfChestSlots[i].GetComponent<Container>().IsContainerEmpty())
           {
               break;
           }
           else
           {
               count++;
           }
       }
       if(count == ListOfChestSlots.Length)
       {
          return false;
       }
       return true;
   } 


   public bool CanAddChestToQueue()
   { 
       if(NumOfChestCanWait > 0)
       {
           return true;
       }
       return false;
   }


   public void AddChestInWaitingQueue(ChestSlot chest)
   { 
        NumOfChestCanWait --;
        ChestWaiting ++;
        ChestsWaitingText.text = ChestWaiting.ToString();
        WaitingQueue.Enqueue(chest);
   }

    public void OpenChestInsideWaitingQueue()
   {  
       if(WaitingQueue.Count != 0 && !IsChestTimerStart )
       {
           Debug.Log("Inside Waiting function");
           ChestSlot temp = WaitingQueue.Dequeue();
           temp.timerState.StartTimer();
           NumOfChestCanWait ++;
           ChestWaiting--;
           ChestsWaitingText.text = ChestWaiting.ToString();
       }
   }

}
