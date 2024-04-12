using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LOLS_EnnemyCard : MonoBehaviour
{
    [SerializeField] private Sprite spriteEnnemyCard;

    private bool isBusy = false;
    private bool doOnce = false;
    [SerializeField] private int cardIndex;
    private GameObject cacheEnemy;
    [SerializeField] private List<Sprite> otherMeshSprites = new List<Sprite>();

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    public void SetBusy(bool busy)
    {
        isBusy = busy;
    }

    public bool GetBusy()
    {
        return isBusy;
    }

    public void SetDoOnce(bool _doOnce)
    {
        doOnce = _doOnce;
    }

    public bool GetDoOnce()
    {
        return doOnce;
    }

    private void SetMeshMaterial(LOLS_EnnemyCard _card, string _enemyTag)
    {
        switch (_enemyTag)
        {  
            default: _card.GetComponent<Image>().sprite = otherMeshSprites[2];
                     _card.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(_card.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, -32.70001f);
                     _card.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(_card.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x, -32.70001f);
            break;
            case "Mannequin": _card.GetComponent<Image>().sprite = otherMeshSprites[0];
                              _card.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(_card.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, -32.70001f);
                              _card.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(_card.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x, -32.70001f);
            break;
            case "Boss": _card.GetComponent<Image>().sprite = otherMeshSprites[1]; 
                         //_card.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y = -340f;
                         _card.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(_card.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, -350f);
                         _card.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(_card.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x, -350f);
            break;
        }
    }

    public void EnemyCardEnterCombat(GameObject _enemy)
    {
        if(_enemy != cacheEnemy)
        {
            if (doOnce == false)
            {
                doOnce = true;
                isBusy = true;
                setNewOwner(_enemy);

               
                if (cardIndex == 0)
                {
                    animator.SetTrigger("EnterCombat1");
                    SetMeshMaterial(this, _enemy.tag);
                }
                else
                {
                    foreach (GameObject card in GameObject.FindGameObjectsWithTag("EnemyCard"))
                    {
                        LOLS_EnnemyCard cardRef = card.GetComponent<LOLS_EnnemyCard>();
                        if (cardRef != this)
                        {
                            if (!cardRef.GetBusy())
                            {
                                doOnce = false;
                                isBusy = false;
                                cardRef.SetBusy(true);
                                cardRef.doOnce = true;
                                cardRef.setNewOwner(cacheEnemy);
                                animator.SetTrigger("EnterCombat1");
                                SetMeshMaterial(cardRef, _enemy.tag);
                                _enemy.GetComponent<LOLS_Enemy>().SetCardRef(card.GetComponent<LOLS_EnnemyCard>());
                            }
                            else
                            {
                                doOnce = false;
                                SetMeshMaterial(this, _enemy.tag);
                                setNewOwner(cacheEnemy);
                                animator.SetTrigger("EnterCombat2");
                                _enemy.GetComponent<LOLS_Enemy>().SetCardRef(card.GetComponent<LOLS_EnnemyCard>());
                            }
                        }
                    }

                }
            }
        }
    }

    public void setNewOwner(GameObject _enemy)
    {
        cacheEnemy = _enemy;
    }

    public void EnemyAttack()
    {
        if (cardIndex == 0)
        {
            animator.SetTrigger("Attack1");
        }
        else
        {
            animator.SetTrigger("Attack2");
        }
    }

    public void EnemyCardExitCombat()
    {
            doOnce = false;
            isBusy = false;
            if (cardIndex == 0)
            {
                animator.SetTrigger("ExitCombat1");
            }
            else
            {
                animator.SetTrigger("ExitCombat2");
            }
    }
}
