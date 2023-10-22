
<div align="center">
  <h1>MOS-Reunited Kingdom: V2 Launcher</h1>
  <img src="https://i.imgur.com/d7EGGYY.png" width="1920" alt="MOSRK Launcher" /></a>
</div>

The following is a repo for the [MOS:Reunited Kingdom V2.0](https://www.moddb.com/mods/divide-and-conquer) Launcher. The program is used to configure different aspects of the mod, ensure LAA is applied and to launch the mod.

  <img src="https://github.com/FynnTW/DaC_Launcher/assets/22448079/d84f2825-f72e-437b-9888-cf6c0b864c70" width="1920" alt="MOSRK Launcher" /></a>

## Configuration Options
The launcher currently provides the following configuration options

- **Permanent Arrows:** Ensures that arrows embedded in the ground stay for the duration of the battle and do not disappear. Has a small performance impact for low-end PCs.
- **Use alternate strategy textures:** Use the strategy map textures from the AGO mod, with a more realistic satellite art style.
   ![](https://cdn.discordapp.com/attachments/417013331614892042/1129740969659601016/image.png)
   ![](https://cdn.discordapp.com/attachments/417013331614892042/1129740970167128154/image.png)
   ![](https://cdn.discordapp.com/attachments/417013331614892042/1129740971060498502/image.png)
- **Use running up Javelin animations:** Use alternate javelin animations with which include a running-up, which look slightly more realistic at the cost of being more annoying to manage.
- **Use permanent arrows:** Arrows do not disappear from the ground, slight performance impact.
- **Skip Khazad-dûm expedition:** Skip the Khazad-dûm expedition at the start of the Khazad-dûm campaign.

## Download
The launcher is included as part of the MOS Reunited Kingdom install process but if for whatever reason you want to pick up the latest version, you can grab the latest artifact from the latest commit [here](https://github.com/MikeGolf45/MOSRK_Launcher/actions/workflows/build-mosrk-launcher.yml).

## Building from source

#### [Install .NET 6.0 and verify installation](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

`dotnet --version`

#### Install the dependencies
`dotnet restore`

#### Build the executable

`dotnet publish -c Release --no-restore`

#### Run the executable
`bin\Release\net6.0-windows\win-x86\publish\MOSRK_Launcher.exe`
