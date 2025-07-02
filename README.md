# GitForce

GitForce is a GUI front-ends for the git command line tool. GitForce runs on both Windows and Linux.

## Features

* Intuitive GUI with drag and drop
* Single executable file and no need for installation
* Runs on Windows and Linux (using Mono) and perhaps on other OS-es with Mono support
* Create and manage multiple git repos
* Easy scan for local repos
* Supports multiple remote repos
* Multiple workspaces which are sets of repos
* Multiple sets of changes independent of a single “index” mandated by the git design
* Add custom tools to context menus to customize it to your specific needs
* Easy manage SSH keys and remotes
* See files’ revision history, stash, unstash etc.
* Manage local and remote branches
* Integrated git command line interface if you still want to use it
* Users familiar with Perforce will find this UI very familiar
* Simple, intuitive and light git wrapper for all normal tasks and workflows

## Links

* Documentation [link](https://baltazarstudios.com/software/gitforce)
* Issues [link](https://github.com/gdevic/GitForce/issues)

## Running GitForce.exe on Linux (and other Mono) environments

GitForce.exe needs Mono support, 'git' and one of the diff-tool packages:
```
$ sudo apt-get install mono-complete git meld
```

## Building from sources

On Windows OS, install MSVC 2019 with C# support and open GitForce.sln. The code targets .NET 4.5 (which 2019 supports)
If you use MSVC 2022, install .NET 4.5 framework. See dot.net.45 folder's readme.

On Linux, use xbuild (part of mono-complete):
```
$ xbuild /p:Configuration=Release GitForce.sln
```

The current code has been developed (and built) on Windows. The same binary works on Linux (using Mono).
It should also work on other OS-es that have Mono support.

## Screenshot

<img src="https://a.fsdn.com/con/app/proj/gitforce/screenshots/gitforce-main-window.png" />
