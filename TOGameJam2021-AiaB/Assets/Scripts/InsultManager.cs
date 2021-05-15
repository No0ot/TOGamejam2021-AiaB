using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultManager : MonoBehaviour
{
    private static InsultManager instance;
    public static InsultManager Instance { get { return instance; } }

    public GameObject InsultPrefab;
    public int InsultAmount = 20;

     List<GameObject> m_InsultList;
    public List<GameObject> m_CurrentHand;

    [SerializeField]
    Vector2[] CardPosition;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;

        m_InsultList = new List<GameObject>(InsultAmount);

        for(int i = 0; i < InsultAmount; i++)
        {
            GameObject newInsult = Instantiate(InsultPrefab);

            newInsult.SetActive(false);
            m_InsultList.Add(newInsult);
        }
    }
    private void Start()
    {
        PickupInsults();
        DisplayHand();
    }

    public void PickupInsults()
    {
        for(int i = 0; i < 5; i++)
        {
            int r = Random.Range(0, m_InsultList.Count);
            m_CurrentHand.Add(m_InsultList[r]);
            m_InsultList.RemoveAt(r);
        }
    }

    public void DisplayHand()
    {
        for(int i = 0; i < 5; i++)
        {
            m_CurrentHand[i].SetActive(true);
            m_CurrentHand[i].transform.position = CardPosition[i];
        }
    }
}
