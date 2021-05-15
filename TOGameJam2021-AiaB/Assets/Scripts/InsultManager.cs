using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultManager : MonoBehaviour
{
    private static InsultManager instance;
    public static InsultManager Instance { get { return instance; } }

    public GameObject InsultPrefab;
    public int InsultAmount = 20;

    public List<GameObject> m_InsultList;
    public List<GameObject> m_CurrentHand;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;

        m_InsultList = new List<GameObject>(InsultAmount);

        for(int i = 0; i < InsultAmount; i++)
        {
            GameObject newInsult = Instantiate(InsultPrefab);

            m_InsultList.Add(newInsult);
        }
    }

    public void PickupInsults()
    {
        for(int i = 0; i < 6; i++)
        {
            int r = Random.Range(0, m_InsultList.Count);
            m_CurrentHand.Add(m_InsultList[r]);
            m_InsultList.RemoveAt(r);
        }
    }
    public void DisplayHand()
    {

    }
}
