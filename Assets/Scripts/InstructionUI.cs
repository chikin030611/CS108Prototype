using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionUI : MonoBehaviour
{
    private String _instruction = "Instruction:\n< > to move\n^ to jump\n(press again to double jump)\nx to use sword\n" +
                                  "c to use shuriken\ns to use fire ninjutsu\nd to use ice ninjutsu";
    private String _tab = "Press TAB to open/close instructions\n";
    private TextMeshProUGUI textObject;
    
    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        textObject.text = _tab;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (textObject.text == _tab)
            {
                textObject.text += _instruction;
            }
            else
            {
                textObject.text = _tab;
            }
        }
    }
}
