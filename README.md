# Junk-Remover
.NET attributes cleaner/Junk remover for obfuscation.

(you currently need to use settings.txt from the og application because i forgot to provide it because im a dumbass)

# Usage
Drag & Drop on the application to clean it

# Mod Improvements
<pre>
- Very Basic Protection "Remover" (more like disabler but aight), Supports AntiDebugging, AntiTampering & AntiDumping
  |- Tested on a DotNetPatcher Obfuscated Application
- CUI Improvements & Fixes

TLDR: it just comments instructions that are protections so they don't load
</pre>

# Mod Notes:
This mod will likely trigger any AntiTampering methods that it hasnt detected, please use the original application if so.

(This mod is just to target basic obfuscators, it will likely not work on stronger obfuscators)

# Arguments (v1.0.1)
<pre>
- "-commentProtectionMethods" | Keeps protection as strings instead of clearing them (they're still disabled)
- "-showAll" | Shows every actions of the tool
</pre>

# Screenshots
<details>
  <summary>v1.1.0 & later</summary>
  ![basicView](https://i.imgur.com/mZBJT5Z.png)
</details>

<details>
  <summary>before v1.1.0</summary>
  
  ![basicView](https://i.imgur.com/3AVDZy5.png)

  (using "-showAll" (v1.0.1 <))

  ![showAll](https://i.imgur.com/2GCCkaS.png)
</details>

# Result
Protections will be presented as such if found by the mod

Version 1.0.1: you will have to add "-commentProtectionMethods" in the starting arguments to have the following, 
Version 1.1.0: set "Comment Protection Methods" to True

this option will/can increase the file size
![disabledProtectionsOutput](https://i.imgur.com/ukcQMfq.png)

Debugging calls will be changed to strings to prevent the application to check for debuggers
![debuggerCallsToStrings](https://i.imgur.com/87sMGlO.png)

`DllImport / ImplMap` Methods are renamed to `%FunctionNameInImportedDll%_%ImportedDllName%`
![implmap](https://i.imgur.com/odJ1LZc.png)

# Credits
- 0xd4d (now wtfsck) - <a href="https://github.com/0xd4d/dnlib/">dnlib</a>
- <a href="https://github.com/DevT02/">DevT02</a> - <a href="https://github.com/DevT02/Junk-Remover">Junk Remover (Original App)</a>
