using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace MiniSequencer
{
    internal class Sequencer
    {
        List<Track> tracks;
        int bpm;
        int bars = 16;
        int bar;
        Timer timer;
        DateTime previousTime;
        MidiOut midi;
        double time;
        public Sequencer()
        {
            tracks = new List<Track>();
            bpm = 1;
            bar = 0;
        }

        public int BPM
        {
            get { return bpm; }
            set 
            {
                if (value > 0)
                {
                    bpm = value;
                }
            }
        }

        internal void Stop()
        {
            if (timer != null) timer.Dispose();
            if (midi != null) midi.Dispose();
        }

        internal void SetTempoInBPM(int bpm)
        {
            this.bpm = bpm;
        }

        public int GetTempoInBPM()
        {
            return bpm;
        }

        internal void DeleteTracks()
        {
            tracks.Clear();
        }

        internal void AddTrack(Track track)
        {
            tracks.Add(track);
        }

        internal void Start()
        {
            Stop();
            bar = 0;
            midi = new MidiOut(0);
            timer = new Timer();
            timer.Interval = 1000 / 60;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            previousTime = DateTime.Now;
            PlayBar();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var time = e.SignalTime;
            var delta = (time - previousTime).TotalMilliseconds;
            previousTime = time;

            this.time += delta;
            var minute = 1000 * 60;
            var barLength = (double)minute / (bpm * 4);
            if (this.time > barLength)
            {
                bar++;
                if (bar == 16) bar = 0;
                this.time = 0;
                PlayBar();
            }
        }

        private void PlayBar()
        {
            var tracksCopy = tracks.ToArray();
            foreach (var track in tracksCopy)
            {
                track.Play(bar, midi);
            }
        }
    }
}