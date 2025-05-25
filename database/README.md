Instructions to load these databases to your computer(bash)

To load the data in your computer, follow these steps
1. Initially, connect to the psql
   psql -U postgress
2. Then create a database
   CREATE DATABASE "<any name you desire but I went with unisolar-data>";
3. Then carry this operation out
   \i <path-to-the-sql-file>/unisolar.sql

