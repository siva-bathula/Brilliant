using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using GenericDefs.DotNet;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml;

namespace AppRunner
{
    public class ProjectEnumBuilder
    {
        public static void GenerateDlls()
        {
            AppDomainSetup domainSetup = new AppDomainSetup()
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationName = AppDomain.CurrentDomain.SetupInformation.ApplicationName,
                LoaderOptimization = LoaderOptimization.MultiDomainHost,
                ShadowCopyFiles = "true"
            };
            AppDomain a = AppDomain.CreateDomain("AssemblyGenerator", null, domainSetup);
            a.DoCallBack(() =>
            {
                GenerateClassAssembly("BrilliantProblems", "BrilliantProblems", typeof(Brilliant.ISolve));
                GenerateClassAssembly("ProjectEulerProblems", "ProjectEulerProblems", typeof(ProjectEuler.IProblem));
                GenerateProblemXsdAssembly();
            });
            AppDomain.Unload(a);
        }

        internal static void GenerateProblemXsdAssembly()
        {
            string savePath = StreamHelper.GetDynamicallyBuiltAssembliesPath(Assembly.GetExecutingAssembly());

            AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyName name = new AssemblyName("ProblemSets");
            AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave, savePath);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name.Name, name.Name + ".dll");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("Problem", TypeAttributes.Public | TypeAttributes.Serializable);
            PropertyBuilder p1 = typeBuilder.DefineProperty("BrilliantProblem", PropertyAttributes.HasDefault, typeof(BrilliantProblems), null);

            // The property set and property get methods require a special
            // set of attributes.
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the "get" accessor method for CustomerName.
            MethodBuilder BrilliantProblemGetPropMthdBldr =
                typeBuilder.DefineMethod("getBrilliantProblemName", getSetAttr, typeof(BrilliantProblems), Type.EmptyTypes);

            ILGenerator ilg = BrilliantProblemGetPropMthdBldr.GetILGenerator();
            FieldBuilder fbbp = typeBuilder.DefineField("brilliantProblem", typeof(BrilliantProblems), FieldAttributes.Private);
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Ldfld, fbbp);
            ilg.Emit(OpCodes.Ret);

            // Define the "set" accessor method for CustomerName.
            MethodBuilder BrilliantProblemSetPropMthdBldr =
                typeBuilder.DefineMethod("setBrilliantProblemName", getSetAttr, null, new Type[] { typeof(BrilliantProblems) });

            ILGenerator ils = BrilliantProblemSetPropMthdBldr.GetILGenerator();

            ils.Emit(OpCodes.Ldarg_0);
            ils.Emit(OpCodes.Ldarg_1);
            ils.Emit(OpCodes.Stfld, fbbp);
            ils.Emit(OpCodes.Ret);

            // Last, we must map the two methods created above to our PropertyBuilder to 
            // their corresponding behaviors, "get" and "set" respectively. 
            p1.SetGetMethod(BrilliantProblemGetPropMthdBldr);
            p1.SetSetMethod(BrilliantProblemSetPropMthdBldr);

            PropertyBuilder p2 = typeBuilder.DefineProperty("PEProblem", PropertyAttributes.HasDefault, typeof(ProjectEulerProblems), null);
            // The property set and property get methods require a special
            // set of attributes.

            // Define the "get" accessor method for CustomerName.
            MethodBuilder PEProblemGetPropMthdBldr = typeBuilder.DefineMethod("getPEProblemName", getSetAttr, typeof(ProjectEulerProblems), Type.EmptyTypes);

            ILGenerator ilgpe = PEProblemGetPropMthdBldr.GetILGenerator();
            FieldBuilder fbpe = typeBuilder.DefineField("peProblem", typeof(ProjectEulerProblems), FieldAttributes.Private);
            ilgpe.Emit(OpCodes.Ldarg_0);
            ilgpe.Emit(OpCodes.Ldfld, fbpe);
            ilgpe.Emit(OpCodes.Ret);

            // Define the "set" accessor method for CustomerName.
            MethodBuilder PEProblemSetPropMthdBldr = typeBuilder.DefineMethod("setPEProblemName", getSetAttr, null, new Type[] { typeof(ProjectEulerProblems) });

            ILGenerator ilspe = PEProblemSetPropMthdBldr.GetILGenerator();

            ilspe.Emit(OpCodes.Ldarg_0);
            ilspe.Emit(OpCodes.Ldarg_1);
            ilspe.Emit(OpCodes.Stfld, fbpe);
            ilspe.Emit(OpCodes.Ret);

            // Last, we must map the two methods created above to our PropertyBuilder to 
            // their corresponding behaviors, "get" and "set" respectively. 
            p2.SetGetMethod(PEProblemGetPropMthdBldr);
            p2.SetSetMethod(PEProblemSetPropMthdBldr);

            typeBuilder.CreateType();
            assemblyBuilder.Save(name.Name + ".dll");
        }

        internal static void GenerateClassAssembly(string assemblyName, string enumCollectionName, Type t)
        {
            Assembly brilliant = t.Assembly;
            Type[] allTypes = brilliant.GetTypes();
            AssemblyName name = new AssemblyName(assemblyName);
            string savePath = StreamHelper.GetDynamicallyBuiltAssembliesPath(Assembly.GetExecutingAssembly());

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave, savePath);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name.Name, name.Name + ".dll");
            EnumBuilder enumBuilder = moduleBuilder.DefineEnum(enumCollectionName, TypeAttributes.Public, typeof(int));

            int c = -1;
            Type descType = typeof(ClassnameAttribute);
            ConstructorInfo myConstructorInfo = descType.GetConstructor(new Type[1] { typeof(string) });
            foreach (Type type in allTypes)
            {
                if (type.GetInterfaces().Contains(t))
                {
                    string cName = type.FullName.Replace("+", ".");
                    string enumName = cName.Replace(".", "_");
                    CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(myConstructorInfo, new object[1] { cName });
                    (enumBuilder.DefineLiteral(enumName, ++c)).SetCustomAttribute(attributeBuilder);
                }
            }

            enumBuilder.CreateType();
            assemblyBuilder.Save(name.Name + ".dll");
        }

        private static void ShowCompileErrors(object sender, ValidationEventArgs e)
        {
            QueuedConsole.WriteFormat("Validation Error: {0}", e.Message);
        }
    }
}