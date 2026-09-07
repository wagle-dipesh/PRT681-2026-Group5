# Task Manager Console Application

## Overview

This project is a C# console application that connects to SQL Server Express and performs CRUD operations on a Tasks table.

It was developed as the Week 6 developer practice task.

## Technologies

- C#
- .NET
- SQL Server Express
- SQL Server Management Studio
- Microsoft.Data.SqlClient

## Features

The application allows users to:

- Create a new task
- View all tasks
- Update an existing task
- Mark a task as completed or pending
- Delete an existing task
- Validate user input

## Database

The application uses a SQL Server database named:

`TaskManagerDb`

The `Tasks` table contains:

- Id
- Title
- Description
- IsCompleted
- CreatedAt

The database creation script is available in:

`Database/SetupDatabase.sql`

## Database Connection

The application connects to the following local SQL Server Express instance:

`.\SQLEXPRESS`

Windows Authentication is used for the connection.

## Running the Application

1. Install .NET and SQL Server Express.
2. Open `Database/SetupDatabase.sql` in SQL Server Management Studio.
3. Execute the script to create the database and table.
4. Open a terminal in the project folder.
5. Restore the dependencies:

   ```cmd
   dotnet restore