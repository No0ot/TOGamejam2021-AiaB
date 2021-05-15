using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public TMP_Text m_tInsultText;

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
}
