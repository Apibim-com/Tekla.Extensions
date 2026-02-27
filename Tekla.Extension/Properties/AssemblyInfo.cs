using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Tekla.Extensions")]
[assembly: AssemblyDescription("A set of Tekla Structures extensions.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Apibim SpA")]
[assembly: AssemblyProduct("Apibim Tekla Extensions")]
[assembly: AssemblyCopyright("Copyright © Apibim SpA 2025")]
[assembly: AssemblyTrademark("Apibim®")]
[assembly: AssemblyCulture("")]


// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("03c19a66-26c8-4572-a784-e0f59e513cd1")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

// Version-specific assembly versions based on build configuration
#if TEKLA2020
[assembly: AssemblyVersion("2020.0.1")]
[assembly: AssemblyFileVersion("2020.0.1")]
#elif TEKLA2021
[assembly: AssemblyVersion("2021.0.1")]
[assembly: AssemblyFileVersion("2021.0.1")]
#elif TEKLA2022
[assembly: AssemblyVersion("2022.0.1")]
[assembly: AssemblyFileVersion("2022.0.1")]
#elif TEKLA2023
[assembly: AssemblyVersion("2023.0.1")]
[assembly: AssemblyFileVersion("2023.0.1")]
#elif TEKLA2024
[assembly: AssemblyVersion("2024.0.1")]
[assembly: AssemblyFileVersion("2024.0.1")]
#elif TEKLA2025
[assembly: AssemblyVersion("2025.0.1")]
[assembly: AssemblyFileVersion("2025.0.1")]
#else
[assembly: AssemblyVersion("2023.0.1")]
[assembly: AssemblyFileVersion("2023.0.1")]
#endif
