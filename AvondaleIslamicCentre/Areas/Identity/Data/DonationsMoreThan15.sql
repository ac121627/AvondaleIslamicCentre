SELECT d.DonationType, 
       COUNT(d.DonationId) AS TotalDonations, 
       SUM(d.Amount) AS TotalAmount, 
       FORMAT(MIN(d.DateDonated), 'yyyy-MM-dd') AS FirstDonationDate
FROM Donations d
GROUP BY d.DonationType
HAVING SUM(d.Amount) > 15;
