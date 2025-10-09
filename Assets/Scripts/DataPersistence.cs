using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    // singleton
    public static DataPersistence instance;

    // data datanya yang persist
    public int gold = 50000;
    public int maxJenisItem = 9;
    public Dictionary<ScriptableObjectItem, int> inventory = new Dictionary<ScriptableObjectItem, int>();

    private void OnEnable()
    {
        EventManager.instance.OnItemBought += HandleItemBought;
    }

    private void OnDisable()
    {
        EventManager.instance.OnItemBought -= HandleItemBought;
    }

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


    public void HandleItemBought(ScriptableObjectItem item)
    {
        // kalau item udah ada maka tambahin aja value dictionarynya
        if (inventory.ContainsKey(item))
        {
            // tambah valuenya
            inventory[item]++;
        }
        else
        {
            // cek masih bisa nambah jenis nggak
            if (inventory.Count >= maxJenisItem)
            {
                // udah full bang, kasih pop up full
                EventManager.instance.FullTypeItem();
                return;
            }

            inventory.Add(item, 1); // tambahin jenisnya
        }
    }
}
