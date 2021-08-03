using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

var input = new [] {
    "Fd1265ca4294", "Eff4", "db9485b22f0e",
    "D5d4acf1-9175-4ff4-9938-39cf9e0b706b"
};
var iterations = int.TryParse(args.Length > 0 ? args[0] : "1", out int attempts) ? attempts : 1;
var showFaults = bool.TryParse(args.Length > 1 ? args[1] : "true", out bool print) ? print : true;

Console.WriteLine("Week 1.3");

RunApp(input, new Func<string, string[]>(ValidateWithIfStatements), iterations, showFaults);
RunApp(input, new Func<string, string[]>(ValidateWithRegExpStatements), iterations, showFaults);

static void RunApp(string[] values, Func<string, string[]> validationMethod, int iterations, bool showFaults)
{
    if(values == null)
    {
        return;
    }
    
    var startTime = DateTime.Now;
    var startMemory = Process.GetCurrentProcess().WorkingSet64;

    for(var iteration=0; iteration<iterations; iteration++)
    {
        foreach(var id in values)
        {
            var errores = validationMethod(id);
            PrintErrors(id, errores, showFaults);
        }
        if(!showFaults)
        {
            Console.Write(iteration % 10 == 0 ? "*" : ".");
        }
    }
    
    ProcessUsage(validationMethod.Method.Name, startTime, startMemory);
}

static string[] ValidateWithIfStatements(string id)
{
    List<string> errors = new List<string>();
    id = CleanIdParameter(id);

    if (id.Length < 5 || id.Length > 32)
    {
        errors.Add("A valid ID must have a minimum of 5 characters and a maximum of 32");
    }
    if (id.Length > 0 && !(id[0] >= 'A' && id[0] <= 'Z'))
    {
        errors.Add("A valid ID must start with a capital letter: A-Z");
    }
    return errors.ToArray();
}

static string[] ValidateWithRegExpStatements(string id)
{
    var pattern_rule1 = @"^\w{5,32}$";
    var pattern_rule2 = @"[A-Z]";
    List<string> errors = new List<string>();

    if (!Regex.IsMatch(id = CleanIdParameter(id), pattern_rule1))
    {
        errors.Add("A valid ID must have a minimum of 5 characters and a maximum of 32");
    }
    if (id.Length > 0 && !Regex.IsMatch(id.Substring(0, 1), pattern_rule2))
    {
        errors.Add("A valid ID must start with a capital letter: A-Z");
    }
    return errors.ToArray();
}

static string CleanIdParameter(string id) 
{
    return (id != null && id.Length > 0) ? id : string.Empty;
}

static void PrintErrors(string id, string[] errors, bool showFaults)
{
    if(!showFaults)
    {
        return;
    }
    var summary = errors?.Length > 0 ? string.Empty : "OK";
    Console.WriteLine($"Id {id}, validation results: {summary}");
    foreach(var error in errors)
    {
        Console.WriteLine($"* {error}");
    }
    Console.WriteLine();
}

static void ProcessUsage(string methodName, DateTime startTime, long startMemory)
{
    var duration = DateTime.Now.Subtract(startTime).TotalMilliseconds;
    var memoryUsage = (Process.GetCurrentProcess().WorkingSet64 - startMemory) / 1024;

    Console.WriteLine();
    Console.WriteLine($"# Method {methodName} duration {duration} ms, memory usage {memoryUsage} kb");
}