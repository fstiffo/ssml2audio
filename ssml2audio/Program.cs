using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;

// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace ssml2audio
{
    class Program
    {

        class Options
        {
            [Option('r', "read", Required = true, HelpText = "Input files to be processed.")]
            public IEnumerable<string> InputFiles { get; set; }

            // Omitting long name, defaults to name of property, ie "--verbose"
            [Option(
              Default = false,
              HelpText = "Prints all messages to standard output.")]
            public bool Verbose { get; set; }

            [Option("stdin",
              Default = false,
              HelpText = "Read from stdin")]
            public bool stdin { get; set; }

            [Value(0, MetaName = "offset", HelpText = "File offset.")]
            public long? Offset { get; set; }
        }

        static async System.Threading.Tasks.Task Main(string[] args)
        {

            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("Hello - no args");
                    break;
                case 1:
                    await TextToSpeech.SSML2AudioFile(args[0]);
                    break;
                default:
                    {
                        for (int i = 0; i < args.Length; i++)
                        {
                            await TextToSpeech.SSML2AudioFile(args[i]);
                            Console.WriteLine($"arg[{i}] = {args[i]}");
                        }

                        break;
                    }
            }
            Console.WriteLine("Press a key to continue: ");
            Console.ReadLine();
        }
    }
}
