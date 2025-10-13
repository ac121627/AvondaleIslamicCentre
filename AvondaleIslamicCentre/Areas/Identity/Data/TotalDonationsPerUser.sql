SELECT u.FirstName + ' ' + u.LastName AS Donor, SUM(d.Amount) AS TotalDonated
FROM Donations d
JOIN AspNetUsers u ON d.AICUserId = u.Id
GROUP BY u.FirstName, u.LastName
ORDER BY TotalDonated DESC;
