SELECT c.ClassName, COUNT(s.StudentId) AS StudentCount
FROM Class c
LEFT JOIN Students s ON c.ClassId = s.ClassId
GROUP BY c.ClassName
ORDER BY StudentCount DESC;
