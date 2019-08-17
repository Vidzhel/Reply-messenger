# Reply - Messenger
This is a C# based application powered by .NET and WPF frameworks that provides the main functions you would expect from messenger such as creating chats(public, private), sending messages, sending photos, search. The project consists of two parts: the server and the user application.

![illustration](https://i.ibb.co/B2hw5Hs/Illustrations2.png)

## Table of contents
* [I have learned](#i-have-learned)
* [Features](#features)
* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
* [Built With](#built-with)

| [![](https://i.ibb.co/RcbYh3N/Main-Window-8-16-2019-11-56-56-AM.png)](https://i.ibb.co/RcbYh3N/Main-Window-8-16-2019-11-56-56-AM.png)  | [![](https://i.ibb.co/h1hp6Hr/Main-Window-8-16-2019-11-57-33-AM.png)](https://i.ibb.co/h1hp6Hr/Main-Window-8-16-2019-11-57-33-AM.png) |
|:---:|:---:|

| [![](https://i.ibb.co/bs3vCPX/Main-Window-8-16-2019-11-59-01-AM.png)](https://i.ibb.co/bs3vCPX/Main-Window-8-16-2019-11-59-01-AM.png)  | [![](https://i.ibb.co/YWD2Lwc/Main-Window-8-16-2019-12-01-45-PM.png)](https://i.ibb.co/YWD2Lwc/Main-Window-8-16-2019-12-01-45-PM.png) |
|:---:|:---:|

<p align="center"><a href="https://i.ibb.co/bdk4dRp/Server-Client-Modules-Diagram.png"><img src="https://i.ibb.co/bdk4dRp/Server-Client-Modules-Diagram.png" width="700" alt="Server classes diagram"/></a></p>

<p align="center"><a href="https://i.ibb.co/Srgbhrv/Client-Classes-Diagram-1.png"><img src="https://i.ibb.co/Srgbhrv/Client-Classes-Diagram-1.png" width="700" alt="Server classes diagram"/></a></p>

<p align="center"><a href="https://i.ibb.co/KyBby86/Client-Classes-Diagram-2.png"><img src="https://i.ibb.co/KyBby86/Client-Classes-Diagram-2.png"/></a></p>

## I have learned

- OOP;
- Modeling the application architecture;
- Programming patterns;
- Working with the Version Control System(GIT);
- Building classes diagrams;
- Working with threads;
- Dealing with sockets;
- Using of events;
- Managing the SQLite database;

## Features

- Signing Up;
- Signing In;
- Creating or deleting chats(private, public);
- Synchronizing with server;
- User or group searching;
- Adding users to the contacts list;
- Changing profile info;
- Changing the profile photo and the group photo;
- Changing the user password;
- Sending or deleting messages;
- Attaching files or images to a message;

## Getting Started
To use the application, firstly, you will need to copy the entire repository.
Open bash, go to the folder where you would like to store files and write:
```
git clone https://github.com/Vidzhel/desktop_messenger.git
```
Then if you want, you can simply run the program with [client-file WpfApp2/bin/Debug/UI.exe](WpfApp2/bin/Debug/UI.exe), [server-file Server/bin/Debug/Server.exe](Server/bin/Debug/Server.exe)  or you can rewrite the program. To do that open [file Messenger.sln](Messenger.sln) with the [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) or Visual Studio 2017.

All images and data are storing in the same folder as the .exe files under [WpfApp2/bin/Debug/Reply Messenger/](WpfApp2/bin/Debug/Reply%20Messenger/) (Client) and [Server/bin/Debug/Reply Messenger Server/](Server/bin/Debug/Reply%20Messenger%20Server/) (Server) folders

**Note:** see [Prerequisites](#prerequisites) to find out requiered files.

Without any further do you can connect to the server only on local computer (server and clients running on single computer), otherwise you need to have domen name to deal with dynamic ip address and change some data in the file:

[ClientLibs/Core/ConnectionToServer/AsynchronousServerConnection.cs](ClientLibs/Core/ConnectionToServer/AsynchronousServerConnection.cs
)
Replace line 105:
```
IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
```
With (do not forget set YOUR_DOMAIN_NAME):
```
foreach (var addr in Dns.GetHostEntry(YOUR_DOMAIN_NAME).AddressList)
{
    if (addr.AddressFamily == AddressFamily.InterNetwork)
        Console.WriteLine("IPv4 Address: {0}", addr)
}
```

### Prerequisites
You have to have installed ".NET desktop development" packages and .NET of version 4.6.1
To install them, you should open Visual Studio, then go to the upper tool-bar and choose "Tools" then "Get Tools and Features..." there you can find all you need.

## Built With
- C#
- XAML - A markup language for UI
- Ninject v3.3.4 - NuGet Package
- Fody v4.2.1 - NuGet Package
- PropertyChangedFody v2.6.0 - NuGet Package
- System.Data.SQLite.Core v1.0.110 - NuGet Package
- Visual Studio 2017 - IDE
- .NET v4.6.1 - Framework
- WPF - Framework
