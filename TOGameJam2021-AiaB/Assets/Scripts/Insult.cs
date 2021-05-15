using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InsultDamageType
{
    NONE,
    RED,
    YELLOW,
    GREEN,
    NUM_DAMAGE_TYPES
}

public class Insult : MonoBehaviour
{
    float m_fInsultDamage = 5.0f;
    [SerializeField]
    InsultDamageType m_eDamageType = 0;

    GameObject m_gAssaultedActor = null;

    // Start is called before the first frame update
    void Start()
    {
        if(m_eDamageType == 0)
        {
            m_eDamageType = (InsultDamageType)Random.Range(1, 4);
        }
        switch(m_eDamageType)
        {
            case InsultDamageType.GREEN:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case InsultDamageType.RED:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case InsultDamageType.YELLOW:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        m_gAssaultedActor = GameManager.Instance.GetCurrentAudition();
        m_gAssaultedActor.GetComponent<CastMember>().TakeDamage(m_fInsultDamage, m_eDamageType);
        Destroy(this.gameObject);
    }
}
