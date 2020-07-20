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
    bool Exist = false;

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
        while ((Random[i] == Random[0] && i != 0) || (Random[i] == Random[1] && i != 1) || (Random[i] == Random[2] && i != 2)) {
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
          Debug.LogFormat("[RGB Sequences #{0}] You submitted the right LED. Module disarmed.", moduleId);
          StartCoroutine(YouSolvedItYay());
        }
        else if (LED == LEDses[i] && i == (((Random[0] + 1) * (Random[1] + 1) * (Random[2] + 1)) % 10) && ((Vowel == true && Consonant == false) || (Vowel == false && Consonant == true) || (Vowel == false && Consonant == false))) {
          GetComponent<KMBombModule>().HandlePass();
          Debug.LogFormat("[RGB Sequences #{0}] You submitted the right LED. Module disarmed.", moduleId);
          StartCoroutine(YouSolvedItYay());
        }
        else if (LED == LEDses[i]) {
          GetComponent<KMBombModule>().HandleStrike();
          Debug.LogFormat("[RGB Sequences #{0}] You submitted LED {1}. Wrong! Unacceptable!", moduleId, i);
        }
      }
    }

    IEnumerator YouSolvedItYay() {
      while (true) {
        if (Exist == true) {
          Colores[1].GetComponent<MeshRenderer>().material = Things[2];
          yield return new WaitForSeconds(.05F);
          Colores[2].GetComponent<MeshRenderer>().material = Things[2];
          yield return new WaitForSeconds(.05F);
          goto FuckYouWhore;
        }
        Colores[0].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[1].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[2].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        FuckYouWhore:
        Colores[0].GetComponent<MeshRenderer>().material = Things[0];
        Colores[3].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[1].GetComponent<MeshRenderer>().material = Things[0];
        Colores[4].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[2].GetComponent<MeshRenderer>().material = Things[0];
        Colores[5].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[3].GetComponent<MeshRenderer>().material = Things[0];
        Colores[6].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[4].GetComponent<MeshRenderer>().material = Things[0];
        Colores[7].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[5].GetComponent<MeshRenderer>().material = Things[0];
        Colores[8].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[6].GetComponent<MeshRenderer>().material = Things[0];
        Colores[9].GetComponent<MeshRenderer>().material = Things[2];
        Exist = true;
        yield return new WaitForSeconds(.05F);
        Colores[7].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[8].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[8].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[7].GetComponent<MeshRenderer>().material = Things[2];
        yield return new WaitForSeconds(.05F);
        Colores[6].GetComponent<MeshRenderer>().material = Things[2];
        Colores[9].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[5].GetComponent<MeshRenderer>().material = Things[2];
        Colores[8].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[4].GetComponent<MeshRenderer>().material = Things[2];
        Colores[7].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[3].GetComponent<MeshRenderer>().material = Things[2];
        Colores[6].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[2].GetComponent<MeshRenderer>().material = Things[2];
        Colores[5].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[1].GetComponent<MeshRenderer>().material = Things[2];
        Colores[4].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[0].GetComponent<MeshRenderer>().material = Things[2];
        Colores[3].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[2].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
        Colores[1].GetComponent<MeshRenderer>().material = Things[0];
        yield return new WaitForSeconds(.05F);
      }
    }
    //U addw the tiwirub cplay
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} # to press that corresponding LED.";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command){
      if (Regex.IsMatch(command, @"^\s*0\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[0].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*1\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[1].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*2\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[2].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*3\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[3].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*4\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[4].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*5\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[5].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*6\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[6].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*7\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[7].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*8\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[8].OnInteract();
        yield break;
      }
      else if (Regex.IsMatch(command, @"^\s*9\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
        yield return null;
        LEDses[9].OnInteract();
        yield break;
      }
      else {
        yield return "sendtochaterror Not a valid command!";
        yield break;
      }
    }
}
