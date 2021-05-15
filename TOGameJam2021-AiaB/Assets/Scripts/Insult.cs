using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum InsultDamageType
{
    NO_EFFECT,
    MILDLY_EFFECTIVE,
    EFFECTIVE,
    SUPER_EFFECTIVE,
    NUM_DAMAGE_TYPES
}

public class Insult : MonoBehaviour
{
    float m_fInsultDamage = 5.0f;
    [SerializeField]
    InsultDamageType m_eDamageType = 0;
    [SerializeField]
    public TMP_Text m_tInsultText;

    int m_CardNumber;

    GameObject m_gAssaultedActor = null;

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
        switch (m_eDamageType)
        {
            case InsultDamageType.NO_EFFECT:
                GetComponent<SpriteRenderer>().color = Color.grey;
                break;
            case InsultDamageType.MILDLY_EFFECTIVE:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case InsultDamageType.EFFECTIVE:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case InsultDamageType.SUPER_EFFECTIVE:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            default:
                break;
        }
    }

    private void OnMouseDown()
    {
        m_gAssaultedActor = GameManager.Instance.GetCurrentAudition();
        m_gAssaultedActor.GetComponent<CastMember>().TakeDamage(m_fInsultDamage, m_eDamageType);
        Vector2 newposition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1.0f);
        this.gameObject.transform.position = newposition;
        gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        Vector2 newposition = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y + 1.0f);
        this.gameObject.transform.position = newposition;
    }

    private void OnMouseExit()
    {
        Vector2 newposition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1.0f);
        this.gameObject.transform.position = newposition;
    }

    public void setText(string newtext)
    {
        m_tInsultText.SetText(newtext);
    }

    public void setDamageType(int newDamageType)
    {
        m_eDamageType = (InsultDamageType)newDamageType;
    }
}
