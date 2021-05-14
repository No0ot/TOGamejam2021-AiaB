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
    float m_fConfidence = 10.0f;
    [SerializeField]
    InsultArmorType m_eArmor;

    // Start is called before the first frame update
    void Start()
    {
        if (m_eArmor == 0)
        {
            m_eArmor = (InsultArmorType)Random.Range(1, 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_fConfidence <= 0)
        {
            KillSelf();
        }
    }

    public void TakeDamage(float damagevalue, InsultDamageType damagetype)
    {
        switch(damagetype)
        {
            case InsultDamageType.RED:
                if(m_eArmor == InsultArmorType.RED)
                {
                    Debug.Log("RED ON RED");
                } else if(m_eArmor == InsultArmorType.YELLOW)
                {
                    Debug.Log("RED ON YELLOW");
                } else if (m_eArmor == InsultArmorType.GREEN)
                {
                    Debug.Log("RED ON GREEN");
                }
                    break;
            case InsultDamageType.YELLOW:
                if (m_eArmor == InsultArmorType.RED)
                {
                    Debug.Log("YELLOW ON RED");
                }
                else if (m_eArmor == InsultArmorType.YELLOW)
                {
                    Debug.Log("YELLOW ON YELLOW");
                }
                else if (m_eArmor == InsultArmorType.GREEN)
                {
                    Debug.Log("YELLOW ON GREEN");
                }
                break;
            case InsultDamageType.GREEN:
                if (m_eArmor == InsultArmorType.RED)
                {
                    Debug.Log("GREEN ON RED");
                }
                else if (m_eArmor == InsultArmorType.YELLOW)
                {
                    Debug.Log("GREEN ON YELLOW");
                }
                else if (m_eArmor == InsultArmorType.GREEN)
                {
                    Debug.Log("GREEN ON GREEN");
                }
                break;
            default:
                break;
        }
    }

    void KillSelf()
    {
        //Removes itself from the gamemanagers list
    }
}
