using System;
using System.Windows.Forms;
using System.Drawing;

class TrayDemoMethod1 : Form
{
    NotifyIcon trayIcon;
    ContextMenuStrip trayMenu;
    Button btnTrayToggle;

    public TrayDemoMethod1()
    {
        // フォーム初期設定
        this.Text = "Demo: Tray (Method 1)";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ClientSize = new Size(320, 80);

        // コンテキストメニュー（常に表示）
        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("Restore", null, (s, e) => RestoreFromTray());
        trayMenu.Items.Add("Exit",    null, (s, e) => Application.Exit());

        // NotifyIcon を一度だけ作成し常に Visible
        trayIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            ContextMenuStrip = trayMenu,
            Visible = true,
            Text = "TrayDemoMethod1"
        };
        trayIcon.DoubleClick += (s, e) => RestoreFromTray();

        // トレイ格納／展開ボタン
        btnTrayToggle = new Button
        {
            Text = "Toggle Minimize/Restore",
            Left = 40,
            Top = 20,
            Width = 240
        };
        btnTrayToggle.Click += (s, e) => ToggleTray();
        this.Controls.Add(btnTrayToggle);
    }

    // タスクバーから隠す or 元に戻す
    void ToggleTray()
    {
        if (this.WindowState != FormWindowState.Minimized)
        {
            // 最小化してタスクバー非表示
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }
        else
        {
            RestoreFromTray();
        }
    }

    // 復帰処理
    void RestoreFromTray()
    {
        this.ShowInTaskbar = true;
        this.WindowState = FormWindowState.Normal;
        this.Activate();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TrayDemoMethod1());
    }
}

