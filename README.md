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

| [![](https://i.ibb.co/bs3vCPX/Main-Window-8-16-2019-11-59-01-AM.png)](https://i.ibb.co/bs3vCPX/Main-Window-8-16-2019-11-59-01-AM.png)  | [![](https://i.ibb.co/LPQSZHx/Main-Window-8-16-2019-12-01-45-PM.png)](https://i.ibb.co/LPQSZHx/Main-Window-8-16-2019-12-01-45-PM.png) |
|:---:|:---:|

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
Then if you want, you can simply run the program with [client-file WpfApp2/bin/Debug/UI.exe](WpfApp2/bin/Debug/UI.exe), [server file-file Server/bin/Debug/Server.exe](Server/bin/Debug/Server.exe)  or you can rewrite the program. To do that open [file Messenger.sln](Messenger.sln) with the [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) or Visual Studio 2017.

**Note:** see [Prerequisites](#prerequisites) to find out requiered files.

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
