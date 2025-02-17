using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

public class JsonControl : MonoBehaviour
{
  private static JsonControl instance;

  public enum DataTypes
  {
    blinds,
    contacts,
    payouts,
    tables,
    games,
    players,
    chipsets,
    chips
  }

  public static JsonControl Instance
  {
    get { return instance; }
  }

  private void Awake()
  {
    if (instance == null)
      instance = this;

    foreach (var endpoint in endpoints)
      StartCoroutine(GetData(endpoint.url.ToString(), endpoint.dataType));
  }

  private string jsonUrlRoot = "https://localhost:7038/api/";
  //private string jsonUrlRoot = "https://192.168.10.201:45455/api/";
  //  private string jsonUrlRoot = "https://10.0.2.2:7038/api/";
  //private string jsonUrlRoot = "http://10.0.2.2:5097/api/";
  //private string jsonUrlRoot = "https://tallgreenwave25.conveyor.cloud/api/";

  public List<EndpointInfo> Endpoints
  {
    get { return endpoints; }
  }

  public class EndpointInfo
  {
    public DataTypes url { get; }
    public Type dataType { get; }

    public EndpointInfo(DataTypes url, Type dataType)
    {
      this.url = url;
      this.dataType = dataType;
    }
  }

  private List<EndpointInfo> endpoints = new List<EndpointInfo>
    {
        new EndpointInfo(DataTypes.blinds, typeof(Blinds)),
        new EndpointInfo(DataTypes.contacts, typeof(Contacts)),
        new EndpointInfo(DataTypes.payouts, typeof(Payouts)),
        new EndpointInfo(DataTypes.tables, typeof(Tables)),
        new EndpointInfo(DataTypes.games, typeof(Games)),
        new EndpointInfo(DataTypes.players, typeof(Players)),
        new EndpointInfo(DataTypes.chipsets, typeof(Chipsets)),
        new EndpointInfo(DataTypes.chips, typeof(Chips))
    };

  private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
  {
    // all certificates are accepted
    return true;
  }

  IEnumerator GetData(string url, Type dataType)
  {
    ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;

    UnityWebRequest unityWebRequest = UnityWebRequest.Get(jsonUrlRoot + url + "/");

    yield return unityWebRequest.SendWebRequest();

    switch (unityWebRequest.result)
    {
      case UnityWebRequest.Result.ConnectionError:
      case UnityWebRequest.Result.DataProcessingError:
        Debug.LogError("Error: " + unityWebRequest.error);
        break;
      case UnityWebRequest.Result.ProtocolError:
        Debug.LogError("HTTP Error: " + unityWebRequest.error);
        break;
      case UnityWebRequest.Result.Success:
        Debug.Log("Received: " + unityWebRequest.downloadHandler.text);
        ProcessData(unityWebRequest.downloadHandler.text, dataType, url);
        break;
    }
  }

  private void ProcessData(string jsonString, Type dataType, string url)
  {
    // Deserialize JSON data using the provided data type
    object data = JsonUtility.FromJson($"{{\"{url}\":{jsonString}}}", dataType);

    // Store the data using some appropriate method (e.g., in a dictionary or list)
    // For example, you can use a dictionary where the URL is the key.
    Data.data[url] = data;
  }
}
