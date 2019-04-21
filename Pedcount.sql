BULK INSERT [dbo].Pedcount
FROM 'C:\Users\Karth\source\repos\VisionMelbourneV3\PedCount.csv'
WITH (
      FIRSTROW=2,
      FIELDTERMINATOR = ',', 
      ROWTERMINATOR = '\n')

GO