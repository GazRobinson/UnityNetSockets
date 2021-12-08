using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Simple text output to show changing client data values
/// 
/// Gaz Robinson, Abertay University
/// CMP303
/// </summary>
public class DataRender : MonoBehaviour
{
    [SerializeField]
    private ExampleClient m_Client;
    private Text m_Text;
    // Start is called before the first frame update
    void Start()
    {
        m_Text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Text.text = "Position: " + m_Client.dataToSend.position + "\nHealth: " + m_Client.dataToSend.health;
    }
}
