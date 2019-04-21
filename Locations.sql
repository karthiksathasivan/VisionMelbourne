
BULK INSERT [dbo].Locations
FROM 'C:\Users\Karth\source\repos\VisionMelbourneV3\landmarks_cleaned.csv'
WITH (
      FIRSTROW=2,
      FIELDTERMINATOR = ',', 
      ROWTERMINATOR = '\n')

GO
