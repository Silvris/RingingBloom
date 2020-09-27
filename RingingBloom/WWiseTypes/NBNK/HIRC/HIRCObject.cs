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

        HIRCObject(BinaryReader br)
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
        }
    }
}
