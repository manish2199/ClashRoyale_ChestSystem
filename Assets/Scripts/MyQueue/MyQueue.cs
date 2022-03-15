using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQueue<T> 
{
    private T[] queue;
    private int front;
    private int rear;
    private int count;
  
    
    public MyQueue(int size)
    {
        queue = new T[size];
        front = -1;
        rear = -1;
        count = 0;
    }
   
    
    public void enqueue(T element)
    {
       if(IsFull())
       {
           Debug.Log("Error : Queue is Full");
           return;
       }

        if(front == -1)
        {
          front = rear = 0;
          queue[front] = element;
        }
        else
        {
            rear ++;
            queue[rear] = element;
        }

        count++;
    }

    public T dequeue()
    { 
        if(IsEmpty())
        {
           Debug.Log("Error : Queue is Empty");
        
           return default(T) ;
        }

        T temp = queue[front];
        front++;
        if(front > rear)
        {
            resetQueue();
        }
        count--;

        return temp;
    }

    void resetQueue()
    {
        front = rear = -1;
    }

    public bool IsEmpty()
    {
       if(front > rear)
       {
          return true;
       }
      return false;
    }


    public bool IsFull()
    {
       if(rear == 100)
       {
           return true;
       }
       return false;
    }

    public int getCount()
    {
         return count;
    }

    public T GetFront()
    {
       return queue[front];
    }

    public T GetRear()
    {
       return queue[rear];
    }
}
