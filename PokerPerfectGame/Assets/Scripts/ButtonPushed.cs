using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


// this script is not currently used but allows
// data to be pushed back to the server as needed
public class ButtonPushed : MonoBehaviour
{
  //Make sure to attach these Buttons in the Inspector
  public Button button1;
  public Button button2;
  public TMP_InputField input1;
  public TMP_InputField input2;
  public TMP_InputField input3;

  void Start()
  {
    button1.onClick.AddListener(TaskOnClick);
    button2.onClick.AddListener(delegate { TaskWithParameters("Hello"); });
  }

  void TaskOnClick()
  {
    Contact contact = new Contact();

    contact.name = input1.text;
    contact.email = input2.text;
    contact.phone = input3.text;

    StartCoroutine(ProcessRequest("https://localhost:7038/api/contacts", contact));
  }

  void TaskWithParameters(string message)
  {
    Debug.Log(message);
  }

  IEnumerator ProcessRequest(string url, Contact contact)
  {
    string json = JsonUtility.ToJson(contact);

    UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, json, "application/json");

    yield return unityWebRequest.SendWebRequest();

    if (unityWebRequest.result != UnityWebRequest.Result.Success)
      Debug.Log(unityWebRequest.error);
    else
      Debug.Log("Form upload complete!");
  }

}