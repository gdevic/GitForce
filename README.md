# GitForce

GitForce is a GUI front-ends for the git command line tool. GitForce runs on both Windows and Linux.

## Features

* Single executable file with no need for installation, portable by design
* Intuitive GUI with efficient drag and drop for most operations
* Runs on Windows and Linux (using Mono) and on other OS-es with Mono support
* Easy way to manage multiple git repos
* Multiple sets of repos (workspaces)
* Custom tools, menus and extensions tailoring it to your specific needs
* Easy managing of SSH keys and remotes
* Integrated git command line interface
* Users familiar with Perforce will find this UI very familiar!

## Links

* Documentation [link](https://sites.google.com/site/gitforcetool/home)
* Forum [link](https://sourceforge.net/p/gitforce/discussion)

## Running GitForce.exe on Linux (and other Mono) environments

GitForce.exe needs Mono support, 'git' and one of the diff-tool packages:
```
$ sudo apt-get install mono-complete git meld
```

## Building from sources

On Windows OS, install MSVC 2015 (other versions may also work) and open GitForce.sln.

On Linux, use xbuild (part of mono-complete):
```
$ xbuild /p:Configuration=Release GitForce.sln
```

The current code has been developed (and built) on Windows. The same binary works on Linux (using Mono).
It should also work on other OS-es that have Mono support.

## Screenshot

<img src="https://a.fsdn.com/con/app/proj/gitforce/screenshots/gitforce-main-window.png" />
