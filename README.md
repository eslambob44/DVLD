# 🪪 DVLD
DVLD is a desktop application designed to manage driving licenses, from taking tests to issuing, renewing, replacing lost or damaged licenses, revoking them, and paying fines.

## :technologist:Technologies
- Using SQL Server for data storage
- Using ADO.NET to communicate with SQL Server and fetch data
- Using a three-tier architecture
- Using WinForms for the frontend

## :rocket:Features
- Ability to apply for new local and international licenses (to obtain an international license, you must first hold a valid local license, and international licenses are only issued for Class 3 ordinary cars)
- Recording test results
- Renewing licenses
- Replacing licenses if lost or damaged
- Detaining licenses to enforce a fee
- Releasing licenses after the fee is paid

## :arrows_counterclockwise: The Process
This project was part of [Programming advices](https://programmingadvices.com/) course.  
We began by reading the requirements, and then we designed the entire database. After that, the instructor split the project requirements into pieces and gave us tasks covering all the requirements one by one. Once we finished the project on our own, we watched the instructor's videos about his solution to learn from him.

## :books: What I Learned
- The importance of splitting the requirements into smaller pieces and implementing each piece separately
- The instructor's experience on how to write clean, scalable code
- This was my first large project, and I learned how to manage projects of this scale (large relative to my experience, not corporate scale)

## :vertical_traffic_light:Running the project
When you open the repo for the first time,you will face this 

<img width="634" height="279" alt="image" src="https://github.com/user-attachments/assets/641b2a3f-0329-4cb1-8f1d-992f97f883eb" />

First, let's initialize the database:
1. Create database called DVLD
2. Open `Create tables and views script.sql` and run it. This creates all the tables needed to make the application work.
3. Next, we need to populate the database with initial data. Open `Data script.sql` and run it

Now you have the necessary data to start.

The last thing left to do is configure the connection string:
Go to `DVLD-DataAccessLayer -> clsDataAccessLayerSettings.cs`.
You will see this code:
```
static internal class clsDataAccessLayerSettings
{
    public static string ConnectionString { get; } = "Enter your database connection string here";
}
```

Replace the string here with your own local connection string.

Now that you have everything ready, you can launch the app from `DVLD-PresentationLayer -> DVLD-PresentationLayer.sln`.

You can log in using these credentials:  
UserName = Admin, Password = 1234



