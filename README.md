# IrisApp

## Overview
The purpose of the application is iris detection and identification. It's Windows-based application realized in WPF MVVM. It uses VERIEYE SDK (not included due to commercial license, from https://www.neurotechnology.com/verieye.html you can buy SDK or download 30-day SDK Trial).

## Description
IrisApp allows to detect iris from file or iris scanner (online preview and scan sources option are available). After source selection you must choose eye position you want to detect (left, right, unknown). Captured sample can be saved in database (both in sqlite file and directory with pictures of subject) or used to identification. You can also browse app logs, database content, go to subjects directories and set personal settings. User Manual in Polish is available in Instrukcja.pdf.

### Third Party Libraries
- MaterialDesignColors
- MaterialDesignThemes
- MicrosoftExpressionInteraction
- Newtonsoft.Json
- StyleCop.Analyzers
- System.Windows.Interactivity.WPF

![Home view](https://github.com/gradzka/IrisApp/blob/master/IrisApp/Assets/Screenshots/1.png)
![Database view](https://github.com/gradzka/IrisApp/blob/master/IrisApp/Assets/Screenshots/2.png)
![Settings view](https://github.com/gradzka/IrisApp/blob/master/IrisApp/Assets/Screenshots/3.png)
![About view](https://github.com/gradzka/IrisApp/blob/master/IrisApp/Assets/Screenshots/4.png)

## Attributions
- https://github.com/Abel13/NavigationDrawer
- https://github.com/Abel13/Pizzaria1
- https://github.com/blachniet/SimpleBorderlessWPFWindow
- https://github.com/google/material-design-icons
- https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit
- https://rachel53461.wordpress.com/2011/12/18/navigation-with-mvvm-2/
- https://www.technical-recipes.com/2018/navigating-between-views-in-wpf-mvvm/

## Credits
* Monika Grądzka
* Robert Kazimierczak
