SELECT h.Name AS HallName, COUNT(b.BookingId) AS TotalBookings
FROM Booking b
JOIN Hall h ON b.HallId = h.HallId
GROUP BY h.Name
ORDER BY TotalBookings DESC;
