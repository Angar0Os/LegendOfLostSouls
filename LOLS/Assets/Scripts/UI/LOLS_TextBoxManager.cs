using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LOLS_TextBoxManager : MonoBehaviour
{
    public int maxMessages = 10;

    public GameObject ChatPanel;
    public GameObject ScrollViewObject;
    public GameObject TextObject;
    private IEnumerator coroutine;
    private bool coroutineIsSet = false;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SendMessage("Bonsoir");                  
        }
    }

    void Start()
    {

    }

    public void SendMessage(string text)
    {
        ScrollViewObject.SetActive(true); //GetComponent<Image>().color = new Color(36,36,36,128);

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(TextObject, ChatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        messageList.Add(newMessage);
    }

    public void RemoveMessages()
    {
        if (ChatPanel.transform.childCount > 0)
        {
            foreach (Transform textAffiche in ChatPanel.transform)
            {
                //Debug.Log(textAffiche.gameObject.name);
                Destroy(textAffiche.gameObject);
            }
        }


        if (messageList.Count != 0)
        {
            foreach (var textAffiche in messageList)
            {
                Destroy(textAffiche.textObject);
            }
        }
    }
}



[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
