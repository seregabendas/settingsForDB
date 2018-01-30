using System.Windows;
using System.Windows.Input;

namespace Settings
{
    public partial class MainWindow
    {
        private readonly ServerList _serverList;
        private readonly FoldersList _foldersList;

        public MainWindow()
        {
            InitializeComponent();
            _serverList = new ServerList(ServerListBox);
            _foldersList = new FoldersList(FoldersListBox);
        }


        private void Button_Add_Server_Click(object sender, RoutedEventArgs e)
        {
            _serverList.AddServer(ServerText.Text);
            ServerText.Clear();
        }

        private void Button_Add_Folder_Click(object sender, RoutedEventArgs e)
        {
            if (_foldersList.Exists(FolderText.Text))
            {
                _foldersList.AddFolder(FolderText.Text);
            }
            else
            {
                MessageBox.Show("Такой папки не существует");
            }

            FolderText.Clear();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            _serverList.WriteFiles(_foldersList);
            _foldersList.WriteFile();
            Close();
        }

        private void Btn_Server_Up(object sender, RoutedEventArgs e)
        {
            _serverList.ServerUp();
        }

        private void Btn_Server_Down(object sender, RoutedEventArgs e)
        {
            _serverList.ServerDown();
        }

        private void Btn_Server_Del(object sender, RoutedEventArgs e)
        {
            _serverList.DelSelectedServer();
        }

        private void Btn_Folder_Del(object sender, RoutedEventArgs e)
        {
            _foldersList.DelSelectedFolder();
        }

        private void ServerText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Add_Server_Click(sender, e);
            }
        }

        private void FolderText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Add_Folder_Click(sender, e);
            }
        }

        private void FoldersListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Btn_Folder_Del(sender, e);
            }
        }

        private void ServerListBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    Btn_Server_Del(sender, e);
                    break;
                
            }
        }
    }
}