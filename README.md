If you like what I do and want to support me, send a $ tip on ko-fi!  
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/I2I5ZZBRH)
# Thumper Custom Level Editor  
[Download the latest release](https://github.com/CocoaMix86/Thumper-Custom-Level-Editor/releases)  
How to use the editor:
https://youtu.be/bxDgG2yeUk8  
You can find help and additional resources on the Thumper Discord here:  
<a href="https://discord.gg/mGn8RvftTJ"><img src="https://discordapp.com/api/guilds/380672655969353728/widget.png?style=banner3" alt="Discord server invite link"/></a>

## First Time Running  
When you first launch TCLE 2.2, you will be greeted with this screen  
![image](https://github.com/user-attachments/assets/dae30ea5-8cdd-451c-9269-d8cfd1003eb9)  
You will want to say YES to this, unless you don't want your "track_objects.txt" file to be updated (in case you have custom objects in it you don't want overwritten)  
![image](https://github.com/user-attachments/assets/80aa5e21-0b54-41fb-9aaa-56e9a78437f7)  
Feel free to read the changelog to get a feel of what has all changed!

## The UI  
The interface may seem confusing and busy at first, but you will learn it pretty quick. It is not difficult.  
The majority of buttons can be hovered on to display a tooltip about what it does.  
![image](https://github.com/user-attachments/assets/0aa8d1ea-20a7-4f0c-b2d7-cfd0bb80dfeb)  
Some areas also have Blue question marks which can also be hoevered or clicked.  
![image](https://github.com/user-attachments/assets/bc0d4120-a108-464f-a2a9-1cda22cb7d6a)  
The entire interface can be moved and resized as you wish. The areas in between panels is draggable.  
![image](https://github.com/user-attachments/assets/44169e7a-c46d-4f7e-8667-c15d3ff0945e)  
![image](https://github.com/user-attachments/assets/98d4501b-9648-46c8-bbf9-20eade7bc5eb)
In addition, the panels can be undocked and moved about freely by pressing the blue arrow in their title bars to undock it. And then it can be dragged around by its title bar.  
![image](https://github.com/user-attachments/assets/63b3f68c-9254-4529-88f4-7283ff46dea0)  
![image](https://github.com/user-attachments/assets/20f8b901-c899-4547-a52b-843afb0c2318)  
And can be resized using the control in the bottom-right  
![image](https://github.com/user-attachments/assets/dffb77fc-e318-4fec-90fa-48622c1ea24a)  
If a dock is empty, you can chose to dock another panel in its spot  
![image](https://github.com/user-attachments/assets/0303ccad-a2bd-4149-961f-47b9d4149f8c)  
And if you mess up your view a lot and its a big pain, go to `VIEW > Reset Docks` to get back to normal  
![image](https://github.com/user-attachments/assets/83553301-ba42-4084-9dce-b8385544bb7d)  
![image](https://github.com/user-attachments/assets/ba9494f2-0bfe-4641-9d2a-5a11383feda0)  

You can customize the look and keybinds of the Editor too. Go to `EDIT > Preferences`  
![image](https://github.com/user-attachments/assets/00ad0e40-e057-47be-9541-6761ac5816b8)  
Each panel can be colored differently. And to change a keybind, just click on it.  
![image](https://github.com/user-attachments/assets/c7c603ad-19f2-40b8-8af1-2e41723269cd)  
![image](https://github.com/user-attachments/assets/e5d298b0-df4f-4817-b8f5-187e48af093c)




## Creating your first level  
Go to FILE > New Level Folder  
![image](https://github.com/user-attachments/assets/3e90b104-f198-4883-bed4-0f29b7c6ce4f)  
File in the details of your level however you like.  
![image](https://github.com/user-attachments/assets/a9154a2b-286e-4786-86d9-070e96196ad0)  
Keep in mind, when you set the file path, a NEW folder will be created inside that location using the Level Name  
![image](https://github.com/user-attachments/assets/bccd1531-23df-44d5-a883-3f7da4d5a15f)  
So then I will have `X:\Thumper\levels\My First Level\` and all the relevant files for the custom level will be in this location.  
Then click SAVE.  
### Understanding the File Hierarchy
There are a few different files that make up a custom level, and it is important to know how they all related to each other. The file types are  
- Leaf ![leaf](https://github.com/user-attachments/assets/ddd99306-c241-4e61-9be4-f67269f28511)  
- Lvl ![lvl](https://github.com/user-attachments/assets/4f14992a-5faf-4a09-8e97-bf5f6d968159)  
- Gate ![gate](https://github.com/user-attachments/assets/1466b7b0-4cde-44a9-83ba-6bb9075459ab)  
- Master ![master](https://github.com/user-attachments/assets/c63a7dc4-d2c7-48cb-a884-1fa03fe1a315)  
- Sample ![sample](https://github.com/user-attachments/assets/e6f20d2f-6af6-4f5a-9867-e6b49c706a0a)
  
- __**Leaf** files__ are the building blocks of everything. They contain all the data of where objects will spawn and how the track moves and bends. You can think of them as a "pattern" or a "bar" in music. They are typically short, which enables them to be reused if you want the same pattern to show up multiple times throughout a level.  
- __**Lvl** files__ combined multiple leafs into a single sequence, and are typically used to denote 1 single sublevel. Lvl files are also where you can add tunnels, loop tracks, and do volume sequencing.  
- __**Gate** files__ are boss levels. They take 4-5 lvls (Depending on the boss). Each lvl becomes 1 phase of the boss.  
- __**Master** files__ are used to structure the entirety of the custom level. You add lvl and gate files here to setup your sublevels in order. The master also is where you can set level BPM and skybox, as well as checkpoint and rest lvls.  
- __**Sample** files__ hold all of the samples your custom level may reference. If it doesn't exist in a samp file, your level cannot play that sound. You can also add custom sounds/external audio through the Sample Editor panel. There are preset sample packs you can pick from when creating your custom level. Each one comes from a different main game level and work at different BPMs to sound correct.

### Creating Files
To add a file to the project, click the blue file icon in the Working Folder panel, and choose one of the file types  
![image](https://github.com/user-attachments/assets/66d23c01-97e4-4058-9f40-c8a738cda7ad)  
Name it however you want, and then you will see it shows up in its respective editor panel, and the Working Folder. I chose Leaf in this exampe, but it works the same for all the others.  
![image](https://github.com/user-attachments/assets/57308949-0990-4f2b-9362-9a87032fe7f7)  


# Mod Loader  
Along with the level editor, you will need the Mod Loader  
[github page](https://github.com/CocoaMix86/Thumper-Modding-Tool-resharp)
