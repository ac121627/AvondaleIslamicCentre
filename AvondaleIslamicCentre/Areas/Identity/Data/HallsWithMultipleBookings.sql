SELECT h.Name AS HallName, COUNT(b.BookingId) AS TotalBookings, 
       FORMAT(MAX(b.StartDateTime), 'yyyy-MM-dd') AS LatestBookingDate
FROM Hall h
JOIN Booking b ON h.HallId = b.HallId
GROUP BY h.Name
HAVING COUNT(b.BookingId) >= 3;
