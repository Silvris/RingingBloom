using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.Common
{
    public enum SupportedGames
    {
        None,//generic version for maximum support
        MHWorld,//WWise version 120
        RE2DMC5,//WWise version 125
        RE3R,//WWise version 132
        MHRise,//WWise verison 134
        MHRiseSwitch,//WWise version 134 (uses UTF-8 strings in PCK)
        RE8 //WWise version 135

    }
    public enum HIRCTypes
    {
        Empty,//not used but needed to offset these by one without having to go through and set equal to values
        State,
        Sound,
        Action,
        Event,
        RandomSequence,
        Switch,
        ActorMixer,
        AudioBus,
        LayerContain,
        MusicSegment,
        MusicTrack,
        MusicSwitch,
        MusicPlaylist,
        Attenuation,
        DialogueEvent,
        FeedbackBus,
        FeedbackNode,
        FxShareSet,
        FxCustom,
        AuxBus,
        LFOModulator,
        EnvelopeModulator,
        AudioDevice
    }

    public enum SoundSettingsTypes
    {
        Volume,
        LFE,
        Pitch,
        LowPassFilter,
        HighPassFilter,
        BusVolume,
        MakeUpGain,
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

    public enum HIRC3ActionScope
    {
        Null,
        SwitchTrigger,
        Global,
        GameObject,
        StateObject,
        All,
        Null6,
        Null7,
        Null8,
        AllExcept,
    }

    public enum HIRC3ActionType
    {
        Null0,
        Stop,
        Pause,
        Resume,
        Play,
        Trigger,
        Mute,
        Unmute,
        SetPitch,
        ResetPitch,
        SetVolume,
        ResetVolume,
        SetBusVolume,
        ResetBusVolume,
        SetLowPassFilter,
        ResetLowPassFilter,
        EnableState,
        DisableState,
        SetState,
        SetGameParameter,
        ResetGameParameter,
        Unkn15,
        Unkn16,
        Unkn17,
        Unkn18,
        SetSwitch,
        BypassToggle,
        ResetBypass,
        Break,
        Unkn1D,
        Seek
    }

    public enum WWCTType
    {
        WWEV,
        WWBK,
        WWPK,
        WWSW,
        WWST,
        WWGP,
        WWENF,
        Unkn3
    }

}
