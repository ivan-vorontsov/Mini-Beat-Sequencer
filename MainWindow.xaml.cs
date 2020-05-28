using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using NAudio.Midi;


namespace MiniSequencer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CheckBox> checkBoxList;
        Sequencer sequencer;
        Label bpm;

        string[] instrumentNames = { "Bass Drum", "Closed Hi-Hat",
            "Open Hi-Hat", "Acoustic Snare", "Crash Cymbal", "Tambourine",
            "High Tom", "Hi Bongo", "Maracas", "Whistle", "Low Conga",
            "Cowbell", "Vibraslap", "Low-mid Tom", "High Agogo",
            "Open Hi Conga"};
        int[] instruments = { 35, 42, 46, 38, 49, 54, 50, 60, 70, 72, 61, 56, 58, 47, 67, 63 };

        public MainWindow()
        {
            InitializeComponent();
            SetUpMidi();
            BuildGui();
        }

        private void SetUpMidi()
        {
            sequencer = new Sequencer();
            sequencer.SetTempoInBPM(60);
        }

        private void BuildTrackAndStart()
        {
            int[] trackList = null;
            sequencer.DeleteTracks();
            for (int i = 0; i < 16; i++)
            {
                trackList = new int[16];
                int key = instruments[i];

                for(int j = 0; j < 16; j++)
                {
                    CheckBox c = checkBoxList[j + 16 * i];
                    if (c.IsChecked.Value)
                    {
                        trackList[j] = key;
                    }
                    else
                    {
                        trackList[j] = 0;
                    }
                }

                var track =  MakeTrack(trackList);
                sequencer.AddTrack(track);
            }
            
            sequencer.Start();
        }

        private Track MakeTrack(int[] trackList)
        {
            return new Track(trackList);
        }

        private void BuildGui()
        {
            RootGrid.HorizontalAlignment = HorizontalAlignment.Left;
            RootGrid.VerticalAlignment = VerticalAlignment.Top;
            for(int i = 0; i < 24; i++)
            {
                RootGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for(int i = 0; i < 16; i++)
            {
                RootGrid.RowDefinitions.Add(new RowDefinition());
            }
            for(int i = 0; i < 16; i++)
            {
                Label label = new Label
                {
                    Content = instrumentNames[i]
                };
                Grid.SetColumnSpan(label, 3);
                Grid.SetRow(label, i);
                RootGrid.Children.Add(label);
            }

            checkBoxList = new List<CheckBox>();
            for(int i = 0; i < 256; i++)
            {
                CheckBox c = new CheckBox
                {
                    IsChecked = false
                };
                checkBoxList.Add(c);
                Grid.SetColumn(c, 3 + (i % 16));
                Grid.SetRow(c, i / 16);
                RootGrid.Children.Add(c);
            }
            Button start = new Button
            {
                Content = "Start"
            };
            start.Click += Start_Click;
            Grid.SetColumn(start, 22);
            Grid.SetRow(start, 0);
            Grid.SetColumnSpan(start, 3);
            RootGrid.Children.Add(start);

            Button stop = new Button
            {
                Content = "Stop"
            };
            stop.Click += Stop_Click;
            Grid.SetColumn(stop, 22);
            Grid.SetRow(stop, 1);
            Grid.SetColumnSpan(stop, 3);
            RootGrid.Children.Add(stop);

            Button upTempo = new Button
            {
                Content = "Up Tempo"
            };
            upTempo.Click += UpTempo_Click;
            Grid.SetColumn(upTempo, 22);
            Grid.SetRow(upTempo, 2);
            Grid.SetColumnSpan(upTempo, 3);
            RootGrid.Children.Add(upTempo);

            Button downTempo = new Button
            {
                Content = "Down Tempo"
            };
            downTempo.Click += DownTempo_Click;
            Grid.SetColumn(downTempo, 22);
            Grid.SetRow(downTempo, 3);
            Grid.SetColumnSpan(downTempo, 3);
            RootGrid.Children.Add(downTempo);

            bpm = new Label
            {
                Content = sequencer.GetTempoInBPM()
            };
            Grid.SetColumn(bpm, 22);
            Grid.SetRow(bpm, 4);
            Grid.SetColumnSpan(bpm, 3);
            RootGrid.Children.Add(bpm);
        }

        private void DownTempo_Click(object sender, RoutedEventArgs e)
        {
            sequencer.BPM--;
            bpm.Content = sequencer.BPM.ToString();
        }

        private void UpTempo_Click(object sender, RoutedEventArgs e)
        {
            sequencer.BPM++;
            bpm.Content = sequencer.BPM.ToString();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            sequencer.Stop();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            BuildTrackAndStart();
        }
    }
}
