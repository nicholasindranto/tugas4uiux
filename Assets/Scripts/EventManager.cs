using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // singleton
    public static EventManager instance;
    // event buat buy di shop
    public event Action<ScriptableObjectItem> OnItemBought;
    public event Action OnFullTypeItem;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // ada instance lain, buang yang baru
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void ItemBought(ScriptableObjectItem item)
    {
        // disini dia nge invoke semua fungsi / gameobject yang mendengarkan
        // ke dia
        OnItemBought?.Invoke(item);
    }

    public void FullTypeItem()
    {
        // kalo udah full, maka kasih pop up full nya bang
        OnFullTypeItem?.Invoke();
    }
}
