```
# タスクバーへの最小化/タスクトレイへの格納
# monoでは2回目のタスクトレイ格納ですべてが消滅するかも
mcs Minimize.cs -r:System.Windows.Forms -r:System.Drawing
mono ./Minimize.exe

mcs BringFrontAndActiveWin.cs -r:System.Windows.Forms -r:System.Drawing
mono ./BringFrontAndActiveWin.exe

# タスクトレイ常駐によるトレイ格納・展開
# monoでも動く
mcs TrayDemoMethod1.cs -r:System.Windows.Forms -r:System.Drawing
mono ./TrayDemoMethod1.exe

# by o4-mini-high
```
