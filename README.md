# Red's Inventory Management
This is a simple inventory management application.

## Program features
 - [Database connection](#database-connection)
 - Login 
 - Notifications
 - Tables
 - Lists

### Database connection
The program connects to a database file with ".mdf" extension using MS SQL LocalDB. [Microsoft SQL Server 2014 Express LocalDB](https://github.com/kjbartel/SqlLocalDB2014-Bootstrapper) is set as prerequisite in the Setup project, but the program works with other versions of LocalDb as well. When the application starts first time, a small window pops up and you have to create a new database file, or connect to an existing. You can't use the program without database connection. The program automatically stores the path of the database folder and the name of the database file in the DatabaseSettings.txt file in the application folder. You can change the database connection settings under the Settings => Database menu.

### Login
The program stores usernames and encrypted passwords in the "Users" table, and the passwords are encrypted with salted SHA-256 hashing. If the "Users" table is empty at the start of the program, a small window pops up, and you have to add the first user to the database. If there is at least one user in the "Users" table, you can enter the username and the password in the login window. You can Add/Modify/Remove users under the Settings => Users menu.

### Notifications
Program sends messages to the user via Notifications in the top right corner of the screen.
The notifications are based on this [GitHub project](https://github.com/IvanLeonenko/WPFGrowlNotification)

### Tables
Every table builds up similar way in the program with the same features.
You can Add/Edit/Delete:
 - Products
 - Partners
 - Incoming transactions
 - Outgoing transactions
 - Users

If you Add/Edit a record a smaller window pops up in the center of the screen. You can change the details of the record in the window, and you can save or cancel the changes with the two button at the bottom of the window.

### Lists
You can list:
 - Product's stock quantities
 - Partner's money transactions
