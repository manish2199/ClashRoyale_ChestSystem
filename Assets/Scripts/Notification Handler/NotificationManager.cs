using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{ 
   
   [SerializeField] GameObject InvalidSlotsWarningPanel;
   
   [SerializeField] GameObject SlotsFullWarningPanel;

   [SerializeField] GameObject WaitingQueueWarningPanel;

   [SerializeField] GameObject InsufficientGemWarningPanel;



   void OnEnable()
   {
       ChestService.OnInvalidEmptySlotEntry += ShowInvalidSlotsEntryWarning;
       ChestService.OnSlotsFull += ShowSlotsFullWarning;
       ChestService.OnWaitingQueueFull += ShowWaitingQueueWarning; 
       PlayerService.OnInsufficientGems += ShowInsufficientWarningWarningPanel;
   }

   void OnDisable()
   {
       ChestService.OnInvalidEmptySlotEntry -= ShowInvalidSlotsEntryWarning;
       ChestService.OnSlotsFull -= ShowSlotsFullWarning;
       ChestService.OnWaitingQueueFull -= ShowWaitingQueueWarning;
       PlayerService.OnInsufficientGems -= ShowInsufficientWarningWarningPanel;
   }

   void ShowInsufficientWarningWarningPanel()
   {
       StartCoroutine(InsufficientGemsWarning());
   }

             
    IEnumerator InsufficientGemsWarning()
    {
       InsufficientGemWarningPanel.SetActive(true);

       yield return new WaitForSeconds(1f);

       InsufficientGemWarningPanel.SetActive(false);
    }

    void ShowWaitingQueueWarning()
   {
       StartCoroutine(WaitingQueueWarning());
   }

             
    IEnumerator WaitingQueueWarning()
    {
       WaitingQueueWarningPanel.SetActive(true);

       yield return new WaitForSeconds(1f);

       WaitingQueueWarningPanel.SetActive(false);
    }


    void ShowSlotsFullWarning()
   {
       StartCoroutine(ShowSlotFullWarning());
   }

             
    IEnumerator ShowSlotFullWarning()
    {
       SlotsFullWarningPanel.SetActive(true);

       yield return new WaitForSeconds(1f);

       SlotsFullWarningPanel.SetActive(false);
    }


   void ShowInvalidSlotsEntryWarning()
   {
       StartCoroutine(ShowInvalidSlotsEntry());
   }

             
    IEnumerator ShowInvalidSlotsEntry()
    {
       InvalidSlotsWarningPanel.SetActive(true);

       yield return new WaitForSeconds(1f);

       InvalidSlotsWarningPanel.SetActive(false);
    }
    
}
