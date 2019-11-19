# Cross-platform-String-Resource-Utility
Android string resource style for all platforms: Windows, iOS, Android and Web Assembly

This project was created out of the love for the simplicity and high maintainability of Android app string resources, and the reservation about the complexity of Windows app string resources (i.e. Resources.resw).

This simple library may be useful for the following scenarios:
1. Reusing Android string resources for a Windows app.
2. Using strings in Android style for Windows apps even if there is no Android counterpart. 
3. Using Android style string resources  for cross-platform development targeting Windows, iOS, Android, WebAssembly with Uno Platform.

Setup instructions:
One can finish setup and start using and testing XRUtils in a few minutes if string files already exist.

Create a folder structure of Strings as following (similar to that of Android Projects):

	res
		values
			strings.xml
		values-es
			strings.xml
		values-fr
			strings.xml
		values-de
			strings.xml
		values-pt
			strings.xml
		values-ru
			strings.xml
		...
*IMPORTANT: each strings.xml must have its Build Action set to "Embedded resource".*
Folder res can be placed anywhere in a project. 
The res folder of an Android project can be used in a X-platform project by creating a linked folder, so you do not need to copy files:
	Mklink /J C:\MyXPlatformApp\Asset\res C:\MyAndroidApp\app\src\main\res

A library project referenced by other apps can have the res folder too, so many shared strings can be placd in a library. 

Folder "values" has the default string values that are used if values cannot be found from other language folders 
(e.g. values-es for Spanish, values-it for Italian, values-ru for Russian).

string.xml is in the Android string resource format:
```xml
<?xml version="1.0" encoding="utf-8"?>
<resources>
  <string name="thank_you">Спасибо!</string>
<resources>
```
Run the following as early as possible (e.g. in the constructor of App) or after language overriding
```xml
	XRUtils.InitializeInitialize(
            Type typeCalling,
            bool bReset = true,
            string sLanguageCodeParam = null);
```	    
Example:
            //Use the Spanish strings of a library
            XRUtils.Initialize(typeof(MyLibraryClass), true, "es");

            //Use strings of a project. They may overwrite strings with identical names from the library
            //It is important to set bReset = false to keep the strings from the library
			XRUtils.Initialize(GetType() true, "es", "es");   
	
(Obsolete for Silverlight WP)Run the following immediately after InitializeComponent() to handle the app bar localization. 
	`XResourceUtils.InitializeApplicationBar(ApplicationBar) 

In Xaml:
Add namespace XResource:
```
<Page
    x:Class="MyApp.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:MyApp"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
	...
    xmlns:XResource="using:XResourceUtils"
	...
    mc:Ignorable="d"
    RequestedTheme="Dark"
    x:DefaultBindMode="OneWay">
```

(Obsolete for Silverlight WP):
```
	<phone:PhoneApplicationPage
		...
		xmlns:XResource="clr-namespace:XResource;assembly=XResourceUtils"
		...
		DataContext ="{Binding RelativeSource={RelativeSource Self}}">
```

Add the converter XRConverter:
```
    <Page.Resources>
        <XResource:XRStringConverter x:Key="XRConverter"/>    
    </Page.Resources>
```

(Obsolete for Silverlight WP):
```
    <phone:PhoneApplicationPage.Resources>
        <XResource:XRStringConverter x:Key="XRConverter"/>
		...
    </phone:PhoneApplicationPage.Resources>
```

Use data binding in controls:
```
	<TextBlock Text="{x:Bind ConverterParameter=hello, Converter={StaticResource XRConverter}}"/>
	<Button Content="{x:Bind ConverterParameter=thank_you, Converter={StaticResource XRConverter}}"/>
```

(Obsolete for Silverlight WP) ApplicationBarIconButton and ApplicationBarMenuItem are different.  They cannot use data binding:
```    
    <ApplicationBarIconButton Text="param_name" IconUri="..." Click="..." />
    <ApplicationBarMenuItem Text="param_name" Click="..."/>
```

In C# code:
```
string sParam =  XRUtils.GetString("param_name")
```

The solution has a demo UWP app and a demo x-platform library in addition to the XResouceUtils.  You need only XResouceUtils for your own projects. 



			


