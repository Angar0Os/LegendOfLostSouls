using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class LOLS_PassiveSkills : MonoBehaviour
{
    public void Heal()
    {
        this.gameObject.GetComponent<LOLS_Character>().Character.Stats.Health += 0.5f * Time.deltaTime;
    } 
    
    public IEnumerator Bravery(int _basedefense)
    {
        this.gameObject.GetComponent<LOLS_Character>().Character.Stats.Defense += 5;
        yield return new WaitForSeconds(30f);
        this.gameObject.GetComponent<LOLS_Character>().Character.Stats.Defense = _basedefense;
    }

    public bool LockPickingBoost()
    {
        return this.gameObject.GetComponent<LOLS_Character>().Character.Playerclass == LOLS_Character.E_characterClasses.Assassin;
    }

}
