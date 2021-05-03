using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace Anti_De4dot_remover
{
    // Token: 0x02000002 RID: 2
    internal class Program
    {
        public static int countofths = 0;
        public static List<string> CAttributes = new List<string>()
            {
                "AssemblyInfoAttribute",
                "BabelAttribute",
                "BabelObfuscatorAttribute",
                "ConfusedByAttribute",
                "CryptoObfuscator",
                "DotfuscatorAttribute",
                "dotNetProtector",
                "DotNetPatcherPackerAttribute",
                "DotNetPatcherObfuscatorAttribute",
                "EMyPID_8234_",
                "MaxtoCodeAttribute",
                "NineRays.Obfuscator.Evaluation",
                "NETGuard",
                "NETReactorAttribute",
                "ObfuscatedByAgileDotNetAttribute",
                "ObfuscatedByCliSecureAttribute",
                "ObfuscatedByDotNetPatcher",
                "ObfuscatedByGoliath",
                "OiCuntJollyGoodDayYeHavin_____________________________________________________",
                "OrangeHeapAttribute",
                "PoweredByAttribute",
                "ProcessedByXenocode",
                "ProtectedWithCryptoObfuscatorAttribute",
                "Reactor",
                "WTF",
                "YanoAttribute",
                "ZYXDNGuarder"
            };
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        private static bool printAll = false, commentProtections = false;
        private static void Main(string[] args)
        {
            Console.Title = "Junk Remover by OFF_LINE";
            Console.ForegroundColor = ConsoleColor.Red;
            string text = "";
            bool preserveEverything = true;
            string[] shit = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\settings.txt");
            if (!shit[0].Contains("true"))
                preserveEverything = false;            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            string sb = @"       __            __      ____                                     
      / /_  ______  / /__   / __ \___  ____ ___  ____ _   _____  _____
 __  / / / / / __ \/ //_/  / /_/ / _ \/ __ `__ \/ __ \ | / / _ \/ ___/
/ /_/ / /_/ / / / / ,<    / _, _/  __/ / / / / / /_/ / |/ /  __/ /    
\____/\__,_/_/ /_/_/|_|  /_/ |_|\___/_/ /_/ /_/\____/|___/\___/_/     
                                                                    ";
            Console.WriteLine(sb);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Original Authors: Prab + illuZion");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" Mod by Misonothx");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("  |- (Very basic) AntiTampering, AntiDebugger & AntiDump Protection Remover");
            Console.WriteLine("  |- CUI Improvements & Fixes");
            Console.WriteLine();
            if (args.Count() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" No arguments found!");
                goto end;
            }
            for (int x = 0; x < args.Count(); x++)
            {
                if (File.Exists(args[x]))
                {
                    text = args[x];
                    continue;
                }
                switch (args[x])
                {
                    case "-commentProtectionMethods":
                        printAll = true;
                        break;
                    case "-showAll":
                        commentProtections = true;
                        break;
                }
            }
            if (text == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" No file set!");
                goto end;
            }
            try
            {
                Program.module = ModuleDefMD.Load(text);
                Program.asm = Assembly.LoadFrom(text);
                Program.Asmpath = text;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Not a .NET Assembly!");
                Console.ResetColor();
                Console.WriteLine(" Press any key to exit...");
                System.Environment.Exit(0);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Filtering useless Nop instructions..");
            try
            {
                Execute(module);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Failed to filter Nop instructions! (" + ex.Message + ")");
                goto end;
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" " +removed +  " Nop instructions removed!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Removing fake attributes...");
            removeshit();
            removeshit();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" " + countofths + " FakeAttributes/AntiDe4dot cases removed!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Checking for possible protections...");
            Console.WriteLine();
            tryClearProtections();
            string text3 = string.Format("{0}_noJunk{1}", Path.GetFileNameWithoutExtension(Path.GetFullPath(Asmpath)), Path.GetExtension(Asmpath));
            ModuleWriterOptions moduleWriterOptions = new ModuleWriterOptions(Program.module);
            if (preserveEverything)
            {
                moduleWriterOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
            }
            moduleWriterOptions.Logger = DummyLogger.NoThrowInstance;
            NativeModuleWriterOptions nativeModuleWriterOptions = new NativeModuleWriterOptions(Program.module);
            if (preserveEverything) {
                nativeModuleWriterOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
            }
            nativeModuleWriterOptions.Logger = DummyLogger.NoThrowInstance;
            bool isILOnly = Program.module.IsILOnly;
            if (!printAll)
            {
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Now saving " + Path.GetFileNameWithoutExtension(args[0]) + "-IL" + Path.GetExtension(args[0]) + " (IL)...");
            try
            {
                if (isILOnly)
                {
                    Program.module.Write(text3, moduleWriterOptions);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" (IL) '" + Path.GetFileNameWithoutExtension(args[0]) + "-IL" + Path.GetExtension(args[0]) + "' successfully saved!");
                }
                else
                {
                    Program.module.NativeWrite(text3, nativeModuleWriterOptions);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" (Native) '" + Path.GetFileNameWithoutExtension(args[0]) + "-Native" + Path.GetExtension(args[0]) + "' successfully saved!");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Failed to save application! (" + ex.Message + ")");
            }
            end:
            Console.ResetColor();
            Console.WriteLine(" Press any key to exit...");
            Console.ReadKey();
            System.Environment.Exit(0);
        }
        private static IList<TypeDef> lista(ModuleDef A_0)
        {
            return A_0.Types;
        }

        public static void removeshit()
        {
            for (int i = 0; i < Program.module.Types.Count; i++)
            {
                TypeDef typeDef = Program.module.Types[i];
                if (typeDef.HasInterfaces)
                {
                    for (int jic = 0; jic < typeDef.Interfaces.Count; jic++)
                    {
                        if (typeDef.Interfaces[jic].Interface != null)
                        {
                            if (typeDef.Interfaces[jic].Interface.Name.Contains(typeDef.Name) || typeDef.Name.Contains(typeDef.Interfaces[jic].Interface.Name))
                            {
                                Program.module.Types.RemoveAt(i);
                                countofths++;
                            }
                        }
                    }
                }
            }
            if (module.HasCustomAttributes)
            {
                for (int x = 0; x < module.CustomAttributes.Count; x++)
                {
                    if (CAttributes.Contains(module.CustomAttributes[x].AttributeType.Name))
                    {
                        printRemoved(module.CustomAttributes[x].AttributeType.Name, "CustomAttribute");
                        module.CustomAttributes.RemoveAt(x);
                        x -= 1;
                    }
                }
                for (int x = 0; x < module.Types.Count; x++)
                {
                    if (CAttributes.Contains(module.Types[x].Name))
                    {
                        printRemoved(module.Types[x].Name, "class");
                        module.Types.Remove(module.Types[x]);
                    }
                }
            }
            foreach (var type in module.Types.ToList().Where(t => t.HasInterfaces))
            {
                for (var i = 0; i < type.Interfaces.Count; i++)
                {
                    if (type.Interfaces[i].Interface.Name.Contains(type.Name) || type.Name.Contains(type.Interfaces[i].Interface.Name))
                    {
                        module.Types.Remove(type);
                        countofths++;
                    }
                }
            }
        }
        private static bool IsNopBranchTarget(MethodDef method, Instruction nopInstr)
        {
            var instr = method.Body.Instructions;
            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode.OperandType == OperandType.InlineBrTarget || instr[i].OpCode.OperandType == OperandType.ShortInlineBrTarget && instr[i].Operand != null)
                {
                    Instruction instruction2 = (Instruction)instr[i].Operand;
                    if (instruction2 == nopInstr)
                        return true;
                }
            }
            return false;
        }
        public static int removed = 0;

        public static void printDisabled(string data, string moreInfo = "")
        {
            if (!printAll)
            {
                return;
            }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(" [Disabled]: ");
            Console.ForegroundColor = ConsoleColor.White;
            if (moreInfo != "")
            {
                Console.Write(data);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(" (" + moreInfo + ")");
                return;
            }
            Console.WriteLine(data);
        }

        public static void printRemoved(string data, string moreInfo = "")
        {
            if (!printAll)
            {
                return;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" [Removed]: ");
            Console.ForegroundColor = ConsoleColor.White;
            if (moreInfo != "")
            {
                Console.Write(data);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(" (" + moreInfo + ")");
                return;
            }
            Console.WriteLine(data);
        }

        public static void printRenamed(string data, string moreInfo = "")
        {
            if (!printAll)
            {
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" [Renamed]: ");
            Console.ForegroundColor = ConsoleColor.White;
            if (moreInfo != "")
            {
                Console.Write(data);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(" (" + moreInfo + ")");
                return;
            }
            Console.WriteLine(data);
        }

        public static void Execute(ModuleDefMD module)
        {
            foreach (var type in module.Types.Where(t => t.HasMethods))
            {
                foreach (var method in type.Methods.Where(m => m.HasBody && m.Body.HasInstructions))
                {
                    if (method.HasBody)
                    {
                        var instr = method.Body.Instructions;
                        for (int i = 0; i < instr.Count; i++)
                        {
                            if (instr[i].OpCode == OpCodes.Nop &&
                                !IsNopBranchTarget(method, instr[i]) &&
                                !IsNopSwitchTarget(method, instr[i]) &&
                                !IsNopExceptionHandlerTarget(method, instr[i]))
                            {
                                instr.RemoveAt(i);
                                printRemoved("Nop Instruction", type.Name + "." + method.Name);
                                removed++;
                                i--;
                            }
                        }
                    }
                }
            }
        }

        private static bool IsNopSwitchTarget(MethodDef method, Instruction nopInstr)
        {
            var instr = method.Body.Instructions;
            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode.OperandType == OperandType.InlineSwitch && instr[i].Operand != null)
                {
                    Instruction[] source = (Instruction[])instr[i].Operand;
                    if (source.Contains(nopInstr))
                        return true;
                }
            }
            return false;
        }

        private static bool IsNopExceptionHandlerTarget(MethodDef method, Instruction nopInstr)
        {
            bool result;
            if (!method.Body.HasExceptionHandlers)
                result = false;
            else
            {
                var exceptionHandlers = method.Body.ExceptionHandlers;
                foreach (var exceptionHandler in exceptionHandlers)
                {
                    if (exceptionHandler.FilterStart == nopInstr ||
                        exceptionHandler.HandlerEnd == nopInstr ||
                        exceptionHandler.HandlerStart == nopInstr ||
                        exceptionHandler.TryEnd == nopInstr ||
                        exceptionHandler.TryStart == nopInstr)
                        return true;
                }
                result = false;
            }
            return result;
        }

        private static void renameImplMapFunctions()
        {
            foreach (TypeDef t_ in module.Types)
            {
                foreach (MethodDef method in t_.Methods)
                {
                    if (method.HasImplMap)
                    {
                        method.Name = method.ImplMap.Module.Name.ToLower() + "_" + method.ImplMap.Name;
                    }
                }
            }
        }

        private static void tryClearProtections()
        {
            string implMapMethodName = "";
            foreach (TypeDef t_ in module.Types)
            {
                foreach (MethodDef method in t_.Methods)
                {
                    if (method.HasImplMap)
                    {
                        if (method.ImplMap.Name == "VirtualProtect" && method.ImplMap.Module.Name.ToString().ToLower().Contains("kernel32"))
                        {
                            implMapMethodName = method.Name;
                            break;
                        }
                    }
                }
                if (implMapMethodName != "")
                {
                    break;
                }
            }
            foreach (TypeDef t_ in module.Types)
            {
                foreach (MethodDef method in t_.Methods)
                {
                    List<string> instString = new List<string>();
                    if (!method.HasBody) { continue; }
                    for (int x = 0; x < method.Body.Instructions.Count; x++)
                    {
                        Instruction inst = method.Body.Instructions[x];
                        if (inst.OpCode.Equals(OpCodes.Newobj) && method.Body.Instructions[x+1].OpCode.Equals(OpCodes.Throw))
                        {
                            if (inst.Operand.ToString().Contains("BadImageFormatException"))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(" AntiTampering Protection Found!");
                                foreach (Instruction inst_ in method.Body.Instructions)
                                {
                                    instString.Add("IL_" + int.Parse(inst_.Offset.ToString("X"), System.Globalization.NumberStyles.HexNumber) + "       " + inst_.OpCode.ToString() + ((inst_.Operand == null) ? null : "   " + inst_.Operand.ToString()));
                                }
                                method.Body.Instructions.Clear();
                                if (commentProtections)
                                {
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, "[Junk Remover]: Function Cleared! (Reason: AntiTampering Protection)"));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, ""));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, "Original Instructions (" + instString.Count + "):"));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, ""));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, "-----------------------------------------------------------------------------------------"));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, ""));
                                    for (int i = 0; i < instString.Count; i++)
                                    {
                                        method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, instString[i]));
                                    }
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ret));
                                }
                                printDisabled("AntiTampering Protection", t_.Name + "." + method.Name);
                                if (printAll)
                                {
                                    Console.WriteLine();
                                }
                                break;
                            }
                        }
                        if (inst.OpCode.Equals(OpCodes.Call))
                        {
                            if (inst.Operand.ToString().Contains("System.Diagnostics.Debugger"))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(" AntiDebugging Protection Call Found!");
                                string op_ = inst.Operand.ToString();
                                inst.OpCode = OpCodes.Ldstr;
                                inst.Operand = op_;
                                printDisabled("System.Diagnostics.Debugger Call", t_.Name + "." + method.Name + ", " + op_.Split(':')[2].Replace("()", null).Replace("get_", null));
                            }
                            if (inst.Operand.ToString().Contains(implMapMethodName))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(" AntiDumping Protection Found!");
                                foreach (Instruction inst_ in method.Body.Instructions)
                                {
                                    instString.Add("IL_" + int.Parse(inst_.Offset.ToString("X"), System.Globalization.NumberStyles.HexNumber) + "       " + inst_.OpCode.ToString() + ((inst_.Operand == null) ? null : "   " + inst_.Operand.ToString()));
                                }
                                method.Body.Instructions.Clear();
                                if (commentProtections)
                                {
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, "[Junk Remover]: Function Cleared! (Reason: AntiDump Protection)"));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, ""));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, "Original Instructions (" + instString.Count + "):"));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, ""));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, "-----------------------------------------------------------------------------------------"));
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, ""));
                                    for (int i = 0; i < instString.Count; i++)
                                    {
                                        method.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, instString[i]));
                                    }
                                    method.Body.Instructions.Add(new Instruction(OpCodes.Ret));
                                }                                
                                printDisabled("AntiDumping Protection", t_.Name + "." + method.Name);
                                if (printAll)
                                {
                                    Console.WriteLine();
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        // Token: 0x04000002 RID: 2
        public static string Asmpath;

        // Token: 0x04000003 RID: 3
        public static ModuleDefMD module;

        // Token: 0x04000004 RID: 4
        public static Assembly asm;

    }
}
