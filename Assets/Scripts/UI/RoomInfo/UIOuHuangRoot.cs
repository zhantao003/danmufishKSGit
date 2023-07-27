using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOuHuangRoot : MonoBehaviour
{
    public GameObject objSettingRoot;
    public GameObject objShowRoot;
    public GameObject objInputRoot;

    public GameObject objSettingBtn;
    public GameObject objCancelBtn;
    public GameObject objChgBtn;

    public InputField uiQuestInput;

    public Text uiShowQuest;

    //public List<>

    public void Reset()
    {
        objInputRoot.SetActive(false);
        objShowRoot.SetActive(false);
        objSettingRoot.SetActive(true);
        objSettingBtn.SetActive(true);
        objCancelBtn.SetActive(false);
        objChgBtn.SetActive(false);
    }

    public void OnClickChg()
    {
        objSettingBtn.SetActive(false);
        objCancelBtn.SetActive(true);
        objInputRoot.SetActive(true);
        objChgBtn.SetActive(false);
        objSettingRoot.SetActive(true);
        objShowRoot.SetActive(false);
    }

    public void OnClickSetQuest()
    {
        if(objInputRoot.activeSelf)
        {
            objSettingBtn.SetActive(true);
            objCancelBtn.SetActive(false);
            objInputRoot.SetActive(false);
        }
        else
        {
            objSettingBtn.SetActive(false);
            objCancelBtn.SetActive(true);
            objInputRoot.SetActive(true);
        }
    }

    public void OnClickCompeleteSet()
    {
        objInputRoot.SetActive(false);
        objSettingRoot.SetActive(false);
        objShowRoot.SetActive(true);
        uiShowQuest.text = uiQuestInput.text;
        objCancelBtn.SetActive(false);
        objSettingBtn.SetActive(false);
        objChgBtn.SetActive(true);
    }

}
