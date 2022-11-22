using System.Text.RegularExpressions;

namespace NSKurtsTools;

public partial class KurtsTools{
    /**
     * <summary>Generates a unique name</summary>
     * <param name="mask">String with a placeholder {#}</param>
     * <param name="nameExistsFunc">Callback function to check if a name is unique</param>
     * <returns>String based on mask, where {#} is replaces with an integer</returns>
     * <example><code>bool NameExists(string aName){
     *      return File.Exists(aName); 
     * }
     * <para> </para>
     * string name=UniqueName("myFile{#}.txt",NameExists);
     * </code></example>
     */
    public static string UniqueName(string mask, Func<string, bool> nameExistsFunc){
        string[] a=UniqueNameRegex.Split(mask);
        
        if (a.Length == 1){ // no placeholder => set a as if there was a placeholder in the end 
            a = new[]{ mask, "" };
        }
        
        string nameCandidate= string.Join("", a);
        
        if (!nameExistsFunc(nameCandidate)) return nameCandidate;
        
        int index = 2;
        do{
            nameCandidate =string.Join(index.ToString(),a);
            index++;
        } while (nameExistsFunc(nameCandidate));

        return nameCandidate;
    }


    internal static readonly Regex UniqueNameRegex = new("{#+}");

}