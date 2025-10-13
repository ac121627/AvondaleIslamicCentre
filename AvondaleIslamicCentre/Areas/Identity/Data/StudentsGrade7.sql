SELECT s.FirstName, s.LastName, c.ClassName
FROM Students s
JOIN Class c ON s.ClassId = c.ClassId
WHERE c.ClassName = 'Grade 7';
