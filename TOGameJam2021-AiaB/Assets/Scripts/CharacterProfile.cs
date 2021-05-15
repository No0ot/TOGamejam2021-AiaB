using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text ProfileDescription;
    [SerializeField] Image Headshot;

    public void ChangeHeadshot(Image newImage)
    {
        Headshot = newImage;
    }

    public void ChangeDescription(string newDescription)
    {
        ProfileDescription.text = newDescription;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
