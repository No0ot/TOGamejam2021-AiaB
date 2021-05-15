using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastMember : MonoBehaviour
{
    float m_fMaxConfidence = 10.0f;
    float m_fCurrentConfidence;

    [SerializeField]
    GameObject ConfidenceMeter;
    [SerializeField]
    public int[] m_ActingEffectiveness;
    [SerializeField]
    public int[] m_CostumeEffectiveness;
    [SerializeField]
    public int[] m_InterestEffectiveness;
    [SerializeField]
    Sprite m_Headshot;
    [SerializeField]
    string m_CharacterDescription;

    string m_CharacterName;

    int m_NumofAttacks;

    // Start is called before the first frame update
    void Start()
    {
        m_CharacterName = gameObject.name;
        m_fCurrentConfidence = m_fMaxConfidence;
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
            case InsultDamageType.NO_EFFECT:
                m_fCurrentConfidence -= damagevalue * 0.0f;
                    break;
            case InsultDamageType.MILDLY_EFFECTIVE:
                m_fCurrentConfidence -= damagevalue * 0.5f;
                break;
            case InsultDamageType.EFFECTIVE:
                m_fCurrentConfidence -= damagevalue;
                break;
            case InsultDamageType.SUPER_EFFECTIVE:
                m_fCurrentConfidence -= damagevalue * 2.0f;
                break;
            default:
                break;
        }
        Vector3 ConfidencePercentage = new Vector3(m_fCurrentConfidence / m_fMaxConfidence, 1.0f,1.0f);
        ConfidenceMeter.transform.localScale = ConfidencePercentage;
        m_NumofAttacks++;
    }

    void KillSelf()
    {
        GameManager.Instance.OnEliminate(this);
        //Removes itself from the gamemanagers list
    }
}
