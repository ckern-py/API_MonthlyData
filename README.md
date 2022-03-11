# API_MonthlyData
This is a repo containing an Azure API I created that will take in a list of data and write it to an Azure Database.

## Description
This repo contains an Azure API I created that will take in a list of data and write it to an Azure Database. The API has three controllers. CheckHealth, CheckHealthDB, and MonthlyDataUsage. The CheckHealth and CheckHealthDB do exactly what you would expect. By calling them you can make sure the API is running, healthy, and that the connection to the Database is intact. The MonthlyDataUsage controller is the one that accepts requests and writes them to the Database. I have my Raspberry Pi set up to retrieve data from my router, aggregate the data, and then send a request to my API. The request coming in is a list of my internet usage statistics for that month. Each day in the month is its own object with info contained about that day within. Each day object contains what day of the month it was, what day of the week it was, how much internet data was received that day, and how much internet data was sent out that day. My having all this data sent to my API it allows me to keep historical data on internet usage, and easily compare days, months, or even years. 

## Install
Installing this application on your local machine should be simple. You need to make sure you have NET Core Version 3.1 installed. Then you can clone the repo in Visual Studio and open the solution file. 

## Use
This project is intended to be hosted and ran in Azure with the supporting infrastructure. You can run it locally for debugging or as a one off if needed. To run in Azure, you will need some infrastructure set up with it. You'll need the API hosted. Azure has a few different options, so you can choose the one you are most familiar with. After choosing how the API is hosted, you'll also need application insights and an Azure SQL Database at the very minimum. With it being hosted in Azure it will always be available for you to call. You won't have to do anything, and information will automatically be written to the database, when it's received. 

## License
[GNU GPLv3](https://choosealicense.com/licenses/gpl-3.0/)
