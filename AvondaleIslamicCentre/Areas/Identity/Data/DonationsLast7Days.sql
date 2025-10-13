SELECT Amount, DonationType, DateDonated
FROM Donations
WHERE DateDonated >= DATEADD(DAY, -7, GETDATE());
