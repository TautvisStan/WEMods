using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Tournaments
{
    public class CustomSegment : Segment
    {
        public List<int> leftWresters { get; set; } = new();
        public List<int> rightWresters { get; set; } = new();
    }
    public class CustomCard : Card
    {
        public int CurrentSegment { get; set; }
        public CustomCard()
        {
            CurrentSegment = 0;
            date = 0;
            size = 10;
            segment = new CustomSegment[this.size + 1];
            for (int i = 1; i <= this.size; i++)
            {
                segment[i] = new CustomSegment();
                segment[i].id = i;


                switch (i)
                {
                    case 1:
                        segment[i].match = "Finals";
                        segment[i].leftName = "TBD";
                        segment[i].rightName = "TBD";
                        segment[i].time = -1;
                        break;
                    case 2:
                    case 3:
                        segment[i].match = "Semi Finals";
                        segment[i].leftName = "TBD";
                        segment[i].rightName = "TBD";
                        segment[i].time = -1;
                        break;
                    case 4:
                    case 5:
                    case 6:
                        segment[i].match = "?????";
                        segment[i].leftName = "?????";
                        segment[i].rightName = "?????";
                        segment[i].time = -1;
                        break;
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        segment[i].match = "Quarter Finals";
                        break;
                }
            }
        }

    }
}
