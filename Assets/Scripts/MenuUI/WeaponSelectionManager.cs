using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionManager : MonoBehaviour
{
    [Header("Silah Verileri & Butonlar")]
    public WeaponData[] weapons;
    public Button[] weaponButtons;
    
    [Header("Oyuncu Para Bilgisi")]
    public TextMeshProUGUI coinText;
    public int playerCoins = 5000;
    public PlayerData playerData;
    [Header("Outline Spriteları")]
    public Sprite defaultOutlineSprite;
    public Sprite greenOutlineSprite;

    public Vector3 mainTransform;
    private int selectedWeaponIndex = 0;

    void Start()
    {
        mainTransform = new Vector3(0.02401501f, 0.02401501f, 0.02401501f);
        playerCoins = playerData.Coin;
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            int index = i;  
            weaponButtons[i].onClick.AddListener(() => OnWeaponButtonClicked(index));
        }

        UpdateUI();
    }
    public void OnWeaponButtonClicked(int index)
    {
        if (!weapons[index].isUnlocked)
        {
            UnlockWeapon(index);
        }
        else
        {
            SelectWeapon(index);
        }
    }

    public void SelectWeapon(int index)
    {
        if (!weapons[index].isUnlocked)
            return;

        var oldButton = weaponButtons[selectedWeaponIndex];
        oldButton.transform.DOScale(mainTransform, 0.2f);

        Image oldOutlineImage = oldButton.transform.Find("Outline").GetComponent<Image>();
        oldOutlineImage.sprite = defaultOutlineSprite;

        selectedWeaponIndex = index;
        playerData.selectedWeaponIndex=index;
        
        var newButton = weaponButtons[selectedWeaponIndex];
        newButton.transform.DOScale(mainTransform * 1.1f, 0.2f)
            .SetLoops(2, LoopType.Yoyo);
        
        Image newOutlineImage = newButton.transform.Find("Outline").GetComponent<Image>();
        newOutlineImage.sprite = greenOutlineSprite;
    }

    void ShakeLockedWeapon(int index)
    {
        weaponButtons[index].transform.DOShakePosition(0.5f, new Vector3(0.1f, 0f, 0f), 25, 5);
    }

    public void UnlockWeapon(int index)
    {
        if (weapons[index].isUnlocked)
            return;

        if (playerCoins >= weapons[index].unlockPrice)
        {
            playerData.Coin -= weapons[index].unlockPrice;
            weapons[index].isUnlocked = true;
            UpdateUI();
            SelectWeapon(index);
        }
        else
        {
            ShakeLockedWeapon(index);
        }
    }

    void UpdateUI()
    {
        coinText.text = playerData.Coin + " $";

        if (weapons.Length != weaponButtons.Length) 
            return;

        for (int i = 0; i < weapons.Length; i++)
        {
            weaponButtons[i].interactable = true;  

            if (weapons[i].isUnlocked)
            {
                weaponButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "OWNED";
            }
            else
            {
                weaponButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = weapons[i].unlockPrice + " $";
            }

            if (i == selectedWeaponIndex && weapons[i].isUnlocked)
            {
                var outlineImg = weaponButtons[i].transform.Find("Outline").GetComponent<Image>();
                outlineImg.sprite = greenOutlineSprite;
            }
            else
            {
                var outlineImg = weaponButtons[i].transform.Find("Outline").GetComponent<Image>();
                outlineImg.sprite = defaultOutlineSprite;
            }
        }
    }
}
