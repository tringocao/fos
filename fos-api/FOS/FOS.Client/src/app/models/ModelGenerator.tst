${
    // Enable extension methods by adding using Typewriter.Extensions.*
    using Typewriter.Extensions.Types;
	using System.IO;
	using System.Text.RegularExpressions;
   //using System.Windows.Input;

    // Uncomment the constructor to change template settings.
    //Template(Settings settings)
    //{
    //    settings.IncludeProject("Project.Name");
    //    settings.OutputExtension = ".tsx";
    //}
     static string debugInfo = "";
     string PrintDebugInfo(Typewriter.CodeModel.File f){
          return debugInfo;        
     }
    // Custom extension methods can be used in the template by adding a $ prefix e.g. $LoudName
    string LoudName(Property property)
    {
        return property.Name.ToUpperInvariant();
    }
    string Imports(Class c)
    {
        IEnumerable<Type> types = c.Properties
            .Select(p => p.Type)
            .Where(t => !t.IsPrimitive || t.IsEnum)
            .Select(t => t.IsGeneric ? t.TypeArguments.First() : t)
            .Where(t => t.Name != c.Name)
            .Distinct();
        return string.Join(Environment.NewLine, types.Select(t => $"import {{ {t.Name} }} from './{SpacesFromCamel(t.Name)}';").Distinct());
    }
	Template(Settings settings)
    {

        settings
           .IncludeProject("FOS.Model")
           .OutputFilenameFactory = (file) => {
                string[] fileParts = file.FullName.Split('\\');
                Boolean startNamespaceAppending = false;
                string prefixFolder = "";
                string last = fileParts.Last();
                // Extract correct namespace to create corresponding output directory, according to my target Angular/TypeScript project
                foreach (var item in fileParts) {
                    if (!item.Equals(last)) {
                        if (string.Equals(item, "Domain", StringComparison.OrdinalIgnoreCase)) {
                            startNamespaceAppending = true;
                        } else if (startNamespaceAppending) {
                            prefixFolder += SpacesFromCamel(item) + "/";
                        }
                    }
                }
                // Extract correct type name depending on its type (class, enum, interface, ...)
                string typeName = "undefined";
                if (file.Classes.Count > 0) {
                    typeName = file.Classes.First().Name;
                } else if (file.Interfaces.Count > 0) {
                    typeName = file.Interfaces.First().Name;
                } else if (file.Enums.Count > 0) {
                    typeName = file.Enums.First().Name;
                }
                string fileName = "..\\..\\..\\..\\..\\..\\fos-client\\src\\app\\models\\"+ SpacesFromCamel(typeName) + ".ts";
                // Creates target directory if it does not exist
                //Regex rgx = new Regex("^(.*)(FOS\\.[\\w\\.]+\\\\Models)(.*)$");
                //string pathFileName = Path.GetDirectoryName(file.FullName);
                //debugInfo = pathFileName;

                //pathFileName = rgx.Replace(pathFileName, "$1\\FOS.Client\\src\\app\\models\\" + prefixFolder);
                //if (!Directory.Exists(pathFileName)) {
                //    DirectoryInfo directoryInfo = Directory.CreateDirectory(pathFileName);
                //}
                return fileName;
            };
    }

    /* Transform a C# project directory name to a "typescript/client" directory name (no upper case, etc.)*/
    string SpacesFromCamel(string value)
    {
        if (value.Length > 0)
        {
            var result = new List<char>();
            char[] array = value.ToCharArray();
            foreach (var item in array)
            {
                if (char.IsUpper(item) && result.Count > 0)
                {
                    result.Add('-');
                }
                result.Add(char.ToLower(item));
            }

            return new string(result.ToArray());
        }
        return value;
    }
}

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    $Classes(*)[
          $Imports

    export class $Name$TypeParameters $BaseClass[extends $Name$TypeArguments] implements $Name$TypeArguments   {
        $Properties[   
        public $name: $Type = $Type[$Default];]
    }]
        $Enums(*)[
    export enum $Name {$Values[
       $Name = $Value,]
    }
    ] 
