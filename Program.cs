using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System.Text.RegularExpressions;

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
        private static bool printAll = false, commentProtections = false, fixBasicMath = true, disableProtections = true, preserveEverything = true, noBase64Decode = true, editArgsFirst = true, removeNops = true, removeAntiDe4dots = true, renameDllImports = true;
        private static int controlIndex = 0, mathFixed = 0;
        private static bool[] boolVars = new bool[9];
        private static void editArgs()
        {
            SplashScreen();
            if (editArgsFirst)
            {
                boolVars[0] = removeNops;
                boolVars[1] = removeAntiDe4dots;
                boolVars[2] = renameDllImports;
                boolVars[3] = disableProtections;
                boolVars[4] = !noBase64Decode;
                boolVars[5] = commentProtections;
                boolVars[6] = printAll;
                boolVars[7] = !preserveEverything;
                boolVars[8] = fixBasicMath;
                editArgsFirst = false;
            }            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Options:");
            Console.ResetColor();
            Console.WriteLine("  ---------------------------------------|---CLEANING OPTIONS---Original---");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  |- Remove Nop Instructions:    ");
            PrintFromBool(boolVars[0], controlIndex, 0);
            Console.Write("  |- Remove AntiDe4dots:         ");
            PrintFromBool(boolVars[1], controlIndex, 1);
            Console.ResetColor();
            Console.WriteLine("  ---------------------------------------|---CLEANING OPTIONS---Mod--------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  |- Rename 'DllImport' Methods: ");
            PrintFromBool(boolVars[2], controlIndex, 2);
            Console.Write("  |- Disable Protection Methods: ");
            PrintFromBool(boolVars[3], controlIndex, 3);
            Console.Write("  |- Fix Simple Calculations:    ");
            PrintFromBool(boolVars[8], controlIndex, 4);
            Console.Write("  |- Decode Base64 Strings:      ");
            PrintFromBool(boolVars[4], controlIndex, 5, "(takes a while, not stable)", ConsoleColor.DarkYellow);
            Console.ResetColor();
            Console.WriteLine("  ---------------------------------------|---MISC OPTIONS-------Mod--------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  |- Comment Protection Methods: ");
            PrintFromBool(boolVars[5], controlIndex, 6, "(increases file size)", ConsoleColor.DarkYellow);
            Console.Write("  |- Show All Actions:           ");
            PrintFromBool(boolVars[6], controlIndex, 7, "(console will be flooded)", ConsoleColor.Yellow);
            Console.ResetColor();
            Console.WriteLine("  ---------------------------------------|---SAVING OPTIONS-----Original---");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  |- Don't Preserve Metadata:    ");
            PrintFromBool(boolVars[7], controlIndex, 8, "(not recommended to use)", ConsoleColor.DarkRed);
            Console.WriteLine();
            Console.Write("     Application Type: ");
            if (module.IsILOnly) { Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine("IL-Only"); } else { Console.ForegroundColor = ConsoleColor.DarkYellow; Console.WriteLine("Native"); }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(" Press Arrow Keys to edit Arguments...");
            Console.ResetColor();
            Console.Write(" (anything else = start cleaning)");
            ConsoleKeyInfo key_ = Console.ReadKey();
            Console.Clear();
            switch (key_.Key)
            {
                case ConsoleKey.UpArrow:
                    if (controlIndex != 0) { controlIndex--; }
                    break;
                case ConsoleKey.DownArrow:
                    if (controlIndex != boolVars.Count() - 1) { controlIndex++; }
                    break;
                case ConsoleKey.LeftArrow:
                    boolVars[controlIndex] = !boolVars[controlIndex];
                    break;
                case ConsoleKey.RightArrow:
                    boolVars[controlIndex] = !boolVars[controlIndex];
                    break;
                default:
                    removeNops = boolVars[0];
                    removeAntiDe4dots = boolVars[1];
                    renameDllImports = boolVars[2];
                    disableProtections = boolVars[3];
                    noBase64Decode = !boolVars[4];
                    commentProtections = boolVars[5];
                    printAll = boolVars[6];
                    preserveEverything = !boolVars[7];
                    fixBasicMath = boolVars[8];
                    return;
            }
            editArgs();
        }

        private static void PrintFromBool(bool value, int currentPosition, int expectedPosition, string moreInfo = "", ConsoleColor moreInfoColor = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (value)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            if (value)
            {
                Console.Write(value.ToString() + " ");
            }
            else
            {
                Console.Write(value.ToString());
            }
            Console.ForegroundColor = ConsoleColor.White;
            if (currentPosition == expectedPosition)
            {
                Console.Write(" < | ");
            }
            else
            {
                Console.Write("   | ");
            }
            Console.ForegroundColor = moreInfoColor;
            Console.WriteLine(moreInfo);
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void SplashScreen()
        {
            Console.Title = "Junk Remover by OFF_LINE";
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
            Console.WriteLine("  |- DllImport Method Renamer");
            Console.WriteLine("  |- Base64 String Fixer");
            Console.WriteLine("  |- CUI Improvements & Fixes");
            Console.WriteLine();
        }

        private static void Main(string[] args)
        {
            string text = "";
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
            }
            catch (Exception)
            {
            }
            editArgs();
            Program.Asmpath = Path.GetFullPath(text);
            if (removeNops)
            {
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
                Console.WriteLine(" " + removed + " Nop instructions removed!");
                Console.WriteLine();
            }
            if (removeAntiDe4dots)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" Removing fake attributes...");
                removeshit();
                removeshit();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(" " + countofths + " FakeAttributes/AntiDe4dot cases removed!");
                Console.WriteLine();
            }
            if (disableProtections || fixBasicMath)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (fixBasicMath)
                {
                    Console.WriteLine(" Checking for possible protections & fixing math calculations...");
                }
                else
                {
                    Console.WriteLine(" Checking for possible protections...");
                }
                Console.WriteLine();
                tryClearProtections();
                if (fixBasicMath)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(" " + mathFixed + " Math calculations simplified!");
                    Console.WriteLine();
                }
            }
            if (!noBase64Decode)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (!printAll) { Console.WriteLine(); }
                Console.WriteLine(" Checking for Base64 Strings (might take a bit)...");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" " + decodeBase64Strings() + " Base64 Strings decoded!");
                Console.WriteLine();
            }
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            try
            {
                if (isILOnly)
                {
                    Console.WriteLine(" Now saving " + Path.GetFileNameWithoutExtension(args[0]) + "-Native" + Path.GetExtension(args[0]) + " (Native)...");
                    Program.module.Write(text3, moduleWriterOptions);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" (IL) '" + Path.GetFileNameWithoutExtension(args[0]) + "-IL" + Path.GetExtension(args[0]) + "' successfully saved!");
                }
                else
                {
                    Console.WriteLine(" Now saving " + Path.GetFileNameWithoutExtension(args[0]) + "-Native" + Path.GetExtension(args[0]) + " (Native)...");
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
            Console.Write(" Press any key to exit...");
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
        private static int decodeBase64Strings()
        {
            int countBase64Strings = 0;
            foreach (TypeDef t_ in module.Types)
            {
                foreach (MethodDef method in t_.Methods)
                {
                    List<string> instString = new List<string>();
                    if (!method.HasBody) { continue; }
                    for (int x = 0; x < method.Body.Instructions.Count; x++)
                    {
                        Instruction inst = method.Body.Instructions[x];
                        if (inst.OpCode.Equals(OpCodes.Ldstr))
                        {
                            if (!(inst.Operand.ToString() == ""))
                            {
                                try
                                {
                                    string oldOp_ = inst.Operand.ToString();
                                    byte[] temp_ = null;
                                    if (IsBase64String(oldOp_))
                                    {
                                        inst.Operand = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(inst.Operand.ToString()));
                                        printRenamed("'" + oldOp_ + "' -> '" + inst.Operand.ToString() + "'", "Base64 String");
                                        countBase64Strings++;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
            return countBase64Strings;
        }

        public static bool IsBase64String(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;

            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,2}$", RegexOptions.None);

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
                        printRenamed("'" + method.Name + "' -> '" + method.ImplMap.Name + "_" + Path.GetFileNameWithoutExtension(method.ImplMap.Module.Name.ToLower()) + "'", "DllImport method");
                        method.Name = method.ImplMap.Name + "_" + Path.GetFileNameWithoutExtension(method.ImplMap.Module.Name.ToLower());
                        if (method.ImplMap.Name == "VirtualProtect" && method.ImplMap.Module.Name.ToString().ToLower().Contains("kernel32"))
                        {
                            implMapMethodName = method.Name;
                        }
                    }
                }
            }
            if (printAll)
            {
                Console.WriteLine();
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
                        // Simple Math Solver - Code taken from my mod of PointMutationCleaner (https://github.com/miso-xyz/MutationCleaner)
                        if (fixBasicMath)
                        {
                            if (inst.OpCode.Equals(OpCodes.Xor) || inst.OpCode.Equals(OpCodes.Mul) || inst.OpCode.Equals(OpCodes.Add) || inst.OpCode.Equals(OpCodes.Sub))
                            {
                                if (method.Body.Instructions[x - 1].OpCode.Equals(OpCodes.Ldc_I4) && method.Body.Instructions[x - 2].OpCode.Equals(OpCodes.Ldc_I4))
                                {
                                    int endCalc = -1;
                                    int typeCalc = -1;
                                    switch (inst.OpCode.ToString())
                                    {
                                        case "xor":
                                            typeCalc = 0;
                                            endCalc = int.Parse(method.Body.Instructions[x - 2].Operand.ToString()) ^ int.Parse(method.Body.Instructions[x - 1].Operand.ToString());
                                            break;
                                        case "mul":
                                            typeCalc = 1;
                                            endCalc = int.Parse(method.Body.Instructions[x - 2].Operand.ToString()) * int.Parse(method.Body.Instructions[x - 1].Operand.ToString());
                                            break;
                                        case "add":
                                            typeCalc = 2;
                                            endCalc = int.Parse(method.Body.Instructions[x - 2].Operand.ToString()) + int.Parse(method.Body.Instructions[x - 1].Operand.ToString());
                                            break;
                                        case "sub":
                                            typeCalc = 3;
                                            endCalc = int.Parse(method.Body.Instructions[x - 2].Operand.ToString()) - int.Parse(method.Body.Instructions[x - 1].Operand.ToString());
                                            break;
                                    }
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    switch (typeCalc)
                                    {
                                        case 0:
                                            Console.WriteLine(" Calculation fixed '" + method.Body.Instructions[x - 2].Operand.ToString() + " ^ " + method.Body.Instructions[x - 1].Operand.ToString() + "' -> '" + endCalc + "'!");
                                            break;
                                        case 1:
                                            Console.WriteLine(" Calculation fixed '" + method.Body.Instructions[x - 2].Operand.ToString() + " * " + method.Body.Instructions[x - 1].Operand.ToString() + "' -> '" + endCalc + "'!");
                                            break;
                                        case 2:
                                            Console.WriteLine(" Calculation fixed '" + method.Body.Instructions[x - 2].Operand.ToString() + " + " + method.Body.Instructions[x - 1].Operand.ToString() + "' -> '" + endCalc + "'!");
                                            break;
                                        case 3:
                                            Console.WriteLine(" Calculation fixed '" + method.Body.Instructions[x - 2].Operand.ToString() + " - " + method.Body.Instructions[x - 1].Operand.ToString() + "' -> '" + endCalc + "'!");
                                            break;
                                    }
                                    Instruction calculated = new Instruction(OpCodes.Ldc_I4, endCalc);
                                    method.Body.Instructions.RemoveAt(x - 2);
                                    method.Body.Instructions.RemoveAt(x - 2);
                                    method.Body.Instructions.RemoveAt(x - 2);
                                    method.Body.Instructions.Insert(x - 2, OpCodes.Ldc_I4.ToInstruction(endCalc));
                                    mathFixed++;
                                }
                            }
                        }
                        // end of math solver
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
