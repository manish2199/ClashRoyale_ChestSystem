using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestService : GenericSingleton<ChestService>
{
    [SerializeField] ChestView ChestSlot;
    ChestView[] ListOfChestSlots;

    [SerializeField] ChestScriptableObjectList Chest;

    [SerializeField] GameObject ScrollingChestPanel;
   

    [SerializeField] int NoOfEmptySlots;

    [HideInInspector] public bool IsChestTimerStart;


    MyQueue<ChestController> WaitingQueue;
    [SerializeField] int NumOfChestCanWait;
    private int ChestWaiting ;
    [SerializeField] Text ChestsWaitingText;

    public static event Action OnInvalidEmptySlotEntry;
    public static event Action OnSlotsFull;
    public static event Action OnWaitingQueueFull;
    public static event Action OnRepeatationInQueue;


   void OnEnable()
   {
       ChestController.OnChestOpen += OpenChestInsideWaitingQueue;
   }

   void OnDisable()
   {
       ChestController.OnChestOpen -= OpenChestInsideWaitingQueue;
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
      WaitingQueue = new MyQueue<ChestController>(NumOfChestCanWait);
      IsChestTimerStart = false; 
   }


   void CreateEmptyChestSlots()
   {
       ListOfChestSlots = new ChestView[NoOfEmptySlots];
     
       for(int i = 0; i<NoOfEmptySlots ; i++)
       {
            var temp =GameObject.Instantiate<ChestView>(ChestSlot,new Vector3(ScrollingChestPanel.transform.position.x,ScrollingChestPanel.transform.position.y,ScrollingChestPanel.transform.position.z),Quaternion.identity);
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

   void GenerateRandomChest(ChestView chestView)
   {
       ChestModel chestModel = new ChestModel(Chest.chestScriptableObjects[RandomNoGenerator()]);

       ChestController chestController = new ChestController(chestModel,chestView);
   }


   int RandomNoGenerator()
   {
       return UnityEngine.Random.Range(0,Chest.chestScriptableObjects.Length-1);
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


   public void AddChestInWaitingQueue(ChestController chestController)
   { 
    //    var temp = WaitingQueue.
       if( WaitingQueue.getCount() != 0 && chestController == WaitingQueue.GetRear() )
       {
          OnRepeatationInQueue?.Invoke();
          Debug.Log("Already Added to queue ");
          return;
       }
      
       if(NumOfChestCanWait > 0)
       {
           NumOfChestCanWait --;
           ChestWaiting ++;
           ChestsWaitingText.text = ChestWaiting.ToString();
           WaitingQueue.enqueue(chestController);

       }
       else if( NumOfChestCanWait == 0)
       {
          print("Waiting Queue is Full");
          OnWaitingQueueFull?.Invoke();
       }
   }

    void OpenChestInsideWaitingQueue(int temp1, int temp2)
   {
       if(WaitingQueue.getCount() != 0)
       {
           print("Waiting Queue is not empty");
           ChestController temp = WaitingQueue.dequeue();
           temp.StartTimer();
           NumOfChestCanWait ++;
           ChestWaiting--;
           ChestsWaitingText.text = ChestWaiting.ToString();
       }
   }

}


