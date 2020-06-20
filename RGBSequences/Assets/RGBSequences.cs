using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class RGBSequences : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public Material[] Things;
    public GameObject[] Colores;
    public KMSelectable[] LEDses;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    private string[] ColorSequence = {"RKBKGBGRKG","BBGKKRKRGB","KRKGGGRRRB","RGGBKKKGRG","RRKBRBGBGK","GKBRRKBBBG","RBRBGRBKGK","GRKBBBRRBG","KBRRBRGKKB","BBGBRBRBBR"};
    int[] Random = {0,0,0};
    int NumberVenn = 0;
    string ColorVenn = "KRGYBMCW";
    string StringOne = "";
    string StringTwo = "";
    string StringThree = "";
    string StringFour = "";
    string SN = "";
    bool Vowel = false;
    bool Consonant = false;

    void Awake () {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable LED in LEDses) {
            LED.OnInteract += delegate () { LEDPress(LED); return false; };
        }
    }

    void Start () {
      SN = Bomb.GetSerialNumber();
      for (int i = 0; i < 6; i++) {
        if (SN[i].ToString() == "A" || SN[i].ToString() == "E" || SN[i].ToString() == "I" || SN[i].ToString() == "O" || SN[i].ToString() == "U") {
          Vowel = true;
        }
        else {
          Consonant = true;
        }
      }
      for (int i = 0; i < 3; i++) {
        Random[i] = UnityEngine.Random.Range(0,10);
        while (Random[0] == Random[i] && i != 0) {
          Random[i] = UnityEngine.Random.Range(0,10);
        }
        while (Random[1] == Random[i] && i != 1) {
          Random[i] = UnityEngine.Random.Range(0,10);
        }
        while (Random[2] == Random[i] && i != 2) {
          Random[i] = UnityEngine.Random.Range(0,10);
        }
      }
      StringOne = ColorSequence[Random[0]];
      StringTwo = ColorSequence[Random[1]];
      StringThree = ColorSequence[Random[2]];
      for (int i = 0; i < 10; i++) {
        if (StringOne[i] == 'R' || StringTwo[i] == 'R' || StringThree[i] == 'R') {
          NumberVenn += 1;
        }
        if (StringOne[i] == 'G' || StringTwo[i] == 'G' || StringThree[i] == 'G') {
          NumberVenn += 2;
        }
        if (StringOne[i] == 'B' || StringTwo[i] == 'B' || StringThree[i] == 'B') {
          NumberVenn += 4;
        }
        StringFour += ColorVenn[NumberVenn].ToString();
        Colores[i].GetComponent<MeshRenderer>().material = Things[NumberVenn];
        NumberVenn = 0;
      }
      Debug.LogFormat("[RGB Sequences #{0}] The chosen sequences are {1}, {2}, and {3} to make {4}.", moduleId, Random[0] + 1, Random[1] + 1, Random[2] + 1, StringFour);
      if (Vowel == true && Consonant == true) {
        Debug.LogFormat("[RGB Sequences #{0}] You should submit LED {1}.", moduleId, (((((Random[0] + 1) * (Random[1] + 1) * (Random[2] + 1)) - 1) % 9) + 1));
      }
      else {
        Debug.LogFormat("[RGB Sequences #{0}] You should submit LED {1}.", moduleId, (((Random[0] + 1) * (Random[1] + 1) * (Random[2] + 1)) % 10));
      }
    }

    void LEDPress (KMSelectable LED) {
      for (int i = 0; i < 10; i++) {
        if (LED == LEDses[i] && i == (((((Random[0] + 1) * (Random[1] + 1) * (Random[2] + 1)) - 1) % 9) + 1) && Vowel == true && Consonant == true) {
          GetComponent<KMBombModule>().HandlePass();
        }
        else if (LED == LEDses[i] && i == (((Random[0] + 1) * (Random[1] + 1) * (Random[2] + 1)) % 10) && ((Vowel == true && Consonant == false) || (Vowel == false && Consonant == true) || (Vowel == false && Consonant == false))) {
          GetComponent<KMBombModule>().HandlePass();
        }
        else if (LED == LEDses[i]) {
          GetComponent<KMBombModule>().HandleStrike();
        }
      }
    }
}
