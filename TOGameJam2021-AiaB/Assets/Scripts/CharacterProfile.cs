using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text ProfileDescription;
    [SerializeField] Image Headshot;
    [SerializeField] Text ProfileName;

    public void ChangeHeadshot(Sprite newImage)
    {
        Headshot.sprite = newImage;
    }

    public void ChangeDescription(string newDescription)
    {
        //pasting newline characters in inspector doesnt work, these two lines format the bios properly
        newDescription = newDescription.Replace("Interests:", "\n\nInterests:");
        newDescription = newDescription.Replace("Temperament:", "\n\nTemperament:");
        ProfileDescription.text = newDescription;
    }

    public void ChangeName( string newName)
    {
        //a glitch exists where "(clone)" is appended to name for some reason
        newName = newName.Replace("(Clone)", "");
        ProfileName.text = newName;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TriggerUpdate();
    }

    public void TriggerUpdate()
    {
        GameObject audition = GameManager.Instance.GetCurrentAudition();
        ChangeHeadshot(audition.GetComponent<CastMember>().m_Headshot);
        ChangeDescription(audition.GetComponent<CastMember>().m_CharacterDescription);
        ChangeName(audition.GetComponent<CastMember>().m_CharacterName);
    }
}
