using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionUI : MonoBehaviour
{
    private String _instruction = "<color=#FFC900>< ></color> to move\n" +
                                  "<color=#FFC900>^</color> to jump\n" +
                                  "(press again to double jump)\n" +
                                  "<color=#FFC900>X</color> to use sword\n" +
                                  "<color=#FFC900>C</color> to use shuriken\n" +
                                  "<color=#FFC900>Ki is needed to use ninjutsu</color>\n" +
                                  "<color=#FFC900>S</color> to use fire ninjutsu\n" +
                                  "<color=#FFC900>D</color> to use ice ninjutsu";
    
    private String _tab = "Press <color=#FFC900>TAB</color> to open/close instructions\n";
    private TextMeshProUGUI textObject;
    
    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        textObject.text = _tab + _instruction;
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
