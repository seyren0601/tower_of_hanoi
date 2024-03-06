using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckValue : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TMP_InputField inputField;
    public void CheckValueInput()
    {
        string input = inputField.text;
        if (int.TryParse(input, out int intValue))
        {
            if (intValue >= 3 && intValue <= 10)
            {
                //test 
               /* resultText.text = "Valid input";
                resultText.color = Color.blue;*/

                // If condition correct -> scene choose state 
                SceneManager.LoadSceneAsync(2);
            }
            else
            {
                resultText.text = "Not valid input value, value >=3 or <=10!";
                resultText.color = Color.red;
            }
        }
        else
        {
            resultText.text = "Not valid input, please enter a number";
            resultText.color = Color.red;
        }
    }
}

