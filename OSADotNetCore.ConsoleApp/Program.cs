﻿// See https://aka.ms/new-console-template for more information
using OSADotNetCore.ConsoleApp;
using System.Data;
using System.Data.SqlClient;

Console.WriteLine("Hello, World!");

AdoDotNetExample adoDotNetExample = new AdoDotNetExample();
//adoDotNetExample.Read();
//adoDotNetExample.Create("Title", "Author", "Content");
//adoDotNetExample.Update(12, "Test Title", "Test Author", "Test Content");
//adoDotNetExample.Delete(12);
adoDotNetExample.Edit(10);
Console.ReadKey();