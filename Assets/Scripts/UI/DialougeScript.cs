using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialougeScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public string[] stringArray;
    private bool running = false;
    int i = 0;

    void Start()
    {
        nextLine();
    }

    public void nextLine()
    {
        if (i <= stringArray.Length - 1 && !running)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }else{
            i = 0;
        }
    }

    private IEnumerator TextVisible()
    {   
        running = true;
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                i += 1;
                break;
            }

            counter += 1;
            yield return null;


        }  
        running = false;
    }
}
