using NAudio.Midi;
using System;

namespace MiniSequencer
{
    internal class Track
    {
        int[] keys;
        bool playing;
        public Track(int[] keys)
        {
            this.keys = keys;
            playing = false;
        }

        public void Play(int bar, MidiOut midi)
        {
            if (playing)
            {
                var previousBar = bar - 1;
                if (previousBar < 0) previousBar = keys.Length - 1;
                midi.Send(MidiMessage.StopNote(keys[previousBar], 127, 10).RawData);
                playing = false;
            }
            if (keys[bar] != 0)
            {
                midi.Send(MidiMessage.StartNote(keys[bar], 127, 10).RawData);
                playing = true;
            }
        }
    }
}