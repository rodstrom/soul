using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Soul
{
    // Used to read and write to ini files.
    public class IniFile
    {
        #region Declarations
        // readonly keyword is like const
        // but we are allowed to initialize
        // it in our cTor.
        private readonly string _filename;

        private Dictionary<string, Dictionary<string, string>> _options = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        public string Filename
        {
            get { return _filename; }
        }

        public IniFile(string filename)
        {
            _filename = filename;
        }

        public string getValue(string group, string key)
        {
            return _options[group][key];
        }

        //public string getKey(string group)

        public void addModify(string group, string key, string value)
        {
            if (_options.ContainsKey(group) == false)
            {
                _options.Add(group, new Dictionary<string, string>());
            }

            if (_options[group].ContainsKey(key) == false)
            {
                _options[group].Add(key, value);
            }
            else
            {
                _options[group][key] = value;
            }
        }

        public void parse()
        {
            TextReader iniFile = null;

            if (System.IO.File.Exists(_filename) == true)
            {
                string line = null;
                string currentGroup = null;
                string[] keyPair = null;
                iniFile = new StreamReader(_filename);

                line = iniFile.ReadLine();
                while (line != null)
                {
                    if (line != "")
                    {
                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            currentGroup = line.Substring(1, line.Length - 2);
                            _options.Add(currentGroup, new Dictionary<string, string>());
                        }
                        else
                        {
                            keyPair = line.Split(new char[] {'='}, 2);
                                _options[currentGroup].Add(keyPair[0], keyPair[1]);
                        }
                    }
                    line = iniFile.ReadLine();
                }
                iniFile.Close();
            }
        }

        public void save()
        {
            TextWriter iniFile = new StreamWriter(_filename);

            foreach (KeyValuePair<string, Dictionary<string, string>> group in _options)
            {
                iniFile.WriteLine("[" + group.Key + "]");

                foreach(KeyValuePair<string, string> key in group.Value)
                {
                    iniFile.WriteLine(key.Key + "=" + key.Value);
                }
            }

            iniFile.Close();
        }
    }
}
