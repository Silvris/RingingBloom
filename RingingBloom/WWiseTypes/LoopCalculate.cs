using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom
{
    class LoopCalculate
    {
        //user input values
        public double introStart { get; set; }
        public double loopStart { get; set; }
        public double loopLength { get; set; }
        public double songLength { get; set; }
        //user default values
        public double defIntroTrue { get; set; }
        public double defIntroDisp { get; set; }
        public double defLoopTrue { get; set; }
        public double defLoopDisp { get; set; }
        public double defLoop2 { get; set; }
        //notable math values
        double introDisp = 0;
        double loopDisp = 0;
        double loop2Disp = 0;
        //3-part, same start
        public double A1 { get; set; }
        public double A2 { get; set; }
        public double A3 { get; set; }
        public double A4 { get; set; }
        public double A5 { get; set; }
        public double A6 { get; set; }
        public double A7 { get; set; }
        public double A8 { get; set; }
        public double A9 { get; set; }
        public double A10 { get; set; }
        public double A11 { get; set; }
        public double A12 { get; set; }
        public double A13 { get; set; }
        //3-part, different
        public double B1 { get; set; }
        public double B2 { get; set; }
        public double B3 { get; set; }
        public double B4 { get; set; }
        public double B5 { get; set; }
        public double B6 { get; set; }
        public double B7 { get; set; }
        public double B8 { get; set; }
        public double B9 { get; set; }
        public double B10 { get; set; }
        public double B11 { get; set; }
        public double B12 { get; set; }
        public double B13 { get; set; }
        //2-part, same
        public double C1 { get; set; }
        public double C2 { get; set; }
        public double C3 { get; set; }
        public double C4 { get; set; }
        public double C5 { get; set; }
        public double C6 { get; set; }
        public double C7 { get; set; }
        public double C8 { get; set; }
        public double C9 { get; set; }
        //2-part, different
        public double D1 { get; set; }
        public double D2 { get; set; }
        public double D3 { get; set; }
        public double D4 { get; set; }
        public double D5 { get; set; }
        public double D6 { get; set; }
        public double D7 { get; set; }
        public double D8 { get; set; }
        public double D9 { get; set; }
        //1-part, same
        public double E1 { get; set; }
        public double E2 { get; set; }
        public double E3 { get; set; }
        public double E4 { get; set; }
        public double E5 { get; set; }
        //intro simple
        public double F1 { get; set; }
        public double F2 { get; set; }
        public double F3 { get; set; }
        public double F4 { get; set; }
        public double F5 { get; set; }
        //intro displaced
        public double G1 { get; set; }
        public double G2 { get; set; }
        public double G3 { get; set; }
        public double G4 { get; set; }
        public double G5 { get; set; }
        //intro 2-part
        public double H1 { get; set; }
        public double H2 { get; set; }
        public double H3 { get; set; }
        public double H4 { get; set; }
        public double H5 { get; set; }
        public double H6 { get; set; }
        public double H7 { get; set; }
        public double H8 { get; set; }
        public double H9 { get; set; }

        public LoopCalculate()
        {
            introStart = 0;
            loopStart = 0;
            loopLength = 0;
            songLength = 0;
            defIntroTrue = 0;
            defIntroDisp = 0;
            defLoopTrue = 0;
            defLoopDisp = 0;
            defLoop2 = 0;
    }

        public void CreateMathValues()
        {
            introDisp = defIntroDisp - defIntroTrue;
            loopDisp = defLoopDisp - defLoopTrue;
            loop2Disp = defLoop2 - defLoopTrue;
        }
        public void Intros()
        {
            //intro simple
            F1 = -introStart;
            F2 = introStart;
            F3 = -(songLength - loopStart-loop2Disp);
            F4 = songLength;
            F5 = (loopStart+loop2Disp) - introStart;

            //intro displaced
            G1 = -(introStart + introDisp);
            G2 = introStart + introDisp;
            G5 = (loopStart + loop2Disp) - G2;
            G3 = songLength - G5 - G2;
            G4 = songLength;

            //intro 2-part
            H1 = -introStart;
            H2 = introStart;
            H3 = songLength - loopStart;
            H4 = songLength;
            H5 = -introStart;
            H6 = loopStart;
            H9 = (loopStart + loop2Disp) - introStart;
            H7 = songLength - H9 - H2;
            H8 = songLength;
        }
        public void ThreeParts()
        {
            //assign different values first
            B1 = -(loopStart+loopDisp);
            B2 = loopStart+loop2Disp;
            B4 = songLength;
            B6 = B2 + G5;
            B3 = -(B4 - B6);
            B5 = -loopStart;
            B7 = -(songLength - loopStart - loopLength);
            B8 = songLength;
            B9 = songLength - B2 - (B4 - (loopLength + loop2Disp));
            B10 = -loopStart;
            B11 = -H7;
            B12 = songLength;
            B13 = loopLength + loop2Disp;
            //same values now
            A1 = -(loopStart + loop2Disp);
            A2 = loopStart + loop2Disp;
            A3 = B7;
            A4 = songLength;
            A5 = A4 - loopStart - A2 + A3;
            A6 = loopStart;
            A7 = B3 + A1 - G2;
            A8 = songLength;
            A9 = A5;
            A10 = G5 - G2;
            A11 = B11;
            A12 = songLength;
            A13 = loopLength;
        }

        public void TwoParts()
        {
            //same
            C1 = -(loopStart + loop2Disp);
            C2 = loopStart + loop2Disp;
            C3 = -(songLength - loopLength - loopStart);
            C4 = songLength;
            C5 = C4 - loopStart - C2 + C3;
            C6 = loopStart +loopDisp;
            C7 = -H7;
            C8 = songLength;
            C9 = loopLength;
            //different
            D1 = -loopStart;
            D2 = loopStart + loop2Disp;
            D3 = C3;
            D4 = songLength;
            D5 = C5 + loop2Disp;
            D6 = loopStart + loopDisp;
            D7 = -H7;
            D8 = songLength;
            D9 = loopLength + loop2Disp;
        }

        public void OnePart()
        {
            E1 = -loopStart;
            E2 = loopStart;
            E3 = -(songLength - loopLength - loopStart);
            E4 = songLength;
            E5 = loopLength;
        }

        public string Calculate(int introCount, int loopCount, bool introDiff, bool loopDiff)
        {
            CreateMathValues();
            Intros();
            ThreeParts();
            TwoParts();
            OnePart();
            string r = "Intro:\n";
            switch (introCount)
            {
                case 1:
                    if (introDiff)
                    {
                        r += G1 + "\n";
                        r += G2 + "\n";
                        r += G3 + "\n";
                        r += G4 + "\n";
                        r += G5 + "\n";
                    }
                    else
                    {
                        r += F1 + "\n";
                        r += F2 + "\n";
                        r += F3 + "\n";
                        r += F4 + "\n";
                        r += F5 + "\n";
                    }
                    break;
                case 2:
                    r += H1 + "\n";
                    r += H2 + "\n";
                    r += H3 + "\n";
                    r += H4 + "\n";
                    r += H5 + "\n";
                    r += H6 + "\n";
                    r += H7 + "\n";
                    r += H8 + "\n";
                    r += H9 + "\n";
                    break;
            }
            r += "\nLoop:\n";
            switch (loopCount)
            {
                case 1:
                    r += E1 + "\n";
                    r += E2 + "\n";
                    r += E3 + "\n";
                    r += E4 + "\n";
                    r += E5 + "\n";
                    break;
                case 2:
                    if (loopDiff)
                    {
                        r += D1 + "\n";
                        r += D2 + "\n";
                        r += D3 + "\n";
                        r += D4 + "\n";
                        r += D5 + "\n";
                        r += D6 + "\n";
                        r += D7 + "\n";
                        r += D8 + "\n";
                        r += D9 + "\n";
                    }
                    else
                    {
                        r += C1 + "\n";
                        r += C2 + "\n";
                        r += C3 + "\n";
                        r += C4 + "\n";
                        r += C5 + "\n";
                        r += C6 + "\n";
                        r += C7 + "\n";
                        r += C8 + "\n";
                        r += C9 + "\n";
                    }
                    break;
                case 3:
                    if (loopDiff)
                    {
                        r += B1 + "\n";
                        r += B2 + "\n";
                        r += B3 + "\n";
                        r += B4 + "\n";
                        r += B5 + "\n";
                        r += B6 + "\n";
                        r += B7 + "\n";
                        r += B8 + "\n";
                        r += B9 + "\n";
                        r += B10 + "\n";
                        r += B11 + "\n";
                        r += B12 + "\n";
                        r += B13 + "\n";
                    }
                    else
                    {
                        r += A1 + "\n";
                        r += A2 + "\n";
                        r += A3 + "\n";
                        r += A4 + "\n";
                        r += A5 + "\n";
                        r += A6 + "\n";
                        r += A7 + "\n";
                        r += A8 + "\n";
                        r += A9 + "\n";
                        r += A10 + "\n";
                        r += A11 + "\n";
                        r += A12 + "\n";
                        r += A13 + "\n";
                    }
                    break;
            }
            return r;
        }
    }
}
