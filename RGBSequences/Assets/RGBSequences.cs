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
   public KMRuleSeedable RS;
   public KMColorblindMode ColorblindMode;
   public TextMesh[] CBText;
   public VoltageMeterReader VMR;
   bool cbMode = false;

   static int moduleIdCounter = 1;
   int moduleId;
   private bool moduleSolved;

   string[] ColorSequence = { "RKBKGBGRKG", "BBGKKRKRGB", "KRKGGGRRRB", "RGGBKKKGRG", "RRKBRBGBGK", "GKBRRKBBBG", "RBRBGRBKGK", "GRKBBBRRBG", "KBRRBRGKKB", "BBGBRBRBBR" };
   int[] Random = { 0, 0, 0 };
   int NumberVenn = 0;
   string ColorVenn = "KRGYBMCW";
   string Numberfucker = "0123456789";
   string StringOne = "";
   string StringTwo = "";
   string StringThree = "";
   string StringFour = "";
   string SN = "AAAAAA";
   bool Vowel = false;
   bool Consonant = false;
   bool Exist = false;
   int answer;
   int SolveAnimColor = 2;


   void Awake () {
      moduleId = moduleIdCounter++;

      foreach (KMSelectable LED in LEDses) {
         LED.OnInteract += delegate () { LEDPress(LED); return false; };
      }
      cbMode = ColorblindMode.ColorblindModeActive;
   }

   void Generate () {
      var Rng = RS.GetRNG();
      Debug.LogFormat("[RGB Sequences #{0}] Using ruleseed {1}.", moduleId, Rng.Seed);
      if (Rng.Seed == 1) {
         return;
      }
      string PossibleColors = "KRGB";
      List<string> Answers = new List<string>() { };
      string AnswerBuilder = "";
      int AnswerHelper = 0;
      int boaner_17IQ = 0;

      do {
         Answers.Clear();
         AnswerHelper = 0;
         AnswerBuilder = "";
         for (int i = 0; i < 10; i++) {
            ColorSequence[i] = "";
            for (int j = 0; j < 10; j++) {
               ColorSequence[i] += PossibleColors[Rng.Next(0, 4)];
            }
         }
         for (int i = 0; i < 8; i++) {             //Row 1
            for (int j = i + 1; j < 9; j++) {      //Row 2
               for (int k = j + 1; k < 10; k++) {   //Row 3
                  for (int l = 0; l < 10; l++) {   //Actual adding the colors
                     if (ColorSequence[i][l] == 'R' || ColorSequence[j][l] == 'R' || ColorSequence[k][l] == 'R') {
                        AnswerHelper++;
                     }
                     if (ColorSequence[i][l] == 'G' || ColorSequence[j][l] == 'G' || ColorSequence[k][l] == 'G') {
                        AnswerHelper += 2;
                     }
                     if (ColorSequence[i][l] == 'B' || ColorSequence[j][l] == 'B' || ColorSequence[k][l] == 'B') {
                        AnswerHelper += 4;
                     }
                     AnswerBuilder += ColorVenn[AnswerHelper];
                     AnswerHelper = 0;
                  }
                  Answers.Add(AnswerBuilder);
                  AnswerBuilder = "";
               }
            }
         }
      } while (Answers.Distinct().Count() != 120);
      

      Debug.LogFormat("[RGB Sequences #{0}] Your rows are:", moduleId);
      for (int i = 0; i < 10; i++) {
         Debug.LogFormat("[RGB Sequences #{0}] {1}", moduleId, ColorSequence[i]);
      }
   }

   void Start () {
      SN = Bomb.GetSerialNumber();
      for (int i = 0; i < 10; i++) {
         CBText[i].gameObject.SetActive(cbMode);
      }
      Generate();
      
      for (int i = 0; i < 6; i++) {
         if (SN[i].ToString() == "A" || SN[i].ToString() == "E" || SN[i].ToString() == "I" || SN[i].ToString() == "O" || SN[i].ToString() == "U") {
            Vowel = true;
         }
         else if (!Numberfucker.Contains(SN[i].ToString())) {
            Consonant = true;
         }
      }
      for (int i = 0; i < 3; i++) {
         Random[i] = UnityEngine.Random.Range(0, 10);
         while ((Random[i] == Random[0] && i != 0) || (Random[i] == Random[1] && i != 1) || (Random[i] == Random[2] && i != 2)) {
            Random[i] = UnityEngine.Random.Range(0, 10);
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
         CBText[i].text = ColorVenn[NumberVenn].ToString();
         NumberVenn = 0;
      }
      Debug.LogFormat("[RGB Sequences #{0}] The chosen sequences are {1}, {2}, and {3} to make {4}.", moduleId, Random[0] + 1, Random[1] + 1, Random[2] + 1, StringFour);
      if ((Vowel && Consonant) || (VMR.VoltageMeterInt() < 5 && VMR.VoltageMeterInt() != -1)) {
         answer = (((((Random[0] + 1) * (Random[1] + 1) * (Random[2] + 1)) - 1) % 9) + 1);
      }
      else {
         answer = (((Random[0] + 1) * (Random[1] + 1) * (Random[2] + 1)) % 10);
      }
      Debug.LogFormat("[RGB Sequences #{0}] You should submit LED {1}.", moduleId, answer);
   }

   void LEDPress (KMSelectable LED) {
      if (moduleSolved) {
         SolveAnimColor++;
         SolveAnimColor %= 8;
         if (SolveAnimColor == 0) {
            SolveAnimColor++;
         }
         return;
      }
      for (int i = 0; i < 10; i++) {
         if (LED == LEDses[i]) {
            if (i == answer) {
               GetComponent<KMBombModule>().HandlePass();
               Debug.LogFormat("[RGB Sequences #{0}] You submitted the right LED. Module disarmed.", moduleId);
               moduleSolved = true;
               StartCoroutine(YouSolvedItYay());
            }
            else {
               GetComponent<KMBombModule>().HandleStrike();
               Debug.LogFormat("[RGB Sequences #{0}] You submitted LED {1}. Wrong! Unacceptable!", moduleId, i);
            }
         }
      }
   }

   IEnumerator YouSolvedItYay () {
      while (true) {
         if (Exist) {
            Colores[1].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
            yield return new WaitForSeconds(.05F);
            Colores[2].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
            yield return new WaitForSeconds(.05F);
            goto FuckYouWhore;
         }
         Colores[0].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[1].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[2].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         FuckYouWhore:
         Colores[0].GetComponent<MeshRenderer>().material = Things[0];
         Colores[3].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[1].GetComponent<MeshRenderer>().material = Things[0];
         Colores[4].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[2].GetComponent<MeshRenderer>().material = Things[0];
         Colores[5].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[3].GetComponent<MeshRenderer>().material = Things[0];
         Colores[6].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[4].GetComponent<MeshRenderer>().material = Things[0];
         Colores[7].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[5].GetComponent<MeshRenderer>().material = Things[0];
         Colores[8].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[6].GetComponent<MeshRenderer>().material = Things[0];
         Colores[9].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         Exist = true;
         yield return new WaitForSeconds(.05F);
         Colores[7].GetComponent<MeshRenderer>().material = Things[0];
         yield return new WaitForSeconds(.05F);
         Colores[8].GetComponent<MeshRenderer>().material = Things[0];
         yield return new WaitForSeconds(.05F);
         Colores[8].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[7].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         yield return new WaitForSeconds(.05F);
         Colores[6].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         Colores[9].GetComponent<MeshRenderer>().material = Things[0];
         yield return new WaitForSeconds(.05F);
         Colores[5].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         Colores[8].GetComponent<MeshRenderer>().material = Things[0];
         yield return new WaitForSeconds(.05F);
         Colores[4].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         Colores[7].GetComponent<MeshRenderer>().material = Things[0];
         yield return new WaitForSeconds(.05F);
         Colores[3].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         Colores[6].GetComponent<MeshRenderer>().material = Things[0];
         yield return new WaitForSeconds(.05F);
         Colores[2].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         Colores[5].GetComponent<MeshRenderer>().material = Things[0];
         yield return new WaitForSeconds(.05F);
         Colores[1].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
         Colores[4].GetComponent<MeshRenderer>().material = Things[0];
         yield return new WaitForSeconds(.05F);
         Colores[0].GetComponent<MeshRenderer>().material = Things[SolveAnimColor];
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
   IEnumerator ProcessTwitchCommand (string command) {
      if (Regex.IsMatch(command, @"^\s*colou?rblind|cb\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
         yield return null;
         cbMode = !cbMode;
         for (int i = 0; i < 10; i++) {
            CBText[i].gameObject.SetActive(cbMode);
         }
      }
      else if (Regex.IsMatch(command, @"^\s*0\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
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

   IEnumerator TwitchHandleForcedSolve () {
      yield return ProcessTwitchCommand(answer.ToString());
   }
}
