SatellaWave - version 0.1 Beta
By LuigiBlood

-----
This is the full version of SatellaWave, and pretty different of the Lite version:
It's a BS File Maker, with all parameters, and more.

This beta version only makes these types of BS files:
- Channel Map
- Town Status
- File/Folder/Directory
- Welcome
And can also split ROM files (ROM Splitter), and split to the new format of BSX Files (SX2 Splitter).
(Note to this point: All files made with SatellaWave non-Lite version, will be for bsnes-sx2 v008 and newer.)

I'm sorry for having to make such a confusing Readme.txt.
I'm bad at explaining things.

-----
Changelog
-----
0.1 Beta:
Initial Release


-----
How to use
-----
1/ Channel Map

Making Channel Maps should be easy despite the not so understandable GUI.
Keep in mind the Software and Hardware Channel up there.

First type the Software Channel in this format "N.N.N.N", N from 0 to 255 (just like IPs).
Then the Hardware Channel as a 16-bit Hex Value (From "0000" to "FFFF"), remember that in bsnes-sx2; HW Channel 0000 is the Time.

Set the Fragment Interval (not really needed though for bsnes-sx2), the Auto-Start, and the Download Target. This is very important.
Then Click "Add Channel".

Repeat the process when you want.
If you want to delete Channels, select the channel you want to delete on the list, and click "Delete Channel".

Also, YOU MUST ADD THESE CHANNELS FIRST BEFORE ADDING YOUR OWN; THIS IS FOR THE BS-X, you can choose whatever HW Channel you want.
- 1.1.0.4 / Auto-Start: No / Target: WRAM (Welcome Text, optional)
- 1.1.0.5 / Auto-Start: No / Target: WRAM (Town Status)
- 1.1.0.6 / Auto-Start: No / Target: WRAM (Directory)
- 1.1.0.7 / Auto-Start: No / Target: WRAM (Patch, optional)
- 1.1.0.8 / Auto-Start: No / Target: WRAM / HW Channel: 0000 (Time, optional)
- 1.2.0.48 / Auto-Start: No / Target: WRAM / HW Channel: 0000 (Time, optional)


2/ Informations about File-Folder-Expansion-Directory

First thing you need to know:
Don't forget the IDs, especially for the Town File.

a. Files
These BS files are particular; a "File" contains infos for the BS ROM you will be downloading.
It can also be an Item to buy.
For accurate BS File making, you need to split a BS ROM:
1. Go to Special tab, and click ROM Splitter.
2. Choose the ROM you want to split, and click OK.
3. Go make your File in the File tab.
4. DON'T FORGET THE SOFTWARE CHANNEL ON THE TOP THAT THE FILE WILL USE FOR DOWNLOADING. IT MUST NOT FINISH BY N.N.0.0.
4a. IF YOU NEED TO MAKE IT DOWNLOAD OTHER PARTS, ADD A SOFTWARE CHANNEL FOR THE ADDITIONAL FILE. AGAIN, NOT FINISH BY N.N.0.0.
4b. For 4a: Think of it like a Russian Doll.
5. Click "Save..." button.
6. Save your File (NOT AS A BSX????-?.bin file, you will use it later on SatellaWave)
7. It will also ask for the file you planned to download, load the Splitted File.

And it's done. But you have more to do.

b. Folders
Those folders contains BS Files, so when you finished making all files needed for download:
1. After naming and choosing stuff, go add the BS File to the folder by clicking Add.
1a. If you want to add files if the BS ROM to download is splitted to several parts,
	Finish this folder first with the 1st part file. After saving it: Check "Include File" and add the other parts (NOT THE 1ST!).
2. After doing all you want: Save the Folder. (NOT AS A BSX????-?.bin file again.)

c. Directories
Those Directories contains Folders and Expansion files.
Add the Folders and Expansion files you have made, and click Save.
Not as a BSX????-?.bin again.

Up to this point, go to the Special Tab.
Browse the folder where you want to split the Directory file.
Set the Hardware Channel to what the Channel Map expects to get the Directory.
Then click SX2 Splitter, and choose the Directory file.
It will split it to compatible BSX????-?.bin files for bsnes-sx2 v008+.

And you're not done yet. Now split the BS ROM file and don't forget to change the HW Channel to the one that should get to your file.
(Note: When it's too big for download, it will increment the HW Channel)

Up to this point, you should be able to make everything needed.

-----
Thanks to
-----
Byuu - For making bsnes.
nocash - For his well made documentation (of BS-X, XBAND...)
ikari_01 - For his Memory Pack documentation (and adding BS-X support for sd2snes)
d4s - For his Satellaview Register Testing Doc, mostly.
p4plus2 - For his BS-X BIOS disassembly. (Even if I still don't have it at this time of writing.)
Kiddo - If he wasn't here, i wouldn't do all this, and thanks for making the SatellaBlog.
Matthew Callis - For hosting the BS-X Project website, and for making superfamicom.org.
