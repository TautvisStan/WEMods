using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Tournaments
{
    public class CustomSegment : Segment
    {
        public List<int> leftWresters { get; set; } = new();
        public List<int> rightWresters { get; set; } = new();
        public int HigherBracket;
        public int HigherBracketSide;
        public void UpdateMatchTitle()
        {
            string title = "";
            switch (id)
            {
                case 1:
                    title = "Finals";
                    break;
                case 2:
                case 3:
                    title = "Semi Finals";
                    break;
                case 4:
                case 5:
                case 6:
                    title = "?????";
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                    title = "Quarter Finals";
                    break;
            }
            match = title + " " + match;
        }
        public void SendFurther(List<int> wrestlers)
        {
            if(HigherBracket != -1)
            {
                CustomSegment nextSegment = Plugin.TournamentCard.segment[HigherBracket] as CustomSegment;
                if(HigherBracketSide == 1)
                {
                    nextSegment.leftWresters.AddRange(wrestlers);
                    nextSegment.leftName = GetWinnerName(wrestlers);
                }
                else
                {
                    nextSegment.rightWresters.AddRange(wrestlers);
                    nextSegment.rightName = GetWinnerName(wrestlers);
                }
                if(nextSegment.leftWresters.Count > 0 && nextSegment.rightWresters.Count > 0)
                {
                    nextSegment.time = 0;
                }
            }
            else
            {
                if (FFCEGMEAIBP.OOODPHNGHGD != 0)
                {
                    Plugin.TournamentCard.Winner = GetWinnerName(wrestlers);
                }
                else
                {
                    if (FFCEGMEAIBP.OLJFOJOLLOM < 1) //solo
                    {
                        Plugin.TournamentCard.Winner = wrestlers.Count + "-way Draw!";
                    }
                    else
                    {
                        Plugin.TournamentCard.Winner =  "2-way Draw!";
                    }
                }
            }
        }
        public static string GetWinnerName(List<int> winner)
        {
            if(FFCEGMEAIBP.OLJFOJOLLOM < 1) //solo
            {
                if (winner.Count == 1)
                {
                    return Characters.c[winner[0]].name;
                }
                else
                {
                    return "Multiple Wrestlers";
                }
            }
            else   //team
            {
                if(winner.Count == 2)
                {
                    return Characters.c[winner[0]].BEMDBFHJFAB(Characters.c[winner[1]].id, 1);
                    /*    if (Characters.c[winner[0]].teamName != "")
                        {
                            Debug.LogWarning("TEAM HAS NAME");
                            return Characters.c[winner[0]].teamName;

                        }
                        else
                        {
                            Debug.LogWarning("TEAM NO NAME");
                            return Characters.c[winner[0]].name + " & " + Characters.c[winner[1]].name;
                        }*/
                }
                else
                {
                    return Characters.c[winner[0]].name + "'s Team";
                    //return "Multiple Wrestlers";
                    // return Characters.c[winner[0]].BEMDBFHJFAB(0, 1);
                }

            }
        }
    }
    public class CustomCard : Card
    {
        public int CurrentSegment { get; set; }
        public string Winner { get; set; }
        public GameObject WinnerObj { get; set; }
        public CustomCard()
        {
            Winner = "TBD";
            CurrentSegment = 0;
            date = 0;
            size = 10;
            segment = new CustomSegment[this.size + 1];
            CustomSegment[] customSegment = segment as CustomSegment[];
            for (int i = 1; i <= this.size; i++)
            {
                customSegment[i] = new CustomSegment();
                customSegment[i].id = i;


                switch (i)
                {
                    case 1:
                        customSegment[i].match = "Finals";
                        customSegment[i].leftName = "TBD";
                        customSegment[i].rightName = "TBD";
                        customSegment[i].time = -1;
                        break;
                    case 2:
                    case 3:
                        customSegment[i].match = "Semi Finals";
                        customSegment[i].leftName = "TBD";
                        customSegment[i].rightName = "TBD";
                        customSegment[i].time = -1;
                        break;
                    case 4:
                    case 5:
                    case 6:
                        customSegment[i].match = "?????";
                        customSegment[i].leftName = "?????";
                        customSegment[i].rightName = "?????";
                        customSegment[i].time = -1;
                        break;
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        customSegment[i].match = "Quarter Finals";
                        break;
                }

                switch (i)
                {
                    case 1:
                        customSegment[i].HigherBracket = -1;
                        customSegment[i].HigherBracketSide = -1;
                        break;
                    case 2:
                        customSegment[i].HigherBracket = 1;
                        customSegment[i].HigherBracketSide = 1;
                        break;
                    case 3:
                        customSegment[i].HigherBracket = 1;
                        customSegment[i].HigherBracketSide = 2;
                        break;

                    case 7:
                        customSegment[i].HigherBracket = 2;
                        customSegment[i].HigherBracketSide = 1;
                        break;
                    case 8:
                        customSegment[i].HigherBracket = 2;
                        customSegment[i].HigherBracketSide = 2;
                        break;
                    case 9:
                        customSegment[i].HigherBracket = 3;
                        customSegment[i].HigherBracketSide = 1;
                        break;
                    case 10:
                        customSegment[i].HigherBracket = 3;
                        customSegment[i].HigherBracketSide = 2;
                        break;
                }
            }
        }

    }
}
