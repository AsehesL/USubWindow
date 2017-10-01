# USubWindow

一项简单的Unity Editor Window布局扩展工具，主要实现了基于树结构的子窗体系统，并通过反射的方式获得各窗口绘制方法，只需简单的添加Attribute即可方便的绘制一个子窗口。

![示例1](doc/doc1.gif)

示例：

```
[EWSubWindow("SunWinA", EWSubWindowIcon.Game)]
private void SubWinA(Rect main)
{
	GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinA");

}

[EWSubWindow("Grid", EWSubWindowIcon.Game, true, SubWindowStyle.Grid)]
private void SubWinA(Rect main)
{
	if (GUI.Button(new Rect(main.x, main.y, 100, 20), "Btn"))
	{

	}
}
```

更新：

1.自定义子窗体图标

2.增加Preview样式和Grid样式

![](doc/doc2.gif)

3.helpbox和toolbar

![](doc/doc3.gif)

4.自定义ToolBar和MsgBox

5.保存Layout

![](doc/doc4.jpg)

5.自定义子窗体Drawer

6.子窗体Drawer生命周期

7.支持子窗体锁定和自定义右上角下拉框接口

![](doc/doc5.png)

![](doc/doc6.jpg)

6.子窗体序列化支持

7.自定义MsgBox Drawer

8.动态添加窗口