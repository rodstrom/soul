using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Soul
{
    class SaveData
    {
        private readonly string filename;
        private TextReader fileReader = null;
        private TextWriter fileWriter = null;
        private Dictionary<string, bool> saveData;

        public SaveData(string filename)
        {
            saveData = new Dictionary<string, bool>();
            this.filename = filename;
        }

        public void LoadDataFile()
        {
            fileReader = new StreamReader(filename);
            if (System.IO.File.Exists(filename) == false)
            {
                throw new System.InvalidOperationException("Error: " + filename + " could not be found.");
            }

            saveData.Clear();

            string line = "";
            string read = "";
            line = fileReader.ReadLine();
            while (line != null)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] != '=')
                    {
                        read += line[i];
                    }
                    else
                    {
                        string status = line.Substring(i+1, (line.Length - (i+1) ));
                        saveData.Add(read, Convert.ToBoolean(status));
                        
                    }
                }
                read = "";
                line = fileReader.ReadLine();
            }
            fileReader.Close();
        }

        public void SaveDataFile(string level, bool status)
        {
            fileWriter = new StreamWriter(filename);
            if (System.IO.File.Exists(filename) == false)
            {
                throw new System.InvalidOperationException("Error: " + filename + " could not be found.");
            }

            if (saveData.ContainsKey(level) == true)
            {
                saveData[level] = status;
                foreach (KeyValuePair<string, bool> pair in saveData)
                {
                    fileWriter.WriteLine(pair.Key + "=" + pair.Value.ToString());
                }
            }
            else
            {
                throw new System.InvalidOperationException("Error: could not save " + level + " as this level does not exist.");
            }
            fileWriter.Close();
        }

        public bool LevelStatus(string level)
        {
            if (saveData.ContainsKey(level) == false)
            {
                throw new System.InvalidOperationException("Error: " + level + " when loading status");
            }
            return saveData[level];
        }

        public Dictionary<string, bool> GetSaveData { get { return saveData; } }
    }
}
