SELECT b.BookingId, h.Name AS HallName, b.StartDateTime, b.EndDateTime
FROM Booking b
JOIN Hall h ON b.HallId = h.HallId;
