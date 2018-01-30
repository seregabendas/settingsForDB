using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Settings

{
    public class ServerList
    {
        private const string FileName = "serverlist.xml";
        private readonly ListBox _serverListBox;

        public ServerList(ListBox listBox)
        {
            _serverListBox = listBox;
            try
            {
                ReadServers();
            }
            catch (Exception)
            {
                CreateFile();
            }
        }


        public void ServerUp()
        {
            var index = _serverListBox.SelectedIndex;
            var swap = _serverListBox.SelectedItem;
            if (index == 0) return;
            _serverListBox.Items.RemoveAt(index);
            _serverListBox.Items.Insert(index - 1, swap);
            _serverListBox.SelectedItem = swap;
        }

        public void ServerDown()
        {
            var index = _serverListBox.SelectedIndex;
            var swap = _serverListBox.SelectedItem;
            if (index == _serverListBox.Items.Count - 1) return;
            _serverListBox.Items.RemoveAt(index);
            _serverListBox.Items.Insert(index + 1, swap);
            _serverListBox.SelectedItem = swap;
        }


        public ListBox GetServers()
        {
            return _serverListBox;
        }

        public void AddServer(string serverName)
        {
            if (_serverListBox.Items.Contains(serverName) || serverName.Equals("")) return;
            _serverListBox.Items.Add(serverName);
        }

        public void DelSelectedServer()
        {
            _serverListBox.Items.RemoveAt(_serverListBox.SelectedIndex);
        }

        private void ReadServers()
        {
            using (Stream stream = new FileStream(FileName, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(List<string>));
                var list = (List<string>) serializer.Deserialize(stream);
                foreach (var element in list)
                {
                    if (!_serverListBox.Items.Contains(element))
                    {
                        _serverListBox.Items.Add(element);
                    }
                }
            }
        }

        internal void WriteFiles(FoldersList foldersList)
        {
            WriteFile(FileName);

            foreach (var folder in foldersList.GetFolders())
            {
                WriteFile(folder + "\\" + FileName);
            }
        }

        private void WriteFile(string fileName)
        {
            using (Stream writer = new FileStream(fileName, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<string>));
                var list = _serverListBox.Items.Cast<string>().ToList();

                serializer.Serialize(writer, list);
            }
        }

        private static void CreateFile()
        {
            using (Stream writer = new FileStream(FileName, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<string>));
                serializer.Serialize(writer, new List<string>());
            }
        }
    }
}