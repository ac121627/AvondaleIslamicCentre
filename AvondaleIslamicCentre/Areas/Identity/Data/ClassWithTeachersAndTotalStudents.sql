SELECT c.ClassName, t.FirstName + ' ' + t.LastName AS TeacherName, COUNT(s.StudentId) AS StudentCount
FROM Class c
JOIN Teachers t ON c.TeacherId = t.TeacherId
LEFT JOIN Students s ON c.ClassId = s.ClassId
GROUP BY c.ClassName, t.FirstName, t.LastName
ORDER BY StudentCount DESC;
