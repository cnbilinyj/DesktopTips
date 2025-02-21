# DesktopTips
Translucent tips/reminders floating on the desktop

`DesktopTips --xsa right --xwa right -x 250 --ysa bottom --ywa bottom -y 110 -t "Tips Content\nUse \\n to wrap lines"`

| Parameter Name | Description | Value |
|:--------------:|:------------|:-----:|
| `--xsa` `--x-screen-align` | x screen align. The window should align with the position of the screen. | `left` `center` `right` |
| `--xwa` `--x-window-align` | x window align, Used in conjunction with the parameters above. Align the target position on the screen with where in the window | `left` `center` `right` |
| `-x` `--x` | Align offset. Only when `--xsa` obtains the equivalent parameter value of `right`, offset to the left, and offset to the right at other times, in screen pixels. | All Number |
| `--ysa` `--y-screen-align` | y screen align. The window should align with the position of the screen. | `top` `center` `bottom` |
| `--ywa` `--y-window-align` | y window align, Used in conjunction with the parameters above. Align the target position on the screen with where in the window | `top` `center` `bottom` |
| `-y` `--y` | Align offset. Only when `--ysa` obtains the equivalent parameter value of `bottom`, offset to the top, and offset to the bottom at other times, in screen pixels. | All Number |