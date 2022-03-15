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


    Queue<ChestController> WaitingQueue;
    [SerializeField] int NumOfChestCanWait;
    private int ChestWaiting ;
    [SerializeField] Text ChestsWaitingText;
    

    public static event Action OnInvalidEmptySlotEntry;
    public static event Action OnSlotsFull;
    public static event Action OnWaitingQueueFull;

   void OnEnable()
   {
       ChestController.OnChestOpen += OpenChestInsideWaitingQueue;
   }

   void OnDisable()
   {
       ChestController.OnChestOpen -= OpenChestInsideWaitingQueue;
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
      WaitingQueue = new Queue<ChestController>();
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


   public bool CanAddChestToQueue()
   { 
       if(NumOfChestCanWait > 0)
       {
           return true;
       }
       return false;
   }


   public void AddChestInWaitingQueue(ChestController chestController)
   { 
        NumOfChestCanWait --;
        ChestWaiting ++;
        ChestsWaitingText.text = ChestWaiting.ToString();
        WaitingQueue.Enqueue(chestController);
   }

    void OpenChestInsideWaitingQueue(int temp1, int temp2)
   {
       if(WaitingQueue.Count != 0)
       {
           ChestController temp = WaitingQueue.Dequeue();
           temp.StartTimer();
           NumOfChestCanWait ++;
           ChestWaiting--;
           ChestsWaitingText.text = ChestWaiting.ToString();
       }
   }

}


