SELECT t.FirstName + ' ' + t.LastName AS TeacherName, COUNT(s.StudentId) AS TotalStudents
FROM Teachers t
JOIN Class c ON t.TeacherId = c.TeacherId
JOIN Students s ON s.ClassId = c.ClassId
GROUP BY t.FirstName, t.LastName
ORDER BY TotalStudents DESC;
