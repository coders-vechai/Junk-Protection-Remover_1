# Junk-Remover
.NET attributes cleaner/Junk remover for obfuscation.

# Usage
Drag & Drop on the application to clean it

# Mod Improvements
<pre>
- Very Basic Protection "Remover" (more like disabler but aight), Supports AntiDebugging, AntiTampering & AntiDumping
  |- Tested on a DotNetPatcher Obfuscated Application
- CUI Improvements & Fixes
</pre>

# Mod Notes:
This mod will likely trigger any AntiTampering methods that it hasnt detected, please use the original application if so.

(This mod is just to target basic obfuscators, it will likely not work on stronger obfuscators)

# Screenshots
Protections will be present as such if found by the mod
![disabledProtectionsOutput](https://i.imgur.com/ukcQMfq.png)

Debugging calls will be changed to strings to prevent the application to check for debuggers
![debuggerCallsToStrings](https://i.imgur.com/87sMGlO.png)

# Credits
- 0xd4d (now wtfsck) - <a href="https://github.com/0xd4d/dnlib/">dnlib</a>
- <a href="https://github.com/DevT02/">DevT02</a> - <a href="https://github.com/DevT02/Junk-Remover">Junk Remover (Original App)</a>
