using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
   public ChestModel ChestModel { get; }

   public ChestView ChestView { get; }


   public ChestController ( ChestModel chestModel , ChestView chestView)
   { 
       this.ChestModel = chestModel;
       this.ChestView = chestView;
    
       ChestView.ChestController = this;

   }
}
