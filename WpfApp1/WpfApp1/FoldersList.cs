using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Settings
{
    public class FoldersList
    {
        private readonly ListBox _foldersListBox;
        private const string FileName = "foldername.xml";

        public FoldersList(ListBox foldersListBox)
        {
            _foldersListBox = foldersListBox;
            try
            {
                ReadFolders();
            }
            catch (Exception)
            {
                CreateFile();
            }
        }


        public void AddFolder(string folderText)
        {
            if (_foldersListBox.Items.Contains(folderText) || folderText.Equals("")) return;
            _foldersListBox.Items.Add(folderText);
        }

        public void DelSelectedFolder()
        {
            _foldersListBox.Items.RemoveAt(_foldersListBox.SelectedIndex);
        }

        private void ReadFolders()
        {
            using (Stream stream = new FileStream(FileName, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(List<string>));
                var list = (List<string>) serializer.Deserialize(stream);
                foreach (var element in list)
                {
                    if (Exists(element) && !_foldersListBox.Items.Contains(element))
                    {
                        _foldersListBox.Items.Add(element);
                    }
                }
            }
        }

        internal void WriteFile()
        {
            using (Stream writer = new FileStream(FileName, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(List<string>));
                var list = _foldersListBox.Items.Cast<string>().ToList();

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

        public List<string> GetFolders()
        {
            return _foldersListBox.Items.Cast<string>().ToList();
        }

        public bool Exists(string folderText)
        {
            return Directory.Exists(folderText);
        }
    }
}