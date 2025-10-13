SELECT t.FirstName, t.LastName, COUNT(c.ClassId) AS TotalClasses
FROM Teachers t
JOIN Class c ON t.TeacherId = c.TeacherId
GROUP BY t.FirstName, t.LastName
HAVING COUNT(c.ClassId) > 1;
