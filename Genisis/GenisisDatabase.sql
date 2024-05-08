Create table UserInformation(
Id Int Primary Key,
FullName VarChar(50) Not Null,
UserName VarChar(50) Not Null,
Email VarChar(50) Not Null,
Password VarChar(50) Not Null
);

--drop table [Categories]

Create Table Tournament(
[Id] [int] Primary Key NOT NULL,
[Name] [varchar](50) NOT NULL,
[Location] [varchar](50) NOT NULL,
StreetName VarChar(100) Not Null,
Duration VarChar(50) Not Null,
PitsPlayable int Not Null,
[Address] [varchar](50) NOT NULL,
[Type] [varchar](20) NOT NULL,
[StartDate] [date] NOT NULL,
[EndDate] [date] NOT NULL,
[Extension] [int] NULL,
[IsActive] [bit] NOT NULL,
);

CREATE TABLE [dbo].[Category](
	[Id] [int] Primary Key NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[TournamentId] [int] NOT NULL,
	)

INSERT INTO Tournament
(Id, Name, Location, StreetName, Duration, PitsPlayable, Address, Type, StartDate, EndDate, Extension, IsActive)
VALUES
(1, 'SA Championships', 'Kroonstad', 'Sunny Road 21', 'Week', 10, '123 Main St', 'Seniors', '2023-11-01', '2023-11-07', NULL, 1),
(2, 'Junior League', 'Pretoria', 'Green Lane 45', 'Day', 8, '456 Elm St', 'Juniors', '2023-11-15', '2023-11-15', 2, 1),
(3, 'Open Tournament', 'Johannesburg', 'Hilltop Ave 12', 'League', 12, '789 Pine St', 'Open', '2023-12-01', '2023-12-30', NULL, 1),
(4, 'Winter Challenge', 'Bloemfontein', 'Snowy Blvd 67', 'Week', 6, '101 Maple St', 'Seniors', '2024-06-01', '2024-06-07', 3, 0),
(5, 'Summer Fest', 'Durban', 'Beach Road 89', 'Day', 5, '202 Ocean Dr', 'Juniors', '2024-01-20', '2024-01-20', NULL, 1);
