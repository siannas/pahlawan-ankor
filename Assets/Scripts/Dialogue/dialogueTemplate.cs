using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace KoganeUnityLib
{
    public class Dialog
    {
        public int id { get; set; }
        public string title { get; set; }
        public Dictionary<string, string> persons { get; set; }
        public List<Dictionary<string, string>> dialogue { get; set; }
    }

    public class dialogueTemplate : MonoBehaviour
    {
        const String dialoguePath = "Assets/Files/dialogue.json";
        string dialogue;

        private IList<Dialog> scenario;

        private Dictionary<string, string> persons;

        private List<Dictionary<string, string>>.Enumerator currScenario;

        private bool allComplete = false;


        void Start()
        {
            StreamReader stream = new StreamReader(dialoguePath);
            dialogue = stream.ReadToEnd();
            scenario = JsonConvert.DeserializeObject<IList<Dialog>>(dialogue);
        }



        public void startScenarioAt(int index)
        {
            persons = scenario[index].persons;
            currScenario = scenario[index].dialogue.GetEnumerator();
        }

        public string getNextLine()
        {
            try
            {
                currScenario.MoveNext();
                foreach (KeyValuePair<string, string> line in currScenario.Current)
                {
                    return line.Value;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }

}