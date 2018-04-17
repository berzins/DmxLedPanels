﻿using DmxLedPanel.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxLedPanel.State
{
    public class StateManager
    {
        public static readonly int DEFAULT_STATE_STACK_SIZE = 20;

        public static readonly string DEFAULT_RELATIVE_STATE_FIEL_PATH = "projects\\";
        public static readonly string DEFAULT_STATE_FILE = "default.json";

        private static StateManager instance;
        private static object syncRoot = new Object();

        private FixedSizeStack<string> stateStack;

        private State state;

        private StateManager() {
            //LoadState(DEFAULT_STATE_FILE);
            stateStack = new FixedSizeStack<string>(DEFAULT_STATE_STACK_SIZE);
        }

        public static StateManager Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new StateManager();
                        }
                    }
                }
                return instance;
            }
        }

        private State getStateFromFile(string file) {
            string f = DEFAULT_RELATIVE_STATE_FIEL_PATH + file;
            if (File.Exists(f))
            {
                return new State().Deserialize<State>(FileIO.ReadFile(f, true));
            }
            else {
                Console.WriteLine("State file failed to load.. empty state loaded.");
                return new State();
            }
        }

        public void LoadState(string file)
        {

            Console.WriteLine("load state triggered");
            // a little hack for a bad desing :(.
            // zero out fixture/output counter cause
            // the fixture/output constructor increments smallest id in current items.


            Fixture.ResetIdCounter();
            Output.ResetIdCounter();

            // release artnet listeners
            ArtnetIn.Instance.ClearDmxPacketListeners();

            //load state
            this.state = getStateFromFile(file);

            // connect state to artnet inupt
            ArtnetIn.Instance.AddDmxPacketListeners(
                this.state.GetPatchedFixtures().Select(x => (IDmxPacketHandler)x).ToList()
                );
        }

        // Serialize the state and put in the 'undo' stack;
        public string GetStateSerialized() {
            string json = this.state.Serialize();
            stateStack.Add(json);
            return json;
        }

        public State State { get { return this.state; } }

        public void SaveState(string filename)
        {
            FileIO.WriteFile(DEFAULT_RELATIVE_STATE_FIEL_PATH + filename + ".json", true, GetStateSerialized());
        }

        public string [] GetAllStateFiles() {
           return FileIO.GetFiles(DEFAULT_RELATIVE_STATE_FIEL_PATH, true, false, ".json");
        }
    }
}
