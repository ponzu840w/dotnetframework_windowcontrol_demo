using System;
using System.Windows.Forms;
using System.Drawing;

namespace BringFrontAndActiveWin
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    // メインフォーム：デモ選択用
    public class MainForm : Form
    {
        private Button btnRestoreDemo;
        private Button btnActiveDemo;

        public MainForm()
        {
            this.Text = "Demo: Restore & Active Detection";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(320, 100);

            btnRestoreDemo = new Button {
                Text = "Restore Demo",
                Left = 10, Top = 20, Width = 140
            };
            btnRestoreDemo.Click += (s, e) => new RestoreForm().Show();

            btnActiveDemo = new Button {
                Text = "Active Detection Demo",
                Left = 160, Top = 20, Width = 140
            };
            btnActiveDemo.Click += (s, e) => new ActiveForm().Show();

            this.Controls.Add(btnRestoreDemo);
            this.Controls.Add(btnActiveDemo);
        }
    }

    // ウィンドウ展開（自動復帰）デモ
    public class RestoreForm : Form
    {
        private NotifyIcon trayIcon;
        private Button btnMinimizeTray;
        private Timer restoreTimer;

        public RestoreForm()
        {
            this.Text = "Restore Demo";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(300, 120);

            btnMinimizeTray = new Button {
                Text = "Minimize to Tray",
                Left = 80, Top = 20, Width = 140
            };
            btnMinimizeTray.Click += (s, e) => MinimizeToTray();
            this.Controls.Add(btnMinimizeTray);

            // トレイアイコン
            trayIcon = new NotifyIcon {
                Icon = SystemIcons.Application,
                Text = "Restore Demo",
                Visible = false
            };
            trayIcon.DoubleClick += (s, e) => RestoreFromTray();

            // 自動復帰タイマー（5秒後）
            restoreTimer = new Timer { Interval = 5000 };
            restoreTimer.Tick += (s, e) => {
                restoreTimer.Stop();
                RestoreFromTray();
            };
        }

        private void MinimizeToTray()
        {
            this.Hide();
            trayIcon.Visible = true;
            // 5秒後に自動復帰
            restoreTimer.Start();
        }

        private void RestoreFromTray()
        {
            trayIcon.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            trayIcon.Dispose();
            base.OnFormClosing(e);
        }
    }

    // アクティブ検知デモ
    public class ActiveForm : Form
    {
        private TextBox logBox;
        private Timer pollTimer;
        private bool lastActiveForm;
        private bool lastContainsFocus;
        private bool lastFocused;

        public ActiveForm()
        {
            this.Text = "Active Detection Demo";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(400, 300);

            logBox = new TextBox {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(logBox);

            // 初期状態
            lastActiveForm = (Form.ActiveForm == this);
            lastContainsFocus = this.ContainsFocus;
            lastFocused = this.Focused;

            // ポーリングタイマー
            pollTimer = new Timer { Interval = 500 };
            pollTimer.Tick += (s, e) => CheckActiveStates();
            pollTimer.Start();
        }

        private void CheckActiveStates()
        {
            bool curActiveForm = (Form.ActiveForm == this);
            bool curContains = this.ContainsFocus;
            bool curFocused = this.Focused;
            string time = DateTime.Now.ToString("HH:mm:ss");

            if (curActiveForm != lastActiveForm)
            {
                logBox.AppendText($"[{time}] ActiveForm now {(curActiveForm ? "ACTIVE" : "INACTIVE")}\r\n");
                lastActiveForm = curActiveForm;
            }
            if (curContains != lastContainsFocus)
            {
                logBox.AppendText($"[{time}] ContainsFocus now {(curContains ? "YES" : "NO")}\r\n");
                lastContainsFocus = curContains;
            }
            if (curFocused != lastFocused)
            {
                logBox.AppendText($"[{time}] Focused now {(curFocused ? "YES" : "NO")}\r\n");
                lastFocused = curFocused;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            pollTimer.Stop();
            pollTimer.Dispose();
            base.OnFormClosing(e);
        }
    }
}
