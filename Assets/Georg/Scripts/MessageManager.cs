using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    TextMeshProUGUI messageText;

    public static MessageManager _instance;
    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        messageText = GameObject.Find("MessageText").GetComponent<TextMeshProUGUI>();

    }

    public IEnumerator DisplayMessages(float precursorTime, List<(string,float)> timedMessages)
    {
        yield return new WaitForSeconds(precursorTime);

        foreach((string,float) timedMessage in timedMessages)
        {
            messageText.text = timedMessage.Item1;
            yield return new WaitForSeconds(timedMessage.Item2);
        }
        messageText.text = "";
    }
}
