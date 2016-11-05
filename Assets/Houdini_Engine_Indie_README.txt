Q.How do I install Houdini?

A.

You will need to have "Admin" (administrator, root or sudo) access to install Houdini.
If you are new to Houdini or upgrading a full version (13 to 14 or 12.5 to 13), you will need to install the License Server tools in addition to the Houdini software.
If you have installed a version of Houdini and are to install a daily build of the same version, you do not need to install the License Server tools.
In general, uinstalling a previous version of Houdini is NOT required to intall a new.
Windows:

Run the Houdini setup program by double-clicking on the downloaded executable.
Follow the instructions in the installer. Once installed, the application is available by default through:
Start Menu -> All Programs -> Side Effects Software -> Houdini version
Launch Houdini to proceed to licensing.
Linux:
Open a terminal.
Unpack the downloaded tar.gz archive. For example, 
$ tar -xvzf houdini-14.0.201.13-linux_x86_64_gcc4.8.tar.gz
This should create a directory called houdini-14.0.201.13-linux_x86_64_gcc4.8/
Run the houdini.install script:
$ cd houdini-14.0.201.13-linux_x86_64_gcc4.8/
$ sudo ./houdini.install
* You can also double click on the houdini.install file with your mouse. It will run the installer in a terminal.
Follow the instructions in the installer.
The default installation wants to place the software in /opt/hfs14.0.201.13. If installing as a non-root user, you cannot install the License Server tools, AND you must change this default installation path. Example: /home/janedoe/hfs14.0.201.13
You should now be able to access Houdini in 2 ways:
Applications->Side Effects Software->Houdini 14.0.201.13
In a terminal(shell) type:
$ cd installation of hfs14.0.201.13; (Example: cd /opt/hfs14.0.201.13) 
$ source houdini_setup;
$ houdinifx (or houdini to start Houdini FX, or hescape to start Houdini) at the command prompt.
Launch Houdini to proceed to licensing.
Please see the Readme.txt that comes with the installed version of Houdini for additional information.
Mac:

Double-click on the downloaded .dmg file to unpack and start the installer.
Follow the instructions during the install process.
Launch Houdini to proceed to licensing.
The default installation will be in Applications on the main system drive (/Applications). You should now be able to access Houdini in 2 ways:

Go to the Applications folder on your main system drive (Macintosh HD).
Go to Houdini version and double click on the Houdini icon.
You can drag the Houdini icon to the dock to create a shortcut.
Go to the Applications folder on your main system drive (Macintosh HD). 
Go to Houdini version and double click on the Houdini Shell.terminal program. This will start a terminal(shell) with the Houdini environment set up for you.
Type "houdinifx" (or hmaster, houdini, hescape, hbatch) to launch the program.
You may also drag the Houdini Shell.terminal program to the dock to create a shortcut.