using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    // reference ke text coin nya
    public TextMeshProUGUI textCoin;
    public readonly string hudSceneName = "HUD";
    public GameObject[] itemlist;
    // reference ke animator gameobject pop up nya
    public Animator anim;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.instance.OnItemBought += UpdateCoinUI;
        EventManager.instance.OnFullTypeItem += FullItemTypePopUp;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventManager.instance.OnItemBought -= UpdateCoinUI;
        EventManager.instance.OnFullTypeItem -= FullItemTypePopUp;
    }

    public void Back()
    {
        SceneManager.LoadScene(hudSceneName);
    }

    public void SetItemActive(int index)
    {
        // set inactive semua
        for (int i = 0; i < itemlist.Length; i++)
        {
            itemlist[i].SetActive(false);
        }

        // set aktif cuma yang index
        itemlist[index].SetActive(true);
    }

    public void ItemBuy(ScriptableObjectItem item)
    {
        // ketika beli item, maka panggil fungsi itembought di event manager
        EventManager.instance.ItemBought(item);
    }

    public void UpdateCoinUI(ScriptableObjectItem item)
    {
        DataPersistence.instance.gold -= item.price;
        textCoin.text = $"{DataPersistence.instance.gold}";
    }

    public void FullItemTypePopUp()
    {
        StartCoroutine(PopUp());
    }

    private IEnumerator PopUp()
    {
        anim.Play("show");
        yield return new WaitForSeconds(5f);
        anim.Play("hide");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        textCoin.text = $"{DataPersistence.instance.gold}";
    }
}
