using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text ProfileDescription;
    [SerializeField] Sprite Headshot;
    [SerializeField] Text ProfileName;

    public void ChangeHeadshot(Sprite newImage)
    {
        Headshot = newImage;
    }

    public void ChangeDescription(string newDescription)
    {
        ProfileDescription.text = newDescription;
    }

    public void ChangeName( string newName)
    {
        ProfileName.text = newName;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
