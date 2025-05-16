using System;
using System.Windows.Forms;

class Minimize : Form
{
    NotifyIcon trayIcon;
    ContextMenuStrip trayMenu;
    Button btnMinimize;
    Button btnTray;

    public Minimize()
    {
        // フォーム初期設定
        this.Text = "Demo: Minimize & Tray";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ClientSize = new System.Drawing.Size(300, 120);

        // 「最小化」ボタン
        btnMinimize = new Button {
            Text = "Minimize",
            Left = 10, Top = 10, Width = 120
        };
        btnMinimize.Click += (s, e) => {
            this.WindowState = FormWindowState.Minimized;
        };
        this.Controls.Add(btnMinimize);

        // 「トレイに格納」ボタン
        btnTray = new Button {
            Text = "Minimize to Tray",
            Left = 150, Top = 10, Width = 120
        };
        btnTray.Click += (s, e) => MinimizeToTray();
        this.Controls.Add(btnTray);

        // トレイアイコンのコンテキストメニュー
        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("Restore", null, (s, e) => RestoreFromTray());
        trayMenu.Items.Add("Exit",    null, (s, e) => {
            trayIcon.Visible = false;
            Application.Exit();
        });

        // NotifyIcon 初期化
        trayIcon = new NotifyIcon {
            Text = "Tray Demo",
            Icon = System.Drawing.SystemIcons.Application,
            ContextMenuStrip = trayMenu,
            Visible = false
        };
        trayIcon.DoubleClick += (s, e) => RestoreFromTray();
    }

    // トレイに格納
    void MinimizeToTray()
    {
        trayIcon.Visible = true;
        this.Hide();
    }

    // トレイから復帰＆最前面
    void RestoreFromTray()
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;
        this.BringToFront();
        this.Activate();
        trayIcon.Visible = false;
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Minimize());
    }
}
