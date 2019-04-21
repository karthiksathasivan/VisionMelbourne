
BULK INSERT [dbo].SensorLocations
FROM 'C:\Users\Karth\source\repos\VisionMelbourneV3\sensorlocations.csv'
WITH (
      FIRSTROW=2,
      FIELDTERMINATOR = ',', 
      ROWTERMINATOR = '\n')

GO