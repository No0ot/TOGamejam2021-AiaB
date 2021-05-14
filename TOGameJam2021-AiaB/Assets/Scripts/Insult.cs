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
    [SerializeField]
    CastMember m_gAssaultedActor;

    // Start is called before the first frame update
    void Start()
    {
        if(m_eDamageType == 0)
        {
            m_eDamageType = (InsultDamageType)Random.Range(1, 4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //m_gAssualtedActor = GameManger.Instance.GetCurrentActor()
        m_gAssaultedActor.TakeDamage(m_fInsultDamage, m_eDamageType);
        Destroy(this.gameObject);
    }
}
