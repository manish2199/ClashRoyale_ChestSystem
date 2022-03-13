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

    [SerializeField] GameObject ScrollingPanel;

    [SerializeField] int NoOfEmptySlots;

   [HideInInspector] public bool IsChestTimerStart;

   public static event Action<int,int> OnChestOpen;

   void Awake()
   {
       base.Awake();
   }

   public void InvokeOnChestOpen(int Coin , int Gem)
   { 
      OnChestOpen?.Invoke(Coin,Gem);  
   }
    

   void Start()
   {  
      IsChestTimerStart = false; 
      CreateEmptyChestSlots();
   }


   void CreateEmptyChestSlots()
   {
        ListOfChestSlots = new ChestView[NoOfEmptySlots];
     
       for(int i = 0; i<NoOfEmptySlots ; i++)
       {
            var temp =GameObject.Instantiate<ChestView>(ChestSlot,new Vector3(ScrollingPanel.transform.position.x,ScrollingPanel.transform.position.y,ScrollingPanel.transform.position.z),Quaternion.identity);
            temp.transform.SetParent(ScrollingPanel.transform,false);  
            ListOfChestSlots[i]= temp;
       }
   }

   public void GenerateChest()
   {
        if(NoOfEmptySlots == 0)
        {
            print("Not Enough Slots to add Chest");
            return;
        }

        if(!IsSlotListEmpty())
        {
            print("All Slots are full cant Add Chest");
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

}
