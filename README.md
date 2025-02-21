# DesktopTips
悬浮在桌面上的置底提示/提醒窗口

**使用示例**
`DesktopTips --xsa right --xwa right -x 250 --ysa bottom --ywa bottom -y 110 -t "Tips Content\nUse \\n to wrap lines"`

| 参数名 | 描述 | 有效值 |
|:--------------:|:------------|:-----:|
| `--xsa` `--x-screen-align` | x方向上的屏幕对齐位置。指定窗口应该与屏幕的哪里对齐。 | `left` `center` `right` |
| `--xwa` `--x-window-align` | x方向上的窗口对齐位置，与上面的参数配合使用。指定窗口的哪里对齐屏幕的哪里。比如：窗口的左侧对齐屏幕的中间。 | `left` `center` `right` |
| `-x` `--x` | 对齐偏移量。只有`--xsa`或者其等效参数值为`right`的时候向左偏移，其他时候向右偏移。单位为屏幕上的像素。 | 所有数值（建议整数） |
| `--ysa` `--y-screen-align` | y方向上的屏幕对齐位置。指定窗口应该与屏幕的哪里对齐。 | `top` `center` `bottom` |
| `--ywa` `--y-window-align` | y方向上的窗口对齐位置，与上面的参数配合使用。指定窗口的哪里对齐屏幕的哪里。比如：窗口的顶部对齐屏幕的中间。 | `top` `center` `bottom` |
| `-y` `--y` | 对齐偏移量。只有`--ysa`或者其等效参数值为`bottom`的时候向上偏移，其他时候向下偏移。单位为屏幕上的像素。 | 所有数值（建议整数） |