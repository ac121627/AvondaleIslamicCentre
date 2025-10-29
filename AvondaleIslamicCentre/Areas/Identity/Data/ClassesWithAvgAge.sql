SELECT c.ClassName, 
       COUNT(s.StudentId) AS TotalStudents, 
       AVG(DATEDIFF(YEAR, s.DateOfBirth, GETDATE())) AS AverageAge
FROM Class c
JOIN Students s ON c.ClassId = s.ClassId
GROUP BY c.ClassName
