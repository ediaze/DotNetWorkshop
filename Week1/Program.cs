using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

var input = new [] {
    "Fd1265ca4294", "Eff4", "db9485b22f0e",
    "D5d4acf1-9175-4ff4-9938-39cf9e0b706b"
};
var showFaults = true;
var iterations = 1;

Console.WriteLine("Week 1.2");

RunApp(input, new Func<string, string[]>(ValidateWithIfStatements), iterations, showFaults);
RunApp(input, new Func<string, string[]>(ValidateWithRegExpStatements), iterations, showFaults);

static void RunApp(string[] values, Func<string, string[]> handler, int iterations, bool showFaults)
{
    if(values == null)
    {
        return;
    }
    
    var start = DateTime.Now;
    Console.WriteLine();

    for(var iteration=0; iteration<iterations; iteration++)
    {
        foreach(var id in values)
        {
            var errores = handler(id);
            if(showFaults)
            {
                PrintErrors(id, errores);
            }
        }
        Console.Write(iteration % 10 == 0 && iteration > 0 ? "#" : ".");
    }
    
    ProcessUsage(start);
}

static string[] ValidateWithIfStatements(string id)
{
    string currentMethodName = nameof(ValidateWithIfStatements);
    List<string> errors = new List<string>();
    id = (id != null && id.Length > 0) ? id : string.Empty;

    Console.Write($"+ [{currentMethodName}] ");

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
    id = (id != null && id.Length > 0) ? id : string.Empty;
    string currentMethodName = nameof(ValidateWithRegExpStatements);

    Console.Write($"+ [{currentMethodName}] ");

    if (!Regex.IsMatch(id, pattern_rule1))
    {
        errors.Add("A valid ID must have a minimum of 5 characters and a maximum of 32");
    }
    if (id.Length > 0 && !Regex.IsMatch(id.Substring(0, 1), pattern_rule2))
    {
        errors.Add("A valid ID must start with a capital letter: A-Z");
    }
    return errors.ToArray();
}

static void PrintErrors(string id, string[] errors)
{
    var summary = errors?.Length > 0 ? string.Empty : "OK";
    Console.WriteLine($"Id {id}, validation results: {summary}");
    foreach(var error in errors)
    {
        Console.WriteLine($"* {error}");
    }
    Console.WriteLine();
}

static void ProcessUsage(DateTime start)
{
    var currentProcess = Process.GetCurrentProcess();
    var totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
    var currentProcessName = Process.GetCurrentProcess().ProcessName;
    
    Console.WriteLine();
    Console.WriteLine($"Process duration ms: {DateTime.Now.Subtract(start).TotalMilliseconds}");
    Console.WriteLine($"Process memory kb: {totalBytesOfMemoryUsed / 1024}");
}