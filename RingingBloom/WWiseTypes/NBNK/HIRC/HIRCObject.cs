using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    class HIRCObject
    {
        HIRC1 SettingsObject = null;
        HIRC3 EventAction = null;
        HIRC4 Event = null;
        HIRC10 MusicSegment = null;
        HIRCUnkn Datums = null;

        public HIRCObject(BinaryReader br)
        {
            byte HIRCType = br.ReadByte();
            uint HIRCLength = br.ReadUInt32();
            switch (HIRCType)
            {
                case 1:
                    SettingsObject = new HIRC1(br);
                    break;
                case 3:
                    EventAction = new HIRC3(br);
                    break;
                case 4:
                    Event = new HIRC4(br);
                    break;
                default:
                    Datums = new HIRCUnkn(HIRCType, HIRCLength, br);
                    break;
            }
        }

        public int CalculateSectionLength()
        {
            int length = 5;
            if (SettingsObject != null)
            {
                length += SettingsObject.GetLength();
            }
            if (EventAction != null)
            {
                length += EventAction.GetLength();
            }
            if (Event != null)
            {
                length += Event.GetLength();
            }
            if (Datums != null)
            {
                length += Datums.GetLength();
            }
            return length;
        }

        public void Export(BinaryWriter bw)
        {
            if(SettingsObject != null)
            {
                SettingsObject.Export(bw);
            }
            if (EventAction != null)
            {
                EventAction.Export(bw);
            }
            if (Event != null)
            {
                Event.Export(bw);
            }
            if (Datums != null)
            {
                Datums.Export(bw);
            }
        }
    }
}
