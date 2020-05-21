## 2 Add Reference And Open File

#### 2.1 Add reference

1. Find NDT Data Access Library installation folder, the default path is `C:\Program Files\OlympusNDT\NDT Data Access Library 1.12`.
2. Copy `RDTiff19.dll`, `RDTiffDataAccess.dll`, and `RDTiffDataFile.dll` to your project folder.
3. Right click `DataAccessLibDemo` project -> select `Add` -> select `Reference`
4. Click `Browse` , add `RDTiffDataAccess.dll`, and `RDTiffDataFile.dll`.
5. Change Target Platform to 64bit or 32bit, whichever matches the Data Access Library version that you have installed in [this step](https://github.com/ospqul/DataAccessLibDemo#installation).

#### 2.2 Add File Path test box

Let's add a text box on GUI to input file path.

```xaml
# ShellView.xaml

<Label Content="FilePath"/>
<TextBox x:Name="FilePath" Width="300"/>
```

Bind `FilePath` value in `ShellViewModel.cs`.

```c#
// ShellViewModel.cs
using Caliburn.Micro;

class ShellViewModel : Screen
{
    private string _filePath;

    public string FilePath
    {
        get { return _filePath; }
        set 
        {
            _filePath = value;
            // To notify GUI that FilePath's value has been updated
            NotifyOfPropertyChange(() => FilePath);
        }
    }
}
```

We will use the sample data in NDT Data Access Library installation folder.

Default file path: `C:\Program Files\OlympusNDT\NDT Data Access Library 1.12\Samples\x64\DATA FILE - WELD.opd`

We can assign this default path in `ShellViewModel` constructor for convenience.

```c#
// ShellViewModel.cs

// Ctor
public ShellViewModel()
{
    FilePath = @"C:\Program Files\OlympusNDT\NDT Data Access Library 1.12\Samples\x64\DATA FILE - WELD.opd";
}
```

#### 2.3 Add Open File Button

First, add a button on GUI.

```xaml
# ShellView.xaml

<Button x:Name="OpenFile" Content="Open" Width="50" Margin="5,0"/>
```

Bind button in `ShellViewModel.cs`.

```c#
// ShellViewModel.cs
using RDTiffDataAccess;

public RDTiffDataFile.RDTiffDataFile dataFile { get; set; }

public void OpenFile()
{
    RDTiffData rdtData = new RDTiffData();
    dataFile = rdtData.RDTiffDataFile;
    dataFile.OpenFile(FilePath);
}
```

Add `Dispose()` to close data file safely when exit this application.

```c#
// ShellViewModel.cs

class ShellViewModel : Screen, IDisposable
{
    public void Dispose()
    {
        dataFile.CloseFile();
    }
}
```

#### 2.4 Source Code

Run `git checkout 2_Add_reference_and_open_file ` .