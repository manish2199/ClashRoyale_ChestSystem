using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : GenericSingleton<T>
{
  public static T Instance { get { return instance;} protected set { instance = value; }  }

  public static T instance; 
 
  protected void Awake()
  {
      CreateSingleton(); 
      print("CreateSingleton");
  }

   private void CreateSingleton()
  {
      if( instance == null)
      {
          instance = this as T;
          DontDestroyOnLoad(this as T);
      }
      else
      {
          Destroy(this as T);
      }
  }

}
