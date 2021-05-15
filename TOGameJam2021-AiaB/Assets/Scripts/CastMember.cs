using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InsultArmorType
{
    NONE,
    RED,
    YELLOW,
    GREEN,
    NUM_ARMOR_TYPES
}


public class CastMember : MonoBehaviour
{
    float m_fMaxConfidence = 10.0f;
    float m_fCurrentConfidence;
    [SerializeField]
    InsultArmorType m_eArmor;
    [SerializeField]
    GameObject ConfidenceMeter;

    // Start is called before the first frame update
    void Start()
    {
        m_fCurrentConfidence = m_fMaxConfidence;

        if (m_eArmor == 0)
        {
            m_eArmor = (InsultArmorType)Random.Range(1, 3);
        }
        switch (m_eArmor)
        {
            case InsultArmorType.GREEN:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case InsultArmorType.RED:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case InsultArmorType.YELLOW:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (m_fCurrentConfidence <= 0)
            {
                ConfidenceMeter.SetActive(false);
                KillSelf();
            }
        }
    }

    public void TakeDamage(float damagevalue, InsultDamageType damagetype)
    {
        switch(damagetype)
        {
            case InsultDamageType.RED:
                if(m_eArmor == InsultArmorType.RED)
                {

                    m_fCurrentConfidence -= damagevalue;
                    Debug.Log("RED ON RED");

                } else if(m_eArmor == InsultArmorType.YELLOW)
                {
                    m_fCurrentConfidence -= (damagevalue * 0.5f);
                    Debug.Log("RED ON YELLOW");

                } else if (m_eArmor == InsultArmorType.GREEN)
                {
                    m_fCurrentConfidence -= (damagevalue * 2.0f);
                    Debug.Log("RED ON GREEN");

                }
                    break;
            case InsultDamageType.YELLOW:
                if (m_eArmor == InsultArmorType.RED)
                {
                    m_fCurrentConfidence -= (damagevalue * 2.0f);
                    Debug.Log("YELLOW ON RED");
                }
                else if (m_eArmor == InsultArmorType.YELLOW)
                {
                    m_fCurrentConfidence -= damagevalue;
                    Debug.Log("YELLOW ON YELLOW");
                }
                else if (m_eArmor == InsultArmorType.GREEN)
                {
                    m_fCurrentConfidence -= (damagevalue * 0.5f);
                    Debug.Log("YELLOW ON GREEN");
                }
                break;
            case InsultDamageType.GREEN:
                if (m_eArmor == InsultArmorType.RED)
                {
                    m_fCurrentConfidence -= (damagevalue * 0.5f);
                    Debug.Log("GREEN ON RED");
                }
                else if (m_eArmor == InsultArmorType.YELLOW)
                {
                    m_fCurrentConfidence -= (damagevalue * 2.0f);
                    Debug.Log("GREEN ON YELLOW");
                }
                else if (m_eArmor == InsultArmorType.GREEN)
                {
                    m_fCurrentConfidence -= damagevalue;
                    Debug.Log("GREEN ON GREEN");
                }
                break;
            default:
                break;
        }
        Vector3 ConfidencePercentage = new Vector3(m_fCurrentConfidence / m_fMaxConfidence, 1.0f,1.0f);
        ConfidenceMeter.transform.localScale = ConfidencePercentage;
    }

    void KillSelf()
    {
        GameManager.Instance.OnEliminate(this);
        //Removes itself from the gamemanagers list
    }
}
