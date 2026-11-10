using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagesManager : MonoBehaviour
{
    public static MessagesManager instance;

    [SerializeField] Transform messageParent;
    [SerializeField] GameObject messagePrefab;
    [SerializeField] GameObject secondaryMessagePrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void CreateMessage (string message)
    {
        GameObject newMessage = Instantiate(messagePrefab, messageParent);

        Button messageButton = newMessage.GetComponent<Button>();
        TMP_Text label = messageButton.transform.Find("MessageLabel").GetComponent<TextMeshProUGUI>();
        label.text = message;
    }

    public void CreateMessage(string message, string secondaryMessage)
    {
        GameObject newMessage = Instantiate(secondaryMessagePrefab, messageParent);

        Button messageButton = newMessage.GetComponent<Button>();
        TMP_Text label = messageButton.transform.Find("MessageLabel").GetComponent<TextMeshProUGUI>();
        label.text = message;
        TMP_Text secondaryLabel = messageButton.transform.Find("SecondaryMessageLabel").GetComponent<TextMeshProUGUI>();
        secondaryLabel.text = secondaryMessage;
    }
}
