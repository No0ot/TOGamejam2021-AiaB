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


}
