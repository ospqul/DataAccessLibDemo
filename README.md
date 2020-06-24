# NDT Data Access Library Tutorial (C#)

The NDT Data Access Library is a COM component that allows users to customize the way their inspection data is presented and  processed. This library can be used to retrieve the A-scan, C-scan, and thickness data for custom applications, along with various acquisition  parameters from TomoView and OmniScan data files.

This tutorial will help you get started to build a C# application based on NDT Data Access Library from scratch.

The source codes for this tutorial are available in Github: [https://github.com/ospqul/DataAccessLibDemo](https://github.com/ospqul/DataAccessLibDemo).

Document Tutorial: [https://github.com/ospqul/DataAccessLibDemo/tree/master/doc](https://github.com/ospqul/DataAccessLibDemo/tree/master/doc).

You could clone a local repo: `git clone git@github.com:ospqul/DataAccessLibDemo.git `and check out the source code for each lesson.

```bash
$ git branch

  1_Setup_Environment
  2_Add_reference_and_open_file
  3_Get_channel_info
  4_Get_Beams
  5_Get_Gates_and_DataGroups
  6_Read_and_Plot_Ascan
  7_Read_and_Plot_Sscan
  
$ git checkout 1_Set_Environment
Switched to branch '1_Set_Environment'
```

#### Installation

1. Go to Software download page: https://www.olympus-ims.com/en/service-and-support/downloads/

2. Expand ***TomoView & SDK*** and download ***NDT Data Access Library***
3. Double-click .exe file and follow the instruction to install the software.

#### External Nuget Packages

- [Caliburn.Micro](https://github.com/Caliburn-Micro/Caliburn.Micro): A small, yet powerful framework, designed for building applications across all XAML platforms. Its strong support for MV* patterns will enable you to build your solution quickly, without the need to sacrifice code quality or testability.
- [Oxyplot](https://github.com/oxyplot/oxyplot): A cross-platform plotting library for .NET