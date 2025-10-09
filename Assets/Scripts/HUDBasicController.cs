using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDBasicController : MonoBehaviour
{
    public GameObject inventoryOverlayObject;
    public bool inventoryOverlayStatus = false;
    public GameObject questOverlayObject;
    public bool questOverlayStatus = false;
    public GameObject[] inventoryList;
    public readonly string shopSceneName = "Shop";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // matiin semua inventory list
        HideAll();
    }

    public void Inventory()
    {
        inventoryOverlayStatus = !inventoryOverlayStatus;
        inventoryOverlayObject.SetActive(inventoryOverlayStatus);
    }

    public void Quest()
    {
        questOverlayStatus = !questOverlayStatus;
        questOverlayObject.SetActive(questOverlayStatus);
    }

    public void Shop()
    {
        SceneManager.LoadScene(shopSceneName);
    }

    private void HideAll()
    {
        foreach (var inventory in inventoryList)
        {
            inventory.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // nyalain semua inventory yang ada dan show gambar beserta jumlahnya
        int initiator = 0;
        foreach (var inventory in DataPersistence.instance.inventory)
        {
            // nyalain
            inventoryList[initiator].SetActive(true);
            // set gambarnya
            inventoryList[initiator].GetComponent<Image>().sprite = inventory.Key.itemIcon;
            // set jumlahnya
            inventoryList[initiator].GetComponentInChildren<TextMeshProUGUI>().text = $"{inventory.Value}";
            // jangan lupa ditambahin
            initiator++;
        }
    }
}
