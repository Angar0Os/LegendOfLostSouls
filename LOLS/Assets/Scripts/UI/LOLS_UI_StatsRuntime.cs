using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LOLS_UI_StatsRuntime : MonoBehaviour
{
    public void SetCardHealthValue(float _currentHealth, GameObject _currentCard)
    {
        _currentCard.transform.GetChild(0).GetComponent<Text>().text = _currentHealth.ToString();
    }

    public void SetCardDamagesValue(float _currentDamages, GameObject _currentCard)
    {
        _currentCard.transform.GetChild(1).GetComponent<Text>().text = _currentDamages.ToString();
    }

    public void SetCardLevel(int _currentLevel, GameObject _currentCard)
    {
        _currentCard.transform.GetChild(2).GetComponent<Text>().text = _currentLevel.ToString();
    }
}
