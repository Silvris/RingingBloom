using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.Common
{
    public enum HIRCTypes
    {
        Settings,
        SoundSFXVoice,
        EventAction,
        Event,
        RandomContainer,
        SwitchContainer,
        ActorMixer,
        AudioBus,
        BlendContainer,
        MusicSegment,
        MusicTrack,
        MusicSwitchContainer,
        PlaylistContainer,
        Attenuation,
        DialogueEvent,
        MotionBus,
        MotionFX,
        Effect,
        Type19,
        AuxiliaryBus

    }

    public enum SoundSettingsTypes
    {
        Volume,
        Unkn1,
        Pitch,
        LowPassFilter,
        Unkn4,
        PlaybackPriority,
        PlaybackPriorityOffset,
        LoopCount,
        MotionValueOffset,
        Unkn9,
        UnknA,
        TwoDXCoord,
        TwoDYCoord,
        PositionCenterPercent,
        UnknE,
        UnknF,
        Unkn10,
        Unkn11,
        Bus0Volume,
        Bus1Volume,
        Bus2Volume,
        Bus3Volume,
        GDAuxSendVolume,
        OutputBusVolume,
        OutputBusLowPass
    }
}
